using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Enforces the complete Bunker Breach mission dependency chain.
    /// </summary>
    public static class BunkerBreachVerticalSlice051Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05_1.unity";
        private const string SourceRootName = "BunkerBreachVerticalSlice05_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice05_1_Root";

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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 05.1 - Mission Gate Enforcement")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice05Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice051Builder] Vertical Slice 05 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            PrototypeScientistRescue scientist = FindTransform(scene, "ScientistRescue_Interactable")?.GetComponent<PrototypeScientistRescue>();
            PrototypeAetherPickup aether = FindTransform(scene, "AetherSample_Interactable")?.GetComponent<PrototypeAetherPickup>();
            PrototypeHealth guard = FindTransform(scene, "ExtractionGuard_Target")?.GetComponent<PrototypeHealth>();
            PrototypeLockedDoor door = FindTransform(scene, "ExtractionBlastDoor_Locked")?.GetComponent<PrototypeLockedDoor>();
            PrototypeExtractionZone extraction = FindTransform(scene, "ExtractionZone_Trigger")?.GetComponent<PrototypeExtractionZone>();
            if (root == null || scientist == null || aether == null || guard == null || door == null || extraction == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice051Builder] Required Vertical Slice 05 mission objects were not found.");
                return;
            }

            root.name = SliceRootName;
            ConfigureAetherGate(aether);
            ConfigureExtractionGate(extraction, scientist, guard);
            ConfigureExitGate(root, scientist, guard, door, extraction);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = root.gameObject;
        }

        private static void ConfigureAetherGate(PrototypeAetherPickup aether)
        {
            SerializedObject aetherSo = new SerializedObject(aether);
            aetherSo.FindProperty("_opensExtractionDoor").boolValue = false;
            aetherSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureExtractionGate(PrototypeExtractionZone extraction, PrototypeScientistRescue scientist, PrototypeHealth guard)
        {
            SerializedObject extractionSo = new SerializedObject(extraction);
            extractionSo.FindProperty("_requiredScientist").objectReferenceValue = scientist;
            extractionSo.FindProperty("_requiredGuard").objectReferenceValue = guard;
            extractionSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureExitGate(Transform root, PrototypeScientistRescue scientist, PrototypeHealth guard, PrototypeLockedDoor door, PrototypeExtractionZone extraction)
        {
            PrototypeMissionExitGate exitGate = root.gameObject.AddComponent<PrototypeMissionExitGate>();
            SerializedObject gateSo = new SerializedObject(exitGate);
            gateSo.FindProperty("_scientist").objectReferenceValue = scientist;
            gateSo.FindProperty("_requiredGuard").objectReferenceValue = guard;
            gateSo.FindProperty("_door").objectReferenceValue = door;
            gateSo.FindProperty("_extraction").objectReferenceValue = extraction;
            gateSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene.path == scenePath)
                    return;
            }

            var updatedScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            scenes.CopyTo(updatedScenes, 0);
            updatedScenes[updatedScenes.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = updatedScenes;
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
