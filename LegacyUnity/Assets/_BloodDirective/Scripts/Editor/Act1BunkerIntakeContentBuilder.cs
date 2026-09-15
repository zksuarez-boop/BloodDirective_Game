using BloodDirective.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Adds editable room-content records to the passed Bunker Intake production route.
    /// </summary>
    public static class Act1BunkerIntakeContentBuilder
    {
        private const string SourceScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeRoute01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeContent01.unity";
        private const string SourceRootName = "Act1_BunkerIntakeRoute01_Root";
        private const string ProductionRootName = "Act1_BunkerIntakeContent01_Root";
        private const string ProductionMarkerName = "Act1BunkerIntakeContent_ProductionMarker";
        private const string LevelPath = "Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Act1_BunkerIntakeRoute.asset";
        private const string RoomDirectory = "Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Rooms";
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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 07 - Bunker Intake Content Data")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SourceScenePath) == null)
                Act1BunkerIntakeRouteBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SourceScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeContentBuilder] Act 1 Production 06 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(SourceScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeContentBuilder] Bunker Intake Route root was not found.");
                return;
            }

            root.name = ProductionRootName;
            Transform existingMarker = root.Find(ProductionMarkerName);
            if (existingMarker != null)
                Object.DestroyImmediate(existingMarker.gameObject);

            CreateProductionMarker(root);
            CreateOrUpdateRoomContent();

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

        private static void CreateOrUpdateRoomContent()
        {
            EnsureFolderExists("Assets/_BloodDirective/ScriptableObjects");
            EnsureFolderExists("Assets/_BloodDirective/ScriptableObjects/Levels");
            EnsureFolderExists("Assets/_BloodDirective/ScriptableObjects/Levels/Act1");
            EnsureFolderExists(RoomDirectory);

            RoomDefinition[] rooms =
            {
                CreateOrUpdateRoom("Act1_BunkerIntake", "act1_bunker_intake", "Bunker Intake", 1,
                    "Establish the breach and enter the intake route.", null, false, false, false),
                CreateOrUpdateRoom("Act1_ServiceCorridor", "act1_service_corridor", "Service Corridor", 2,
                    "Neutralize the first Grey to secure access to Logistics Bay.", LoadEncounter("Act1_IntakeSecurity"), false, false, false),
                CreateOrUpdateRoom("Act1_LogisticsBay", "act1_logistics_bay", "Logistics Bay", 3,
                    "Rescue the scientist and return here after collecting the Aether sample.", LoadEncounter("Act1_ContainmentCalibration"), true, false, false),
                CreateOrUpdateRoom("Act1_RelayChamber", "act1_relay_chamber", "Relay Chamber", 4,
                    "Recover the Aether sample after the service corridor is secured.", null, false, true, false),
                CreateOrUpdateRoom("Act1_ExtractionPassage", "act1_extraction_passage", "Extraction Passage", 5,
                    "Neutralize the extraction guard and reach the extraction pad.", LoadEncounter("Act1_ExtractionGuard"), false, false, true)
            };

            LevelDefinition level = AssetDatabase.LoadAssetAtPath<LevelDefinition>(LevelPath);
            if (level == null)
            {
                Debug.LogError("[Act1BunkerIntakeContentBuilder] Bunker Intake Route level record was not found.");
                return;
            }

            SerializedObject serializedLevel = new SerializedObject(level);
            SerializedProperty serializedRooms = serializedLevel.FindProperty("_rooms");
            serializedRooms.arraySize = rooms.Length;
            for (int index = 0; index < rooms.Length; index++)
                serializedRooms.GetArrayElementAtIndex(index).objectReferenceValue = rooms[index];

            serializedLevel.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(level);
        }

        private static RoomDefinition CreateOrUpdateRoom(
            string assetName,
            string roomId,
            string displayName,
            int sequenceIndex,
            string purpose,
            EncounterDefinition encounter,
            bool containsScientist,
            bool containsAether,
            bool isExtraction)
        {
            string roomPath = RoomDirectory + "/" + assetName + ".asset";
            RoomDefinition room = AssetDatabase.LoadAssetAtPath<RoomDefinition>(roomPath);
            if (room == null)
            {
                room = ScriptableObject.CreateInstance<RoomDefinition>();
                AssetDatabase.CreateAsset(room, roomPath);
            }

            SerializedObject serializedRoom = new SerializedObject(room);
            serializedRoom.FindProperty("_roomId").stringValue = roomId;
            serializedRoom.FindProperty("_displayName").stringValue = displayName;
            serializedRoom.FindProperty("_sequenceIndex").intValue = sequenceIndex;
            serializedRoom.FindProperty("_purpose").stringValue = purpose;
            serializedRoom.FindProperty("_encounter").objectReferenceValue = encounter;
            serializedRoom.FindProperty("_containsScientist").boolValue = containsScientist;
            serializedRoom.FindProperty("_containsAether").boolValue = containsAether;
            serializedRoom.FindProperty("_isExtraction").boolValue = isExtraction;
            serializedRoom.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(room);
            return room;
        }

        private static EncounterDefinition LoadEncounter(string encounterName)
        {
            return AssetDatabase.LoadAssetAtPath<EncounterDefinition>(EncounterDirectory + "/" + encounterName + ".asset");
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
