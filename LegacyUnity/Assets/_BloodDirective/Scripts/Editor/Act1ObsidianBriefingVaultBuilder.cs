using BloodDirective.Data;
using BloodDirective.Act1;
using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Creates the first approved blockout for Act 1's opening level.
    /// This scene is intentionally data and layout only; the validated Bunker Intake route remains unchanged.
    /// </summary>
    public static class Act1ObsidianBriefingVaultBuilder
    {
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_ObsidianBriefingVault01.unity";
        private const string RootName = "Act1_ObsidianBriefingVault01_Root";
        private const string MaterialDirectory = "Assets/_BloodDirective/Art/Materials/Act1Bunker";
        private const string LevelDirectory = "Assets/_BloodDirective/ScriptableObjects/Levels/Act1";
        private const string RoomDirectory = LevelDirectory + "/Rooms";
        private const string MissionDirectory = "Assets/_BloodDirective/ScriptableObjects/Missions";
        private const string LevelPath = LevelDirectory + "/Act1_ObsidianBriefingVault.asset";
        private const string MissionPath = MissionDirectory + "/Act1_ObsidianBriefingVault.asset";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string PlayerLayer = "Player";
        private const string VisualOnlyLayer = "VisualOnly";

        private static readonly VaultRoom[] Rooms =
        {
            new VaultRoom("Act1_ObsidianCommandChamber", "act1_obsidian_command_chamber", "Obsidian Command Chamber", "Receive the interrupted black-operation briefing.", false),
            new VaultRoom("Act1_ObservationGallery", "act1_observation_gallery", "Observation Gallery", "Witness the breach cascade through the containment monitors.", false),
            new VaultRoom("Act1_EmergencyDescentShaft", "act1_emergency_descent_shaft", "Emergency Descent Shaft", "Reach the forced descent into Containment Hive.", false),
            new VaultRoom("Act1_CollapsedEscapeRoute", "act1_collapsed_escape_route", "Collapsed Escape Route", "Inspect the destroyed planned infiltration route.", false)
        };

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 08 - Obsidian Briefing Vault Blockout")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            EnsureFolders();
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Act1_ObsidianBriefingVault01";

            Material floor = CreateOrLoadMaterial("Act1Bunker_FloorPlate", new Color(0.045f, 0.07f, 0.085f), 0.72f, 0.25f);
            Material inset = CreateOrLoadMaterial("Act1Bunker_FloorInset", new Color(0.015f, 0.026f, 0.035f), 0.48f, 0.18f);
            Material concrete = CreateOrLoadMaterial("Act1Bunker_Concrete", new Color(0.17f, 0.19f, 0.19f), 0.12f, 0.15f);
            Material trim = CreateOrLoadMaterial("Act1Bunker_Trim", new Color(0.055f, 0.08f, 0.09f), 0.82f, 0.3f);
            Material cyan = CreateOrLoadEmissiveMaterial("Act1Bunker_Cyan", new Color(0.13f, 0.82f, 1f), 2.1f);
            Material warning = CreateOrLoadEmissiveMaterial("Act1Bunker_Warning", new Color(1f, 0.2f, 0.05f), 1.8f);
            Material playerMaterial = CreateOrLoadMaterial("Act1Bunker_PlayerOlive", new Color(0.24f, 0.34f, 0.22f), 0.2f, 0.35f);

            GameObject root = new GameObject(RootName);
            root.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            GameObject player = CreatePlayer(root.transform, playerMaterial);
            ConfigureProceduralLayout(root, floor, inset, concrete, trim, cyan, warning, player.transform);
            Camera camera = CreateCamera(player.transform);
            ConfigureMover(player, camera);
            CreateSceneLighting();
            CreateOrUpdateContentAssets();

            Selection.activeGameObject = root;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildLayout(Transform root, Material floor, Material inset, Material concrete, Material trim, Material cyan, Material warning)
        {
            Transform command = CreateSpace(root, "01_CommandChamber");
            CreateRoomFloor(command, new Vector3(0f, 0f, 0f), new Vector3(28f, 0.2f, 22f), floor, inset);
            CreateBlockerWithVisual(command, "CommandSouthWall", new Vector3(0f, 1.4f, -11.3f), new Vector3(28.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(command, "CommandEastWall", new Vector3(14.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 22.6f), concrete);
            CreateBlockerWithVisual(command, "CommandNorthWallWest", new Vector3(-10f, 1.4f, 11.3f), new Vector3(8.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(command, "CommandNorthWallEast", new Vector3(10f, 1.4f, 11.3f), new Vector3(8.6f, 2.8f, 0.6f), concrete);
            CreateVisualCube(command, "CommandNorthExitHeader_VisualOnly", new Vector3(0f, 2.72f, 11.3f), new Vector3(11.8f, 0.14f, 0.72f), trim);
            CreateBlockerWithVisual(command, "BriefingTable", new Vector3(0f, 0.8f, -2f), new Vector3(6.4f, 1.6f, 3f), trim);
            CreateBlockerWithVisual(command, "CommandConsoleLeft", new Vector3(-8.5f, 0.7f, 4.5f), new Vector3(2f, 1.4f, 1.8f), trim);
            CreateBlockerWithVisual(command, "CommandConsoleRight", new Vector3(8.5f, 0.7f, 4.5f), new Vector3(2f, 1.4f, 1.8f), trim);
            CreateLight(command, "CommandLight", new Vector3(0f, 2.72f, -2f), cyan, new Color(0.2f, 0.82f, 1f), 1.65f, 12f);

            Transform secureCorridor = CreateSpace(root, "02_SecureBriefingCorridor");
            CreateRoomFloor(secureCorridor, new Vector3(0f, 0f, 102f), new Vector3(12f, 0.2f, 182f), floor, inset);
            CreateBlockerWithVisual(secureCorridor, "SecureCorridorWestWall", new Vector3(-6.3f, 1.4f, 102f), new Vector3(0.6f, 2.8f, 182f), concrete);
            CreateBlockerWithVisual(secureCorridor, "SecureCorridorEastWall", new Vector3(6.3f, 1.4f, 102f), new Vector3(0.6f, 2.8f, 182f), concrete);
            CreateVisualCube(secureCorridor, "SecureCorridorConduit_VisualOnly", new Vector3(-2.1f, 2.72f, 102f), new Vector3(0.18f, 0.16f, 176f), trim);
            CreateLight(secureCorridor, "SecureCorridorLight01", new Vector3(0f, 2.68f, 45f), cyan, new Color(0.18f, 0.72f, 1f), 1.2f, 7f);
            CreateLight(secureCorridor, "SecureCorridorLight02", new Vector3(0f, 2.68f, 100f), cyan, new Color(0.18f, 0.72f, 1f), 1.2f, 7f);
            CreateLight(secureCorridor, "SecureCorridorLight03", new Vector3(0f, 2.68f, 155f), cyan, new Color(0.18f, 0.72f, 1f), 1.2f, 7f);

            Transform gallery = CreateSpace(root, "03_ObservationGallery");
            CreateRoomFloor(gallery, new Vector3(30f, 0f, 205f), new Vector3(60f, 0.2f, 28f), floor, inset);
            CreateBlockerWithVisual(gallery, "GalleryEastWall", new Vector3(60.3f, 1.4f, 205f), new Vector3(0.6f, 2.8f, 28.6f), concrete);
            CreateBlockerWithVisual(gallery, "GalleryNorthWall", new Vector3(20f, 1.4f, 219.3f), new Vector3(40.6f, 2.8f, 0.6f), concrete);
            CreateVisualCube(gallery, "ObservationWindow_VisualOnly", new Vector3(30f, 2.05f, 218.95f), new Vector3(32f, 1.25f, 0.06f), cyan);
            CreateBlockerWithVisual(gallery, "ObservationConsoleBank", new Vector3(28f, 0.8f, 200f), new Vector3(12f, 1.6f, 2.4f), trim);
            CreateLight(gallery, "GalleryLight", new Vector3(30f, 2.72f, 205f), cyan, new Color(0.18f, 0.72f, 1f), 1.8f, 16f);

            Transform emergencyConcourse = CreateSpace(root, "04_EmergencyConcourse");
            CreateRoomFloor(emergencyConcourse, new Vector3(58f, 0f, 265f), new Vector3(16f, 0.2f, 100f), floor, inset);
            CreateBlockerWithVisual(emergencyConcourse, "ConcourseWestWall", new Vector3(49.7f, 1.4f, 265f), new Vector3(0.6f, 2.8f, 100f), concrete);
            CreateBlockerWithVisual(emergencyConcourse, "ConcourseEastWall", new Vector3(66.3f, 1.4f, 265f), new Vector3(0.6f, 2.8f, 100f), concrete);
            CreateVisualCube(emergencyConcourse, "EmergencyConcourseConduit_VisualOnly", new Vector3(62f, 2.72f, 265f), new Vector3(0.18f, 0.16f, 94f), trim);
            CreateLight(emergencyConcourse, "ConcourseAlarm01", new Vector3(58f, 2.68f, 240f), warning, new Color(1f, 0.18f, 0.05f), 1.3f, 8f);
            CreateLight(emergencyConcourse, "ConcourseAlarm02", new Vector3(58f, 2.68f, 290f), warning, new Color(1f, 0.18f, 0.05f), 1.3f, 8f);

            Transform shaft = CreateSpace(root, "05_EmergencyDescentShaft");
            CreateRoomFloor(shaft, new Vector3(82f, 0f, 330f), new Vector3(40f, 0.2f, 36f), floor, inset);
            CreateBlockerWithVisual(shaft, "ShaftNorthWall", new Vector3(82f, 1.4f, 348.3f), new Vector3(40.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(shaft, "ShaftEastWall", new Vector3(102.3f, 1.4f, 330f), new Vector3(0.6f, 2.8f, 36.6f), concrete);
            CreateBlockerWithVisual(shaft, "ShaftWestWallNorth", new Vector3(61.7f, 1.4f, 335f), new Vector3(0.6f, 2.8f, 26f), concrete);
            CreateBlockerWithVisual(shaft, "ShaftCore", new Vector3(82f, 0.85f, 330f), new Vector3(11f, 1.7f, 11f), trim);
            CreateVisualCube(shaft, "ShaftHazardRing_VisualOnly", new Vector3(82f, 0.15f, 330f), new Vector3(22f, 0.02f, 0.24f), warning);
            CreateLight(shaft, "ShaftAlarm", new Vector3(82f, 2.6f, 330f), warning, new Color(1f, 0.18f, 0.05f), 2f, 18f);

            Transform collapsed = CreateSpace(root, "06_CollapsedEscapeRoute");
            CreateRoomFloor(collapsed, new Vector3(-48f, 0f, 0f), new Vector3(68f, 0.2f, 14f), floor, inset);
            CreateBlockerWithVisual(collapsed, "CollapsedSouthWall", new Vector3(-48f, 1.4f, -7.3f), new Vector3(68.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(collapsed, "CollapsedNorthWall", new Vector3(-48f, 1.4f, 7.3f), new Vector3(68.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(collapsed, "CollapsedWestWall", new Vector3(-82.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 14.6f), concrete);
            CreateBlockerWithVisual(collapsed, "CollapsedRouteGate", new Vector3(-72f, 1.2f, 0f), new Vector3(3.5f, 2.4f, 8f), concrete);
            CreateVisualCube(collapsed, "CollapsedRouteWarning_VisualOnly", new Vector3(-75f, 0.15f, 0f), new Vector3(0.24f, 0.02f, 10f), warning);
            CreateLight(collapsed, "CollapsedRouteAlarm", new Vector3(-70f, 2.55f, 0f), warning, new Color(1f, 0.16f, 0.04f), 1.4f, 8f);
        }

        private static void ConfigureProceduralLayout(
            GameObject root,
            Material floor,
            Material inset,
            Material concrete,
            Material trim,
            Material cyan,
            Material warning,
            Transform player)
        {
            ProceduralVaultLayoutGenerator generator = root.AddComponent<ProceduralVaultLayoutGenerator>();
            SerializedObject serializedGenerator = new SerializedObject(generator);
            serializedGenerator.FindProperty("_floorMaterial").objectReferenceValue = floor;
            serializedGenerator.FindProperty("_insetMaterial").objectReferenceValue = inset;
            serializedGenerator.FindProperty("_wallMaterial").objectReferenceValue = concrete;
            serializedGenerator.FindProperty("_trimMaterial").objectReferenceValue = trim;
            serializedGenerator.FindProperty("_cyanMaterial").objectReferenceValue = cyan;
            serializedGenerator.FindProperty("_warningMaterial").objectReferenceValue = warning;
            serializedGenerator.FindProperty("_player").objectReferenceValue = player;
            serializedGenerator.FindProperty("_gridWidth").intValue = 32;
            serializedGenerator.FindProperty("_gridHeight").intValue = 46;
            serializedGenerator.FindProperty("_tileSize").vector2Value = new Vector2(8f, 6f);
            serializedGenerator.FindProperty("_seed").intValue = 0;
            serializedGenerator.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Transform CreateSpace(Transform parent, string name)
        {
            GameObject space = new GameObject(name);
            space.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            space.transform.SetParent(parent, false);
            return space.transform;
        }

        private static void CreateRoomFloor(Transform parent, Vector3 position, Vector3 size, Material floor, Material inset)
        {
            GameObject walkable = new GameObject("Walkable");
            walkable.layer = LayerMask.NameToLayer(WalkableLayer);
            walkable.transform.SetParent(parent, false);
            walkable.transform.position = position;
            walkable.AddComponent<BoxCollider>().size = size;
            CreateVisualCube(parent, "FloorPlate_VisualOnly", new Vector3(position.x, 0.115f, position.z), new Vector3(size.x, 0.025f, size.z), floor);
            CreateVisualCube(parent, "FloorInset_VisualOnly", new Vector3(position.x, 0.13f, position.z), new Vector3(size.x - 0.38f, 0.018f, size.z - 0.38f), inset);
        }

        private static void CreateDoorwayWall(Transform parent, string name, Vector3 position, bool horizontal, Material concrete, Material trim)
        {
            Vector3 firstPosition = horizontal ? position + new Vector3(-3.9f, 1.4f, 0f) : position + new Vector3(0f, 1.4f, -3.2f);
            Vector3 secondPosition = horizontal ? position + new Vector3(3.9f, 1.4f, 0f) : position + new Vector3(0f, 1.4f, 3.2f);
            Vector3 wallSize = horizontal ? new Vector3(4.2f, 2.8f, 0.6f) : new Vector3(0.6f, 2.8f, 3f);
            Vector3 headerSize = horizontal ? new Vector3(3.3f, 0.14f, 0.72f) : new Vector3(0.72f, 0.14f, 2.6f);
            CreateBlockerWithVisual(parent, name + "Left", firstPosition, wallSize, concrete);
            CreateBlockerWithVisual(parent, name + "Right", secondPosition, wallSize, concrete);
            CreateVisualCube(parent, name + "Header_VisualOnly", position + new Vector3(0f, 2.72f, 0f), headerSize, trim);
        }

        private static void CreateBlockerWithVisual(Transform parent, string name, Vector3 position, Vector3 size, Material material)
        {
            GameObject blocker = new GameObject(name + "_Blocker");
            blocker.layer = LayerMask.NameToLayer(BlockerLayer);
            blocker.transform.SetParent(parent, false);
            blocker.transform.position = position;
            blocker.AddComponent<BoxCollider>().size = size;
            CreateVisualCube(parent, name + "_VisualOnly", position, size, material);
        }

        private static void CreateVisualCube(Transform parent, string name, Vector3 position, Vector3 size, Material material)
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = name;
            visual.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            visual.transform.SetParent(parent, false);
            visual.transform.position = position;
            visual.transform.localScale = size;
            visual.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(visual.GetComponent<Collider>());
        }

        private static void CreateLight(Transform parent, string name, Vector3 position, Material housingMaterial, Color color, float intensity, float range)
        {
            CreateVisualCube(parent, name + "Housing_VisualOnly", position, new Vector3(1.8f, 0.08f, 0.24f), housingMaterial);
            GameObject lightObject = new GameObject(name + "_VisualOnly");
            lightObject.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.position = position;
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
        }

        private static GameObject CreatePlayer(Transform parent, Material material)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Act1PlayerSpawn";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer(PlayerLayer);
            player.transform.SetParent(parent, false);
            player.transform.position = new Vector3(0f, 1f, -7.5f);
            player.GetComponent<Renderer>().sharedMaterial = material;
            player.AddComponent<PrototypeClickMover>();
            return player;
        }

        private static Camera CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Act1ObsidianIsometricCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.fieldOfView = 45f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 100f;
            cameraObject.tag = "MainCamera";

            PrototypeIsometricCamera followCamera = cameraObject.AddComponent<PrototypeIsometricCamera>();
            followCamera.SetTarget(target);
            return camera;
        }

        private static void ConfigureMover(GameObject player, Camera camera)
        {
            SerializedObject mover = new SerializedObject(player.GetComponent<PrototypeClickMover>());
            mover.FindProperty("_camera").objectReferenceValue = camera;
            mover.FindProperty("_walkableLayer").intValue = 1 << LayerMask.NameToLayer(WalkableLayer);
            mover.FindProperty("_blockerLayer").intValue = 1 << LayerMask.NameToLayer(BlockerLayer);
            mover.FindProperty("_clickBlockerLayer").intValue = 0;
            mover.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateSceneLighting()
        {
            GameObject lightObject = new GameObject("Act1ObsidianDirectionalLight");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.55f;
            light.color = new Color(0.68f, 0.78f, 0.86f);
            lightObject.transform.rotation = Quaternion.Euler(55f, -35f, 0f);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.025f, 0.035f, 0.045f);
        }

        private static void CreateOrUpdateContentAssets()
        {
            MissionDefinition mission = AssetDatabase.LoadAssetAtPath<MissionDefinition>(MissionPath);
            if (mission == null)
            {
                mission = ScriptableObject.CreateInstance<MissionDefinition>();
                AssetDatabase.CreateAsset(mission, MissionPath);
            }

            SerializedObject serializedMission = new SerializedObject(mission);
            serializedMission.FindProperty("_missionId").stringValue = "act1_obsidian_briefing_vault";
            serializedMission.FindProperty("_displayName").stringValue = "Obsidian Briefing Vault";
            serializedMission.FindProperty("_briefing").stringValue = "Receive the black-operation briefing before the containment breach forces an emergency descent.";
            serializedMission.FindProperty("_primaryObjective").stringValue = "Reach the emergency descent shaft.";
            serializedMission.FindProperty("_biomeId").stringValue = "underground_bunker";
            serializedMission.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(mission);

            RoomDefinition[] rooms = new RoomDefinition[Rooms.Length];
            for (int index = 0; index < Rooms.Length; index++)
                rooms[index] = CreateOrUpdateRoom(Rooms[index], index + 1);

            LevelDefinition level = AssetDatabase.LoadAssetAtPath<LevelDefinition>(LevelPath);
            if (level == null)
            {
                level = ScriptableObject.CreateInstance<LevelDefinition>();
                AssetDatabase.CreateAsset(level, LevelPath);
            }

            SerializedObject serializedLevel = new SerializedObject(level);
            serializedLevel.FindProperty("_levelId").stringValue = "act1_obsidian_briefing_vault";
            serializedLevel.FindProperty("_displayName").stringValue = "Obsidian Briefing Vault";
            serializedLevel.FindProperty("_biomeId").stringValue = "underground_bunker";
            serializedLevel.FindProperty("_roomCount").intValue = rooms.Length;
            serializedLevel.FindProperty("_mission").objectReferenceValue = mission;
            SerializedProperty serializedRooms = serializedLevel.FindProperty("_rooms");
            serializedRooms.arraySize = rooms.Length;
            for (int index = 0; index < rooms.Length; index++)
                serializedRooms.GetArrayElementAtIndex(index).objectReferenceValue = rooms[index];
            serializedLevel.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(level);
        }

        private static RoomDefinition CreateOrUpdateRoom(VaultRoom definition, int sequenceIndex)
        {
            string path = RoomDirectory + "/" + definition.AssetName + ".asset";
            RoomDefinition room = AssetDatabase.LoadAssetAtPath<RoomDefinition>(path);
            if (room == null)
            {
                room = ScriptableObject.CreateInstance<RoomDefinition>();
                AssetDatabase.CreateAsset(room, path);
            }

            SerializedObject serializedRoom = new SerializedObject(room);
            serializedRoom.FindProperty("_roomId").stringValue = definition.RoomId;
            serializedRoom.FindProperty("_displayName").stringValue = definition.DisplayName;
            serializedRoom.FindProperty("_sequenceIndex").intValue = sequenceIndex;
            serializedRoom.FindProperty("_purpose").stringValue = definition.Purpose;
            serializedRoom.FindProperty("_encounter").objectReferenceValue = null;
            serializedRoom.FindProperty("_containsScientist").boolValue = false;
            serializedRoom.FindProperty("_containsAether").boolValue = false;
            serializedRoom.FindProperty("_isExtraction").boolValue = definition.IsExtraction;
            serializedRoom.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(room);
            return room;
        }

        private static Material CreateOrLoadMaterial(string name, Color color, float metallic, float smoothness)
        {
            string path = MaterialDirectory + "/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
                return material;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            material = new Material(shader) { name = name, color = color };
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Smoothness", smoothness);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static Material CreateOrLoadEmissiveMaterial(string name, Color color, float intensity)
        {
            Material material = CreateOrLoadMaterial(name, color, 0.15f, 0.45f);
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * intensity);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/_BloodDirective/Scenes", "Act1");
            EnsureFolder("Assets/_BloodDirective/Art/Materials", "Act1Bunker");
            EnsureFolder("Assets/_BloodDirective/ScriptableObjects", "Missions");
            EnsureFolder("Assets/_BloodDirective/ScriptableObjects", "Levels");
            EnsureFolder("Assets/_BloodDirective/ScriptableObjects/Levels", "Act1");
            EnsureFolder(LevelDirectory, "Rooms");
        }

        private static void EnsureFolder(string parentPath, string folderName)
        {
            string fullPath = parentPath + "/" + folderName;
            if (!AssetDatabase.IsValidFolder(fullPath))
                AssetDatabase.CreateFolder(parentPath, folderName);
        }

        private readonly struct VaultRoom
        {
            public VaultRoom(string assetName, string roomId, string displayName, string purpose, bool isExtraction)
            {
                AssetName = assetName;
                RoomId = roomId;
                DisplayName = displayName;
                Purpose = purpose;
                IsExtraction = isExtraction;
            }

            public string AssetName { get; }
            public string RoomId { get; }
            public string DisplayName { get; }
            public string Purpose { get; }
            public bool IsExtraction { get; }
        }
    }
}
