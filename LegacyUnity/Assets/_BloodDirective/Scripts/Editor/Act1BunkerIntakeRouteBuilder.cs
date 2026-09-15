using BloodDirective.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Promotes the validated five-space Bunker Intake route into an Act 1 production scene.
    /// </summary>
    public static class Act1BunkerIntakeRouteBuilder
    {
        private const string SourceScenePath = "Assets/_BloodDirective/Scenes/Prototypes/ExpandedMissionRoutePrototype01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeRoute01.unity";
        private const string SourceRootName = "ExpandedMissionRoutePrototype01_Root";
        private const string ProductionRootName = "Act1_BunkerIntakeRoute01_Root";
        private const string ProductionMarkerName = "Act1BunkerIntakeRoute_ProductionMarker";
        private const string LevelDirectory = "Assets/_BloodDirective/ScriptableObjects/Levels/Act1";
        private const string LevelPath = LevelDirectory + "/Act1_BunkerIntakeRoute.asset";
        private const string MissionPath = "Assets/_BloodDirective/ScriptableObjects/Missions/Act1_BunkerIntake.asset";
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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 06 - Bunker Intake Route")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SourceScenePath) == null)
                ExpandedMissionRoutePrototype01Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SourceScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeRouteBuilder] The expanded mission route prototype was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(SourceScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeRouteBuilder] Expanded mission route root was not found.");
                return;
            }

            root.name = ProductionRootName;
            Transform existingMarker = root.Find(ProductionMarkerName);
            if (existingMarker != null)
                Object.DestroyImmediate(existingMarker.gameObject);

            CreateProductionMarker(root);
            CreateOrUpdateLevelDefinition();

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateProductionMarker(Transform root)
        {
            GameObject marker = new GameObject(ProductionMarkerName);
            marker.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            marker.transform.SetParent(root, false);
        }

        private static void CreateOrUpdateLevelDefinition()
        {
            EnsureFolderExists("Assets/_BloodDirective/ScriptableObjects");
            EnsureFolderExists("Assets/_BloodDirective/ScriptableObjects/Levels");
            EnsureFolderExists(LevelDirectory);

            LevelDefinition level = AssetDatabase.LoadAssetAtPath<LevelDefinition>(LevelPath);
            if (level == null)
            {
                level = ScriptableObject.CreateInstance<LevelDefinition>();
                AssetDatabase.CreateAsset(level, LevelPath);
            }

            MissionDefinition mission = AssetDatabase.LoadAssetAtPath<MissionDefinition>(MissionPath);
            SerializedObject serializedLevel = new SerializedObject(level);
            serializedLevel.FindProperty("_levelId").stringValue = "act1_bunker_intake_route";
            serializedLevel.FindProperty("_displayName").stringValue = "Bunker Intake";
            serializedLevel.FindProperty("_biomeId").stringValue = "underground_bunker";
            serializedLevel.FindProperty("_roomCount").intValue = 5;
            serializedLevel.FindProperty("_mission").objectReferenceValue = mission;
            serializedLevel.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(level);
        }

        private static void EnsureFolderExists(string folderPath)
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
