using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Promotes the verified Bunker Breach mission into a one-scene Windows demo build target.
    /// </summary>
    public static class BunkerBreachVerticalSlice06Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05_1.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice06.unity";
        private const string SourceRootName = "BunkerBreachVerticalSlice05_1_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice06_Root";
        private const string WindowsBuildPath = "Builds/Windows/BloodDirective_BunkerBreachDemo.exe";

        [InitializeOnLoadMethod]
        private static void BuildSceneAfterCompile()
        {
            EditorApplication.delayCall += () =>
            {
                if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating)
                    return;

                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null)
                    BuildScene();
            };
        }

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 06 - Playable Demo")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice051Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice06Builder] Vertical Slice 05.1 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice06Builder] Vertical Slice 05.1 root was not found.");
                return;
            }

            root.name = SliceRootName;
            EditorSceneManager.SaveScene(scene, ScenePath);
            ConfigureDemoBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = root.gameObject;
        }

        [MenuItem("Blood Directive/Reboot/Build Windows Playable Demo")]
        public static void BuildWindowsDemo()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null)
                BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice06Builder] Playable Demo scene was not found or built.");
                return;
            }

            ConfigureDemoBuildSettings();
            string directory = Path.GetDirectoryName(WindowsBuildPath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = WindowsBuildPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };
            BuildPipeline.BuildPlayer(options);
        }

        private static void ConfigureDemoBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };
        }

        private static Transform FindTransform(Scene scene, string objectName)
        {
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                if (rootObject.name == objectName)
                    return rootObject.transform;

                Transform nested = FindTransform(rootObject.transform, objectName);
                if (nested != null)
                    return nested;
            }

            return null;
        }

        private static Transform FindTransform(Transform parent, string objectName)
        {
            if (parent == null)
                return null;

            foreach (Transform child in parent)
            {
                if (child.name == objectName)
                    return child;

                Transform nested = FindTransform(child, objectName);
                if (nested != null)
                    return nested;
            }

            return null;
        }
    }
}
