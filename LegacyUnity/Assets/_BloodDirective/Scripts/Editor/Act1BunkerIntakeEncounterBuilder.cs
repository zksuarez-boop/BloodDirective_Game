using BloodDirective.Data;
using BloodDirective.Prototypes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Promotes the authored bunker layout into a data-mapped Act 1 encounter-production scene.
    /// </summary>
    public static class Act1BunkerIntakeEncounterBuilder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeLayout01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeEncounter01.unity";
        private const string SourceRootName = "Act1_BunkerIntakeLayout01_Root";
        private const string EncounterRootName = "Act1_BunkerIntakeEncounter01_Root";
        private const string AnchorRootName = "Act1BunkerIntake_EncounterAnchors";
        private const string EncounterDirectory = "Assets/_BloodDirective/ScriptableObjects/Encounters/Act1";
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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 03 - Bunker Intake Encounters")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                Act1BunkerIntakeLayoutBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeEncounterBuilder] Act 1 Production 02 was not found or built.");
                return;
            }

            CreateEncounterDefinitions();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeEncounterBuilder] Act 1 Production 02 root was not found.");
                return;
            }

            root.name = EncounterRootName;
            Transform existingAnchors = root.Find(AnchorRootName);
            if (existingAnchors != null)
                Object.DestroyImmediate(existingAnchors.gameObject);

            GameObject anchors = new GameObject(AnchorRootName);
            anchors.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            anchors.transform.SetParent(root, false);

            CreateAnchor(anchors.transform, "IntakeSecurity_Anchor", new Vector3(0f, 0f, -2.2f), "Act1_IntakeSecurity");
            CreateAnchor(anchors.transform, "ContainmentCalibration_Anchor", new Vector3(0f, 0f, 6.8f), "Act1_ContainmentCalibration");
            CreateAnchor(anchors.transform, "ExtractionGuard_Anchor", new Vector3(4.2f, 0f, 2.9f), "Act1_ExtractionGuard");

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateEncounterDefinitions()
        {
            CreateDefinition(
                "Act1_IntakeSecurity",
                "act1_intake_security",
                "Intake Security",
                "Clear the first Grey blocking the bunker intake approach.",
                "The intake Grey is defeated.",
                1,
                false,
                false);

            CreateDefinition(
                "Act1_ContainmentCalibration",
                "act1_containment_calibration",
                "Containment Calibration",
                "Reach the containment annex, secure the scientist, and recover the Aether sample.",
                "The scientist calibrates the weapon after the Aether sample is collected.",
                0,
                true,
                true);

            CreateDefinition(
                "Act1_ExtractionGuard",
                "act1_extraction_guard",
                "Extraction Guard",
                "Use the calibrated route to clear the Grey guarding extraction.",
                "The extraction Grey is defeated and the pad is available.",
                1,
                true,
                true);
        }

        private static void CreateDefinition(
            string assetName,
            string encounterId,
            string displayName,
            string objective,
            string completionCondition,
            int expectedGreyCount,
            bool requiresScientistCalibration,
            bool requiresAether)
        {
            string path = EncounterDirectory + "/" + assetName + ".asset";
            EncounterDefinition definition = AssetDatabase.LoadAssetAtPath<EncounterDefinition>(path);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<EncounterDefinition>();
                definition.name = assetName;
                AssetDatabase.CreateAsset(definition, path);
            }

            SerializedObject definitionSo = new SerializedObject(definition);
            definitionSo.FindProperty("_encounterId").stringValue = encounterId;
            definitionSo.FindProperty("_displayName").stringValue = displayName;
            definitionSo.FindProperty("_objective").stringValue = objective;
            definitionSo.FindProperty("_completionCondition").stringValue = completionCondition;
            definitionSo.FindProperty("_expectedGreyCount").intValue = expectedGreyCount;
            definitionSo.FindProperty("_requiresScientistCalibration").boolValue = requiresScientistCalibration;
            definitionSo.FindProperty("_requiresAether").boolValue = requiresAether;
            definitionSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateAnchor(Transform parent, string name, Vector3 position, string definitionName)
        {
            EncounterDefinition definition = AssetDatabase.LoadAssetAtPath<EncounterDefinition>(
                EncounterDirectory + "/" + definitionName + ".asset");
            if (definition == null)
                throw new System.InvalidOperationException("Missing required Act 1 encounter definition: " + definitionName);

            GameObject anchor = new GameObject(name);
            anchor.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            anchor.transform.SetParent(parent, false);
            anchor.transform.position = position;

            Act1EncounterAnchor encounterAnchor = anchor.AddComponent<Act1EncounterAnchor>();
            SerializedObject anchorSo = new SerializedObject(encounterAnchor);
            anchorSo.FindProperty("_definition").objectReferenceValue = definition;
            anchorSo.ApplyModifiedPropertiesWithoutUndo();
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
