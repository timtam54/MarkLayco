# Build & Deploy — Layco Software Suite

Step-by-step guide for building the MSI installer on Windows and publishing the download page.

All Windows steps must run on a Windows 10/11 machine (the WPF app, MSBuild, and WiX are Windows-only).

---

## Part 1 — One-time Windows setup

Do these once on the Windows machine you'll use.

You have two paths. Both produce the same MSI. Pick one and follow only that section.

| | Path A — VS Code (minimal) | Path B — Visual Studio (full IDE) |
|---|---|---|
| Total install size | ~1.5 GB | ~6 GB |
| Can edit code | ✓ | ✓ |
| Can build the MSI | ✓ (via terminal) | ✓ (one-click) |
| Can open `.sln` visually | ✗ | ✓ |
| WPF XAML designer | ✗ | ✓ |
| Recommended for | Just building the MSI from clean source | Editing the WPF UI |

---

### Path A — VS Code minimal install

#### 1A.1 Install Git
- Download: https://git-scm.com/download/win
- Accept defaults.

#### 1A.2 Install VS Code
- Download: https://code.visualstudio.com/
- Accept defaults.

#### 1A.3 Install Visual Studio 2022 Build Tools (NOT full VS — just the build engine)
- Download: https://visualstudio.microsoft.com/downloads/?q=build+tools
- Scroll to **"Tools for Visual Studio"** → **Build Tools for Visual Studio 2022** → Download.
- In the installer, tick the **".NET desktop build tools"** workload. That installs MSBuild + the .NET Framework 4.7.2 targeting pack.
- ~1 GB.

#### 1A.4 Install .NET SDK 8
- Download: https://dotnet.microsoft.com/download
- Pick **.NET 8 SDK** or newer.

#### 1A.5 Install WiX 5 CLI
Open PowerShell and run:
```powershell
dotnet tool install --global wix --version 5.0.2
```
Verify:
```powershell
wix --version
```
If `wix` is not recognised, close and reopen PowerShell (PATH refresh).

#### 1A.6 Clone the repo to the exact expected path
`Installer\Product.wxs` hardcodes `C:\MarkOlearly\Installer\stage\**`, so the repo **must** sit at `C:\MarkOlearly`.

In VS Code: File → Open Folder → `C:\` → then in the integrated terminal (Ctrl+`) :
```powershell
git clone https://github.com/timtam54/Layco42 C:\MarkOlearly
```
Then File → Open Folder → `C:\MarkOlearly`.

---

### Path B — Full Visual Studio install

#### 1B.1 Install Git
- Download: https://git-scm.com/download/win
- Accept defaults during install.

#### 1B.2 Install Visual Studio 2022 Community (free)
- Download: https://visualstudio.microsoft.com/vs/community/
- In the installer, tick the **".NET desktop development"** workload.
- ~6 GB download, ~20 min.

#### 1B.3 Install .NET SDK (for the WiX CLI tool)
- Download: https://dotnet.microsoft.com/download
- Pick **.NET 8 SDK** or newer.

#### 1B.4 Install WiX 5 CLI
Open PowerShell and run:
```powershell
dotnet tool install --global wix --version 5.0.2
```
Verify:
```powershell
wix --version
```
If `wix` is not recognised, close and reopen PowerShell (PATH needs refreshing).

#### 1B.5 Clone the repo to the exact expected path
`Installer\Product.wxs` hardcodes `C:\MarkOlearly\Installer\stage\**`, so the repo **must** sit at `C:\MarkOlearly`.

```powershell
git clone https://github.com/timtam54/Layco42 C:\MarkOlearly
```

---

## Part 2 — Build the MSI

Every time you change the app and want a new installer.

### 2.1 Build the app in Release mode

