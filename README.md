# codex01

## Windows browser opener

Dieses Repository enthält das Skript `open_browsers.py`, das auf einem
Windows-System alle bekannten installierten Browser sucht und die angegebene
GitHub-URL darin öffnet. Standardmäßig wird die codex01-Repository-URL
verwendet, Sie können aber auch eine andere URL als Argument übergeben.

```powershell
py open_browsers.py https://github.com/<ihr-account>/codex01
```

Das Skript erkennt derzeit Google Chrome, Microsoft Edge, Mozilla Firefox,
Brave, Opera und Vivaldi anhand ihrer Standard-Installationspfade. Weitere
Browser lassen sich durch Ergänzen des `BROWSERS`-Wörterbuchs hinzufügen.
