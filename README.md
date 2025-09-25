# codex01

## Windows Browser Opener (Executable)

Dieses Repository enthält eine kleine .NET-Konsolenanwendung, die auf einem
Windows-System alle bekannten installierten Browser sucht und die angegebene
GitHub-URL darin öffnet. Standardmäßig wird die codex01-Repository-URL
verwendet, Sie können aber auch eine andere URL als Argument übergeben.

### Projekt strukturieren

Der Quellcode befindet sich in `src/OpenBrowsersExe`. Die Anwendung kann mit dem
.NET SDK gebaut werden und erzeugt ein eigenständiges Windows-Executable.

### Voraussetzungen

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* Ein Windows-System, auf dem Sie die EXE später ausführen möchten

### Build-Anleitung

```powershell
# Aus dem Projektverzeichnis heraus
cd src/OpenBrowsersExe

# Optional: Sicherstellen, dass die Abhängigkeiten restauriert werden
dotnet restore

# Build für Windows x64 als eigenständige EXE
dotnet publish -c Release -r win-x64 --self-contained true
```

Der veröffentlichte Build liegt anschließend unter
`src/OpenBrowsersExe/bin/Release/net8.0/win-x64/publish/OpenBrowsersExe.exe` und
kann direkt auf einem Windows-Rechner ausgeführt werden. Sie können `win-x86`
oder andere Runtime-IDs angeben, wenn Sie 32-Bit-Systeme oder ARM unterstützen
möchten.

### Verwendung

```powershell
# Standard-URL öffnen
OpenBrowsersExe.exe

# Eine eigene URL öffnen
OpenBrowsersExe.exe https://github.com/<ihr-account>/codex01
```

Die Anwendung erkennt derzeit Google Chrome, Microsoft Edge, Mozilla Firefox,
Brave, Opera und Vivaldi anhand ihrer Standard-Installationspfade. Weitere
Browser lassen sich durch Ergänzen des `Browsers`-Wörterbuchs in
`Program.cs` hinzufügen.