**Path A (VS Code):** Open the integrated terminal in VS Code (Ctrl+`) and run:
```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" `
  "C:\MarkOlearly\Layco Software Suite\Layco Software Suite.sln" /p:Configuration=Release
```

**Path B (Visual Studio):** Open `C:\MarkOlearly\Layco Software Suite\Layco Software Suite.sln`.
- Set the configuration dropdown to **Release**.
- Build → Build Solution (Ctrl+Shift+B).

Or from the command line on Path B:
```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  "C:\MarkOlearly\Layco Software Suite\Layco Software Suite.sln" /p:Configuration=Release
```

**Confirm the build succeeded:**
`C:\MarkOlearly\Layco Software Suite\bin\Release\AGI_Manage.exe` exists.

### 2.2 Build the MSI
```powershell
powershell -ExecutionPolicy Bypass -File C:\MarkOlearly\Installer\build-installer.ps1
```

Output: `C:\MarkOlearly\Installer\LaycoSoftwareSuiteSetup.msi`

### 2.3 Smoke test
Double-click the MSI on a clean machine (or the same machine after uninstalling). It should:
- Install to `C:\Program Files\Layco Software Suite`
- Add a Start Menu shortcut "Layco Software Suite"
- Run, skipping login, with a fresh SQLite DB at `C:\ProgramData\AGI\Inventory\YSS.db`

---

## Part 3 — Publish the MSI to GitHub Releases

The download page (`docs/index.html`) points at:
```
https://github.com/timtam54/Layco42/releases/latest/download/LaycoSoftwareSuiteSetup.msi
```
That URL only works once a release is published with the MSI attached using that exact filename.

### 3.1 Create a release
1. Browser → https://github.com/timtam54/Layco42/releases
2. Click **Draft a new release**.
3. Choose a tag: e.g. `v4.2.0` (create new).
4. Release title: `Layco Software Suite 4.2.0`.
5. Drag `LaycoSoftwareSuiteSetup.msi` into the "Attach binaries" box.
6. Click **Publish release**.

The filename on disk **must** be `LaycoSoftwareSuiteSetup.msi` (matches the link in `index.html`). If you rename it, edit the `href` in `docs/index.html` to match.

### 3.2 Verify the download link works
Open in a browser:
```
https://github.com/timtam54/Layco42/releases/latest/download/LaycoSoftwareSuiteSetup.msi
```
You should get a download prompt.

---

## Part 4 — Publish the download page (GitHub Pages, free)

The HTML page lives at `docs/index.html`. Enabling GitHub Pages serves it as a public website.

### 4.1 Enable GitHub Pages
1. Browser → https://github.com/timtam54/Layco42/settings/pages
2. **Source:** Deploy from a branch.
3. **Branch:** `main`, folder: `/docs`.
4. Click **Save**.
5. Wait ~1 minute. The page shows: *"Your site is live at https://timtam54.github.io/Layco42/"*.

### 4.2 Done
Share `https://timtam54.github.io/Layco42/` with anyone who needs the app. They click Download → get the MSI → install.

---

## Optional — Publish to Azure Static Web Apps instead

GitHub Pages is the simplest free host. If you specifically want it on Azure (e.g. to keep everything in one place):

1. Azure Portal → Create resource → **Static Web App**.
2. Plan: **Free**.
3. Source: GitHub → choose `Layco42` repo, `main` branch.
4. App location: `/docs`.
5. Skip API and output location (leave blank).
6. Create. Azure adds a GitHub Action that auto-deploys on every push.
7. You get a URL like `https://<name>.azurestaticapps.net`.

This is the only way to use a custom domain (e.g. `download.layco.com`) without paying for Pages Pro.

---

## Cost notes

- GitHub Releases: free, 2 GB per file limit.
- GitHub Pages: free, no bandwidth limit for normal use.
- Azure Static Web Apps Free tier: free, 100 GB bandwidth/month, 0.5 GB storage. Plenty for a download page.

If the MSI is large (>50 MB) and you expect heavy traffic, consider Azure Blob Storage with public read access for the MSI itself (~A$0.02/GB egress) and keep the HTML on Pages.

---

## Troubleshooting

**`build-installer.ps1` fails with "Release build not found"**
Build the solution in Release first (step 2.1).

**`wix` command not found**
Close and reopen PowerShell. If still missing, check `%USERPROFILE%\.dotnet\tools` is on `PATH`.

**MSBuild not found at the path in the build command**
The path depends on whether you installed full Visual Studio or just Build Tools. The two common locations are:
- Build Tools: `C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe`
- VS Community: `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`

Pick whichever matches your install. To find it generically:
```powershell
Get-ChildItem "C:\Program Files*\Microsoft Visual Studio\2022" -Recurse -Filter MSBuild.exe -ErrorAction SilentlyContinue | Select-Object -First 1
```

**MSI install fails with "another version is already installed"**
Uninstall the old version via Settings → Apps → Layco Software Suite.

**App won't run after install: "The application failed to start"**
Likely missing .NET Framework 4.7.2. Install: https://dotnet.microsoft.com/download/dotnet-framework/net472

**SmartScreen blocks the download**
Expected — the MSI is unsigned. Click "More info" → "Run anyway". Long-term fix: buy a code-signing certificate (~A$200/yr).
