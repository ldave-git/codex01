"""Open the codex01 GitHub repository in every detected Windows browser."""
from __future__ import annotations

import argparse
import os
import subprocess
from pathlib import Path
from typing import Dict, Iterable, List, Sequence

DEFAULT_URL = "https://github.com/your-org/codex01"


DEFAULT_ROOT_ENV_VARS: Sequence[str] = (
    "PROGRAMFILES",
    "PROGRAMFILES(X86)",
)


def candidate_paths(*segments: str, root_env_vars: Sequence[str] | None = None) -> List[Path]:
    """Return normalized paths assembled from known environment roots."""
    roots = [os.environ.get(name) for name in (root_env_vars or DEFAULT_ROOT_ENV_VARS)]
    results: List[Path] = []
    for root in roots:
        if not root:
            continue
        results.append(Path(root).joinpath(*segments))
    return results


BROWSERS: Dict[str, Iterable[Path]] = {
    "Google Chrome": candidate_paths("Google", "Chrome", "Application", "chrome.exe"),
    "Microsoft Edge": [
        Path("C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe"),
        Path("C:/Program Files/Microsoft/Edge/Application/msedge.exe"),
    ],
    "Mozilla Firefox": candidate_paths("Mozilla Firefox", "firefox.exe"),
    "Brave": candidate_paths("BraveSoftware", "Brave-Browser", "Application", "brave.exe"),
    "Opera": [
        Path("C:/Program Files/Opera/launcher.exe"),
        Path("C:/Program Files/Opera GX/launcher.exe"),
        *candidate_paths(
            "Programs", "Opera", "launcher.exe", root_env_vars=("LOCALAPPDATA",)
        ),
    ],
    "Vivaldi": candidate_paths("Vivaldi", "Application", "vivaldi.exe"),
}


def resolve_browser(path_candidates: Iterable[Path]) -> Path | None:
    for candidate in path_candidates:
        if candidate.is_file():
            return candidate
    return None


def open_browser(executable: Path, url: str) -> None:
    subprocess.Popen([str(executable), url])


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Open the codex01 GitHub repository in every detected browser."
    )
    parser.add_argument(
        "url",
        nargs="?",
        default=DEFAULT_URL,
        help="Repository URL to open (defaults to the codex01 GitHub URL).",
    )
    return parser.parse_args()


def main() -> None:
    args = parse_args()
    opened_any = False
    for name, paths in BROWSERS.items():
        browser_path = resolve_browser(paths)
        if browser_path is None:
            print(f"{name}: not installed or executable not found")
            continue
        try:
            open_browser(browser_path, args.url)
            print(f"Opened {name}")
            opened_any = True
        except OSError as exc:
            print(f"Failed to open {name}: {exc}")
    if not opened_any:
        print("No known browsers were opened. Consider adding their paths to BROWSERS.")


if __name__ == "__main__":
    main()
