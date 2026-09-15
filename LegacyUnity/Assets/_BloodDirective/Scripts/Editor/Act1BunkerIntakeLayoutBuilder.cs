using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Creates an authored, visual-only bunker layout from the passed Act 1 intake foundation.
    /// </summary>
    public static class Act1BunkerIntakeLayoutBuilder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntake.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeLayout01.unity";
        private const string SourceRootName = "Act1_BunkerIntake_Root";
        private const string LayoutRootName = "Act1_BunkerIntakeLayout01_Root";
        private const string VisualLayoutRootName = "Act1BunkerIntake_AuthoredLayout_VisualOnly";
        private const string PrefabDirectory = "Assets/_BloodDirective/Prefabs/Environment/Act1Bunker";
        private const string VisualOnlyLayer = "VisualOnly";

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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 02 - Bunker Intake Layout")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                Act1BunkerIntakeBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeLayoutBuilder] Act 1 Production 01 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeLayoutBuilder] Act 1 Bunker Intake root was not found.");
                return;
            }

            root.name = LayoutRootName;
            Transform existingLayout = root.Find(VisualLayoutRootName);
            if (existingLayout != null)
                Object.DestroyImmediate(existingLayout.gameObject);

            GameObject layoutRoot = new GameObject(VisualLayoutRootName);
            layoutRoot.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            layoutRoot.transform.SetParent(root, false);

            BuildZoneLandmarks(layoutRoot.transform);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildZoneLandmarks(Transform parent)
        {
            GameObject wall = LoadPrefab("Act1Bunker_WallModule.prefab");
            GameObject console = LoadPrefab("Act1Bunker_ContainmentConsole.prefab");
            GameObject pipe = LoadPrefab("Act1Bunker_PipeModule.prefab");
            GameObject light = LoadPrefab("Act1Bunker_LightModule.prefab");

            Transform intake = CreateZone(parent, "Intake_VisualOnly");
            Place(wall, intake, new Vector3(-5.9f, 0f, -3.1f), Quaternion.Euler(0f, 90f, 0f));
            Place(console, intake, new Vector3(-5.58f, 0f, -3.2f), Quaternion.Euler(0f, 90f, 0f));
            Place(light, intake, new Vector3(0f, 2.82f, -3.25f), Quaternion.identity);

            Transform breach = CreateZone(parent, "BreachCheckpoint_VisualOnly");
            Place(wall, breach, new Vector3(5.9f, 0f, 0.8f), Quaternion.Euler(0f, -90f, 0f));
            Place(console, breach, new Vector3(5.58f, 0f, 1.35f), Quaternion.Euler(0f, -90f, 0f));
            Place(pipe, breach, new Vector3(-5.58f, 2.55f, 1.25f), Quaternion.identity);
            Place(light, breach, new Vector3(0f, 2.82f, 1.2f), Quaternion.identity);

            Transform annex = CreateZone(parent, "ContainmentAnnex_VisualOnly");
            Place(wall, annex, new Vector3(-0.95f, 0f, 8.82f), Quaternion.identity);
            Place(console, annex, new Vector3(-1.62f, 0f, 8.22f), Quaternion.Euler(0f, 180f, 0f));
            Place(pipe, annex, new Vector3(0f, 2.58f, 8.52f), Quaternion.Euler(0f, 90f, 0f));
            Place(light, annex, new Vector3(0f, 2.8f, 7.1f), Quaternion.identity);

            Transform calibration = CreateZone(parent, "CalibrationStation_VisualOnly");
            Place(console, calibration, new Vector3(1.62f, 0f, 8.22f), Quaternion.Euler(0f, 180f, 0f));
            Place(pipe, calibration, new Vector3(5.58f, 2.55f, 2.4f), Quaternion.identity);

            Transform extraction = CreateZone(parent, "ExtractionPassage_VisualOnly");
            Place(wall, extraction, new Vector3(5.9f, 0f, 3.6f), Quaternion.Euler(0f, -90f, 0f));
            Place(console, extraction, new Vector3(5.58f, 0f, 2.9f), Quaternion.Euler(0f, -90f, 0f));
        }

        private static Transform CreateZone(Transform parent, string name)
        {
            GameObject zone = new GameObject(name);
            zone.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            zone.transform.SetParent(parent, false);
            return zone.transform;
        }

        private static GameObject LoadPrefab(string fileName)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabDirectory + "/" + fileName);
            if (prefab == null)
                throw new System.InvalidOperationException("Missing required Act 1 bunker prefab: " + fileName);

            return prefab;
        }

        private static void Place(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.name = prefab.name + "_Instance_VisualOnly";
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
