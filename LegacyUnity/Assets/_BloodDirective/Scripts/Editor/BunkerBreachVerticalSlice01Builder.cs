using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Promotes the verified Prototype 12 gameplay loop into the first reusable vertical slice.
    /// </summary>
    public static class BunkerBreachVerticalSlice01Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype12.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/VerticalSlices";
        private const string ScenePath = SceneDirectory + "/BunkerBreachVerticalSlice01.unity";
        private const string SourceRootName = "MovementPrototype12_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice01_Root";

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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 01 - Bunker Breach")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype12Builder.BuildScene();

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice01Builder] Prototype 12 could not be found or built.");
                return;
            }

            EnsureFolder(SceneDirectory);

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice01Builder] Prototype 12 root was not found.");
                return;
            }

            root.name = SliceRootName;
            Selection.activeGameObject = root.gameObject;

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
                return;

            string parentPath = System.IO.Path.GetDirectoryName(folderPath)?.Replace('\\', '/');
            string folderName = System.IO.Path.GetFileName(folderPath);
            if (!string.IsNullOrEmpty(parentPath) && !string.IsNullOrEmpty(folderName))
                AssetDatabase.CreateFolder(parentPath, folderName);
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
