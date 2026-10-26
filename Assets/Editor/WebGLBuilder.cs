using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class WebGLBuilder
{
    const string OutputPath = "Builds/WebGL";
    const string ItchZipPath = "Builds/MushroomGame-itch.zip";

    [MenuItem("Build/WebGL")]
    public static void Build()
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"WebGL build succeeded: {Path.GetFullPath(OutputPath)} ({summary.totalSize / (1024 * 1024)} MB)");

            // itch.io wants a zip with index.html at its root
            if (File.Exists(ItchZipPath))
                File.Delete(ItchZipPath);
            ZipFile.CreateFromDirectory(OutputPath, ItchZipPath, System.IO.Compression.CompressionLevel.Optimal, false);
            Debug.Log($"itch.io zip: {Path.GetFullPath(ItchZipPath)}");
        }
        else
        {
            Debug.LogError($"WebGL build failed: {summary.result}");
            if (Application.isBatchMode)
                EditorApplication.Exit(1);
        }
    }
}
