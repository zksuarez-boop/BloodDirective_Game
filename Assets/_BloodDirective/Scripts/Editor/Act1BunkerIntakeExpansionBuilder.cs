using BloodDirective.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Promotes the passed large-map blockout into the Act 1 bunker intake production foundation.
    /// </summary>
    public static class Act1BunkerIntakeExpansionBuilder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/LevelBlockoutPrototype01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeExpansion01.unity";
        private const string SourceRootName = "LevelBlockoutPrototype01_Root";
        private const string ProductionRootName = "Act1_BunkerIntakeExpansion01_Root";
        private const string LevelDirectory = "Assets/_BloodDirective/ScriptableObjects/Levels/Act1";
        private const string LevelPath = LevelDirectory + "/Act1_BunkerIntakeExpansion.asset";
        private const string MissionPath = "Assets/_BloodDirective/ScriptableObjects/Missions/Act1_BunkerIntake.asset";
        private const string ProductionMarkerName = "Act1BunkerIntakeExpansion_ProductionMarker";
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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 05 - Bunker Intake Expansion")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                LevelBlockoutPrototype01Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeExpansionBuilder] Level Blockout Prototype 01 was not found or built.");
                return;
            }

            CreateLevelDefinition();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeExpansionBuilder] Level Blockout Prototype 01 root was not found.");
                return;
            }

            root.name = ProductionRootName;
            Transform existingMarker = root.Find(ProductionMarkerName);
            if (existingMarker != null)
                Object.DestroyImmediate(existingMarker.gameObject);

            GameObject marker = new GameObject(ProductionMarkerName);
            marker.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            marker.transform.SetParent(root, false);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateLevelDefinition()
        {
            MissionDefinition mission = AssetDatabase.LoadAssetAtPath<MissionDefinition>(MissionPath);
            if (mission == null)
                throw new System.InvalidOperationException("Missing required Act 1 mission definition.");

            LevelDefinition definition = AssetDatabase.LoadAssetAtPath<LevelDefinition>(LevelPath);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<LevelDefinition>();
                definition.name = "Act1_BunkerIntakeExpansion";
                AssetDatabase.CreateAsset(definition, LevelPath);
            }

            SerializedObject definitionSo = new SerializedObject(definition);
            definitionSo.FindProperty("_levelId").stringValue = "act1_bunker_intake_expansion";
            definitionSo.FindProperty("_displayName").stringValue = "Bunker Intake";
            definitionSo.FindProperty("_biomeId").stringValue = "underground_bunker";
            definitionSo.FindProperty("_roomCount").intValue = 5;
            definitionSo.FindProperty("_mission").objectReferenceValue = mission;
            definitionSo.ApplyModifiedPropertiesWithoutUndo();
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
