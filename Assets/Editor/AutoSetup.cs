#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

// Runs once when the project is first opened: imports TextMeshPro essentials and builds the whole game.
[InitializeOnLoad]
public static class AutoSetup
{
    const string TriedKey = "FairytaleFlight_AutoSetupTried";

    static AutoSetup()
    {
        EditorApplication.delayCall += Run;
    }

    static bool GameAlreadyBuilt()
    {
        return File.Exists("Assets/Scenes/Game.unity") && File.Exists("Assets/Scenes/MainMenu.unity");
    }

    static void Run()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
        if (GameAlreadyBuilt()) return;
        if (SessionState.GetBool(TriedKey, false)) return;
        SessionState.SetBool(TriedKey, true);

        if (!FairytaleFlightBuilder.TmpReady())
        {
            var info = UnityEditor.PackageManager.PackageInfo.FindForPackageName("com.unity.ugui");
            string path = info != null ? Path.Combine(info.resolvedPath, "Package Resources", "TMP Essential Resources.unitypackage") : null;
            if (path != null && File.Exists(path))
            {
                AssetDatabase.importPackageCompleted += OnImported;
                AssetDatabase.ImportPackage(path, false);
            }
            else
            {
                Debug.LogWarning("Fairytale Flight: run Window > TextMeshPro > Import TMP Essential Resources, then Fairytale Flight > Build Everything.");
            }
            return;
        }
        SafeBuild();
    }

    static void OnImported(string packageName)
    {
        AssetDatabase.importPackageCompleted -= OnImported;
        EditorApplication.delayCall += SafeBuild;
    }

    static void SafeBuild()
    {
        try { FairytaleFlightBuilder.BuildAll(false); }
        catch (System.Exception e) { Debug.LogError("Fairytale Flight auto-build failed: " + e); }
    }
}
#endif
