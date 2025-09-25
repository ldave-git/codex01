using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Codex01.OpenBrowsers;

internal static class Program
{
    private const string DefaultUrl = "https://github.com/your-org/codex01";

    private static readonly string[] DefaultRootEnvVars =
    {
        "PROGRAMFILES",
        "PROGRAMFILES(X86)",
    };

    private static IReadOnlyCollection<string> CandidatePaths(
        IEnumerable<string> segments,
        IEnumerable<string>? rootEnvVars = null)
    {
        var segmentArray = segments as string[] ?? segments.ToArray();
        var envVarNames = (rootEnvVars ?? DefaultRootEnvVars).ToArray();
        var results = new List<string>();

        foreach (var envVar in envVarNames)
        {
            var root = Environment.GetEnvironmentVariable(envVar);
            if (string.IsNullOrWhiteSpace(root))
            {
                continue;
            }

            var path = segmentArray.Aggregate(root!, Path.Combine);
            results.Add(path);
        }

        return results;
    }

    private static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> Browsers =
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            ["Google Chrome"] = CandidatePaths(new[]
            {
                "Google",
                "Chrome",
                "Application",
                "chrome.exe",
            }),
            ["Microsoft Edge"] = new[]
            {
                @"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",
                @"C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe",
            },
            ["Mozilla Firefox"] = CandidatePaths(new[]
            {
                "Mozilla Firefox",
                "firefox.exe",
            }),
            ["Brave"] = CandidatePaths(new[]
            {
                "BraveSoftware",
                "Brave-Browser",
                "Application",
                "brave.exe",
            }),
            ["Opera"] = new[]
                {
                    @"C:\\Program Files\\Opera\\launcher.exe",
                    @"C:\\Program Files\\Opera GX\\launcher.exe",
                }
                .Concat(
                    CandidatePaths(
                        new[]
                        {
                            "Programs",
                            "Opera",
                            "launcher.exe",
                        },
                        new[] { "LOCALAPPDATA" }))
                .ToArray(),
            ["Vivaldi"] = CandidatePaths(new[]
            {
                "Vivaldi",
                "Application",
                "vivaldi.exe",
            }),
        };

    private static string? ResolveBrowser(IEnumerable<string> pathCandidates)
    {
        foreach (var candidate in pathCandidates)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                continue;
            }

            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static void OpenBrowser(string executable, string url)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = url,
            UseShellExecute = false,
        };

        Process.Start(startInfo);
    }

    private static string GetUrl(string[] args) =>
        args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : DefaultUrl;

    private static int Main(string[] args)
    {
        var url = GetUrl(args);
        var openedAny = false;

        foreach (var (name, paths) in Browsers)
        {
            var browserPath = ResolveBrowser(paths);
            if (browserPath is null)
            {
                Console.WriteLine($"{name}: not installed or executable not found");
                continue;
            }

            try
            {
                OpenBrowser(browserPath, url);
                Console.WriteLine($"Opened {name}");
                openedAny = true;
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine($"Failed to open {name}: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Failed to open {name}: {ex.Message}");
            }
        }

        if (!openedAny)
        {
            Console.WriteLine("No known browsers were opened. Consider adding their paths to the list.");
        }

        return 0;
    }
}
