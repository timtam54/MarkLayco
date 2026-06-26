# Layco Recipe Program (MarkLayco42)

Context document for Claude Code / developers. Summarises what this project is,
how it was created, every change made during the migration, and how to build &
run it. Written 2026-06-26.

## What this app is

A **.NET Framework 4.8 WinForms desktop application** for managing blending
**recipes** for an industrial feed/fertilizer blender. It talks to a **PLC** over
**Modbus TCP** (via the `PlcComms` project) to read hopper info and run blends,
and stores products / recipes / run-history in a local database.

Solution: `LaycoRecipeProgram.sln` — 10 projects:

| Project | Role |
|---|---|
| `LaycoRecipeProgram` | **Startup EXE** (WinForms). `MainWindow` = tabbed UI: Recipes / Products / History / Settings. x86. |
| `Database` | Data-access layer. **Now uses SQLite** (was SQL Server Compact). |
| `Recipes` | `ModifyRecipes` ("RecipeMaker") form — add/edit/delete recipes. |
| `Products` | Product management UI/logic. |
| `History` | Run-history UI/logic. |
| `Settings` | App settings, `InitialSetup` (setup screen), Backup/Restore, `LRP` settings class. |
| `PlcComms` | Modbus/PLC communication (`PlcInteraction`). |
| `PrintBatches` | Batch ticket printing. |
| `GeneralData` | Shared models (`Product`, etc.). |
| `SoftwareInteraction` | PLC polling / software interface glue. |

## Origin

Copied from `C:\Mark\LRP 4-27-15MarkEmail13032026` into `C:\MarkLayco42`,
**excluding** the redundant backup trees `LRP 4-27-15Copy` and `LRP 4-27-15USB`,
plus build artifacts (`bin/`, `obj/`, `.vs/`). The original remains untouched.

## Changes made (this is the important part)

### 1. Recipe "Save" button was always disabled — FIXED
Root cause: the setup had saved **`NumHoppers = 0`**. The RecipeMaker
(`Recipes/ModifyRecipes.cs`) builds **one Product+Ratio row per hopper**, so with
0 hoppers there were **no ratio fields**, the ratio total could never reach 100%,
and Save (`btnNext`, which only enables when the ratios total exactly 100) stayed
greyed forever. It was *not* a user-level/admin issue — this app has no concept of
users or admin at all.

Hoppers come from the PLC, but this PC has no PLC (`PlcIp = 0.0.0.0`), so the count
was never read. Guards added:
- `Recipes/ModifyRecipes.cs` → `makeRecipe()`: if `NumHoppers <= 0`, default to 8.
- `Settings/InitialSetup.cs` → `btnSave_Click`: empty/0 hopper count defaults to 8;
  empty PLC IP is normalised to `0.0.0.0`.

### 2. App crashed on startup with no PLC — FIXED
`MainWindow.setupSettings()` called `PlcInteraction.GetHopperNames()`, which throws
`ArgumentNullException` when there is no PLC IP. An **empty** IP slipped past the
old `!= "0.0.0.0"` guard. Fixes in `LaycoRecipeProgram/MainWindow.cs`:
- `setupSettings()`: guard is now `!string.IsNullOrWhiteSpace(PlcIp) && PlcIp != "0.0.0.0"`.
- `checkPlc()`: returns early (skips PLC polling) when IP is empty/`0.0.0.0`, so the
  background polling timer can't crash the app in offline/demo use.

### 3. Setup/login screen bypassed — DONE
`MainWindow.checkSetup()` no longer shows the `InitialSetup` dialog. When setup
isn't done it **auto-provisions defaults** (PlcIp `0.0.0.0`, 8 hoppers, ProgramData
paths, `LRP.db`) and proceeds straight to the main window.

### 4. Database migrated SQL Server Compact → SQLite — DONE
Only two files used the DB engine: `Database/Databases.cs` and
`Database/DatabaseViewer.cs`. Mechanical, behaviour-preserving swap:

- `System.Data.SqlServerCe` → **`System.Data.SQLite`** (package
  `Stub.System.Data.SQLite.Core.NetFramework` **1.0.119.0**).
- `SqlCeConnection/Command/DataReader/DataAdapter` → `SQLite*` equivalents.
- DB file `LRP.sdf` → **`LRP.db`** (all `.sdf` references across the solution —
  backup/restore in `Settings/Backup.cs`, `LaycoRecipeProgram/DbViewer.cs`,
  `MainWindow.cs`, `InitialSetup.cs` — updated to `.db`).
- Connection string: `Data Source=<path>;Version=3;` (the old CE
  `Password='yargus'` was dropped — fresh SQLite DB).
- DB creation: `SqlCeEngine.CreateDatabase()` → `SQLiteConnection.CreateFile()`.
- Table-existence check: `Information_Schema.Tables` → `sqlite_master`.
- `.csproj` (both `Database` and `LaycoRecipeProgram`): removed the SqlServerCe
  reference + native-binary post-build copy; added the `System.Data.SQLite`
  reference and an `Import` of the package's `.targets` (deploys the native
  `SQLite.Interop.dll` to `bin\...\x86`). `packages.config` updated in both.

**Data was NOT migrated** — the app starts with a fresh empty SQLite DB (schema
created on first run). The schema is identical (tables: `Products`, `RecipeList`,
`RecipeInfo`, `History`).

Verified working: app launches straight to the main window, creates
`C:\ProgramData\Yargus\LRP\LRP.db`, all 4 tables created, insert/read confirmed.

## Build

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  "C:\MarkLayco42\LaycoRecipeProgram.sln" /t:Rebuild /p:Configuration=Debug
```
The startup project (`LaycoRecipeProgram`) builds **Debug|x86**; the rest build
AnyCPU via the solution mapping. Don't pass `/p:Platform=x86` to the whole solution
(the class-library projects have no x86 config and it breaks the build) — just set
`Configuration`.

NuGet: `packages/` is gitignored. After cloning, Visual Studio restores
automatically, or run `nuget restore LaycoRecipeProgram.sln`. The SQLite package
1.0.119.0 is on nuget.org.

## Run

`C:\MarkLayco42\LaycoRecipeProgram\bin\Debug\LaycoRecipeProgram.exe`
Opens straight to the main window (no setup/login screen, no PLC needed).

- SQLite DB: `C:\ProgramData\Yargus\LRP\LRP.db`
- App settings (per-user): `%LOCALAPPDATA%\LaycpRecipeProgram\...\user.config`
  (note the historical `LaycpRecipeProgram` typo in the namespace/assembly metadata).

### To add a recipe (the original ask)
Recipes tab → **Add** → RecipeMaker shows 8 hopper rows → pick products and enter
ratios that total **100%** → **Save** enables. (Define products on the Products tab
first so the dropdowns are populated.)

## Git / GitHub

Local git repo is initialised and committed in `C:\MarkLayco42`, ready to push.
`gh` CLI is not installed here; push with the cached `timtam54` credentials, e.g.:

```powershell
git -C C:\MarkLayco42 remote add origin https://github.com/timtam54/<REPO>.git
git -C C:\MarkLayco42 push -u origin main
```

## Known notes / possible follow-ups
- No data migration from the old `LRP.sdf`; if existing products/recipes are needed,
  they must be exported from SQL Server Compact and imported into `LRP.db`.
- PLC features are inert offline (`PlcIp = 0.0.0.0`); set a real IP to use a PLC.
- The assembly/namespace is historically misspelled `LaycpRecipeProgram` — left as-is
  to avoid churn.
