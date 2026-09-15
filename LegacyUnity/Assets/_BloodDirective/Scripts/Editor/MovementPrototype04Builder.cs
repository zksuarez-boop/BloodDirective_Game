using BloodDirective.Prototypes.Interaction;
using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype04Builder
    {
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype04.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string PlayerLayer = "Player";
        private const string VisualOnlyLayer = "VisualOnly";
        private const string InteractableLayer = "Interactable";

        [InitializeOnLoadMethod]
        private static void BuildSceneAfterCompile()
        {
            EditorApplication.delayCall += () =>
            {
                if (Application.isPlaying)
                    return;
                if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                    return;
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
                    return;

                BuildScene();
            };
        }

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 04")]
        public static void BuildScene()
        {
            EnsureProjectFolders();
            EnsureLayer(8, WalkableLayer);
            EnsureLayer(9, BlockerLayer);
            EnsureLayer(10, PlayerLayer);
            EnsureLayer(11, VisualOnlyLayer);
            EnsureLayer(12, InteractableLayer);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MovementPrototype04";

            Material floorMaterial = CreateMaterial("Prototype04_DarkConcreteFloor", new Color(0.04f, 0.06f, 0.07f));
            Material wallMaterial = CreateMaterial("Prototype04_BunkerConcrete", new Color(0.24f, 0.26f, 0.23f));
            Material darkMetalMaterial = CreateMaterial("Prototype04_BlastSteel", new Color(0.055f, 0.06f, 0.065f));
            Material openDoorMaterial = CreateMaterial("Prototype04_OpenDoorGreen", new Color(0.12f, 0.42f, 0.25f));
            Material playerMaterial = CreateMaterial("Prototype_PlayerOlive", new Color(0.24f, 0.34f, 0.22f));
            Material lightMaterial = CreateMaterial("Prototype04_ColdTubeLight", new Color(0.86f, 0.95f, 1f));
            Material terminalMaterial = CreateMaterial("Prototype04_TerminalIdle", new Color(0.06f, 0.32f, 0.28f));
            Material activeMaterial = CreateMaterial("Prototype04_TerminalActive", new Color(0.1f, 0.75f, 0.42f));
            Material exitMaterial = CreateMaterial("Prototype04_ExitIdle", new Color(0.25f, 0.35f, 0.7f));
            Material exitReachedMaterial = CreateMaterial("Prototype04_ExitReached", new Color(0.75f, 0.85f, 0.32f));

            GameObject root = new GameObject("MovementPrototype04_Root");
            BuildWalkableLayout(root.transform, floorMaterial, darkMetalMaterial);
            BuildBlockers(root.transform, wallMaterial, darkMetalMaterial, openDoorMaterial, terminalMaterial, activeMaterial, exitMaterial, exitReachedMaterial);
            BuildVisualOnlyDetails(root.transform, darkMetalMaterial, lightMaterial);

            GameObject player = CreatePlayer(root.transform, playerMaterial);
            Camera camera = CreateCamera(player.transform);
            ConfigureMover(player, camera);
            ConfigureInteraction(camera);
            CreateLighting();

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.033f, 0.04f, 0.043f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildWalkableLayout(Transform root, Material floorMaterial, Material guideMaterial)
        {
            CreateCube("ReadyRoom_Walkable", new Vector3(0f, 0f, 0f), new Vector3(10f, 0.2f, 8f), floorMaterial, WalkableLayer, root);
            CreateCube("SecurityCorridor_Walkable", new Vector3(0f, 0f, 10.5f), new Vector3(4f, 0.2f, 13f), floorMaterial, WalkableLayer, root);
            CreateCube("ControlRoom_Walkable", new Vector3(7f, 0f, 12f), new Vector3(10f, 0.2f, 8f), floorMaterial, WalkableLayer, root);
            CreateCube("ExitVestibule_Walkable", new Vector3(7f, 0f, 20f), new Vector3(4f, 0.2f, 5f), floorMaterial, WalkableLayer, root);

            for (int i = 0; i < 6; i++)
                CreateCube($"ReadyRoom_FloorJoint_VisualOnly_{i:00}", new Vector3(0f, 0.13f, -2.8f + i * 1.35f), new Vector3(9.2f, 0.035f, 0.035f), guideMaterial, VisualOnlyLayer, root, false);

            for (int i = 0; i < 7; i++)
                CreateCube($"Corridor_FloorJoint_VisualOnly_{i:00}", new Vector3(0f, 0.13f, 5f + i * 1.7f), new Vector3(3.5f, 0.035f, 0.035f), guideMaterial, VisualOnlyLayer, root, false);

            for (int i = 0; i < 7; i++)
                CreateCube($"ControlExit_FloorJoint_VisualOnly_{i:00}", new Vector3(4.4f + i * 0.85f, 0.13f, 12f), new Vector3(0.035f, 0.035f, 7.3f), guideMaterial, VisualOnlyLayer, root, false);
        }

        private static void BuildBlockers(Transform root, Material wallMaterial, Material darkMetalMaterial, Material openDoorMaterial, Material terminalMaterial, Material activeMaterial, Material exitMaterial, Material exitReachedMaterial)
        {
            CreateCube("ReadyRoom_WestWall_Blocker", new Vector3(-5.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 8.7f), wallMaterial, BlockerLayer, root);
            CreateCube("ReadyRoom_EastWall_Blocker", new Vector3(5.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 8.7f), wallMaterial, BlockerLayer, root);
            CreateCube("ReadyRoom_SouthWall_Blocker", new Vector3(0f, 1.4f, -4.3f), new Vector3(10.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ReadyRoom_NorthWallLeft_Blocker", new Vector3(-3.6f, 1.4f, 4.3f), new Vector3(2.8f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ReadyRoom_NorthWallRight_Blocker", new Vector3(3.6f, 1.4f, 4.3f), new Vector3(2.8f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            CreateCube("Corridor_WestWall_Blocker", new Vector3(-2.3f, 1.4f, 10.5f), new Vector3(0.6f, 2.8f, 13f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_EastWallLower_Blocker", new Vector3(2.3f, 1.4f, 7.15f), new Vector3(0.6f, 2.8f, 5.9f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_EastWallUpper_Blocker", new Vector3(2.3f, 1.4f, 16.25f), new Vector3(0.6f, 2.8f, 2.5f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_NorthCap_Blocker", new Vector3(0f, 1.4f, 17.3f), new Vector3(4.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            CreateCube("ControlRoom_SouthWall_Blocker", new Vector3(7f, 1.4f, 7.7f), new Vector3(10.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ControlRoom_EastWall_Blocker", new Vector3(12.3f, 1.4f, 12f), new Vector3(0.6f, 2.8f, 8.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ControlRoom_WestWallUpper_Blocker", new Vector3(2.7f, 1.4f, 14.8f), new Vector3(0.6f, 2.8f, 3f), wallMaterial, BlockerLayer, root);
            CreateCube("ControlRoom_WestWallLower_Blocker", new Vector3(2.7f, 1.4f, 8.9f), new Vector3(0.6f, 2.8f, 1.8f), wallMaterial, BlockerLayer, root);
            CreateCube("ControlRoom_NorthWallLeft_Blocker", new Vector3(4.5f, 1.4f, 16.3f), new Vector3(3.7f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ControlRoom_NorthWallRight_Blocker", new Vector3(10.4f, 1.4f, 16.3f), new Vector3(3.8f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            CreateCube("ExitVestibule_WestWall_Blocker", new Vector3(4.7f, 1.4f, 20f), new Vector3(0.6f, 2.8f, 5f), wallMaterial, BlockerLayer, root);
            CreateCube("ExitVestibule_EastWall_Blocker", new Vector3(9.3f, 1.4f, 20f), new Vector3(0.6f, 2.8f, 5f), wallMaterial, BlockerLayer, root);
            CreateCube("ExitVestibule_NorthWall_Blocker", new Vector3(7f, 1.4f, 22.8f), new Vector3(4.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            GameObject door = CreateCube("LockedBlastDoor_OpensOnTerminal", new Vector3(7f, 1.4f, 16.35f), new Vector3(3.6f, 2.8f, 0.45f), darkMetalMaterial, BlockerLayer, root);
            var lockedDoor = door.AddComponent<PrototypeLockedDoor>();
            SerializedObject doorSo = new SerializedObject(lockedDoor);
            doorSo.FindProperty("_blockingCollider").objectReferenceValue = door.GetComponent<Collider>();
            doorSo.FindProperty("_doorRenderer").objectReferenceValue = door.GetComponent<Renderer>();
            doorSo.FindProperty("_openedMaterial").objectReferenceValue = openDoorMaterial;
            doorSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject terminal = CreateCube("DoorTerminal_Interactable", new Vector3(4.55f, 1f, 14.55f), new Vector3(0.35f, 1.2f, 0.85f), terminalMaterial, InteractableLayer, root);
            AddDoorTerminal(terminal, "Door Terminal", activeMaterial, lockedDoor);

            GameObject exit = CreateCube("ExitZone_Trigger", new Vector3(7f, 0.15f, 21f), new Vector3(2.2f, 0.16f, 1.2f), exitMaterial, VisualOnlyLayer, root);
            BoxCollider exitCollider = exit.GetComponent<BoxCollider>();
            exitCollider.isTrigger = true;
            var exitZone = exit.AddComponent<PrototypeExitZone>();
            SerializedObject exitSo = new SerializedObject(exitZone);
            exitSo.FindProperty("_renderer").objectReferenceValue = exit.GetComponent<Renderer>();
            exitSo.FindProperty("_reachedMaterial").objectReferenceValue = exitReachedMaterial;
            exitSo.ApplyModifiedPropertiesWithoutUndo();

            CreateCube("ControlRoom_ServerRack_Blocker", new Vector3(9f, 0.9f, 11.4f), new Vector3(1.5f, 1.8f, 1f), darkMetalMaterial, BlockerLayer, root);
            CreateCube("ReadyRoom_Pillar_Blocker", new Vector3(-3.2f, 0.95f, -1.8f), new Vector3(1.1f, 1.9f, 1.1f), darkMetalMaterial, BlockerLayer, root);
        }

        private static void BuildVisualOnlyDetails(Transform root, Material darkMetalMaterial, Material lightMaterial)
        {
            CreateCube("DoorHeader_StartToCorridor_VisualOnly", new Vector3(0f, 3.05f, 4.3f), new Vector3(4.4f, 0.32f, 0.7f), darkMetalMaterial, VisualOnlyLayer, root, false);
            CreateCube("DoorHeader_CorridorToControl_VisualOnly", new Vector3(2.35f, 3.05f, 12f), new Vector3(0.7f, 0.32f, 4.4f), darkMetalMaterial, VisualOnlyLayer, root, false);
            CreateCube("DoorHeader_LockedDoor_VisualOnly", new Vector3(7f, 3.05f, 16.35f), new Vector3(4.5f, 0.32f, 0.7f), darkMetalMaterial, VisualOnlyLayer, root, false);
            CreateCube("OverheadPipe_Corridor_VisualOnly", new Vector3(-1.15f, 3.25f, 10.5f), new Vector3(0.16f, 0.16f, 12f), darkMetalMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_ReadyRoom_VisualOnly", new Vector3(0f, 3.28f, -1.4f), new Vector3(2.4f, 0.08f, 0.35f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_ControlRoom_VisualOnly", new Vector3(7.4f, 3.28f, 14.2f), new Vector3(2.4f, 0.08f, 0.35f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_ExitVestibule_VisualOnly", new Vector3(7f, 3.28f, 20.6f), new Vector3(1.8f, 0.08f, 0.3f), lightMaterial, VisualOnlyLayer, root, false);
        }

        private static void AddDoorTerminal(GameObject target, string displayName, Material activatedMaterial, PrototypeLockedDoor lockedDoor)
        {
            var interactable = target.AddComponent<PrototypeInteractable>();
            SerializedObject serialized = new SerializedObject(interactable);
            serialized.FindProperty("_displayName").stringValue = displayName;
            serialized.FindProperty("_activatedMaterial").objectReferenceValue = activatedMaterial;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            UnityEventTools.AddPersistentListener(interactable.Activated, lockedDoor.Open);
        }

        private static GameObject CreatePlayer(Transform root, Material playerMaterial)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "PrototypePlayer";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer(PlayerLayer);
            player.transform.SetParent(root);
            player.transform.position = new Vector3(0f, 1f, -2.2f);
            player.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            player.AddComponent<PrototypeClickMover>();

            Rigidbody rigidbody = player.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return player;
        }

        private static Camera CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("PrototypeIsometricCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.fieldOfView = 45f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 250f;
            cameraObject.tag = "MainCamera";

            var cameraFollow = cameraObject.AddComponent<PrototypeIsometricCamera>();
            cameraFollow.SetTarget(target);
            return camera;
        }

        private static void ConfigureMover(GameObject player, Camera camera)
        {
            var mover = player.GetComponent<PrototypeClickMover>();
            SerializedObject moverSo = new SerializedObject(mover);
            moverSo.FindProperty("_camera").objectReferenceValue = camera;
            moverSo.FindProperty("_walkableLayer").intValue = 1 << LayerMask.NameToLayer(WalkableLayer);
            moverSo.FindProperty("_blockerLayer").intValue = 1 << LayerMask.NameToLayer(BlockerLayer);
            moverSo.FindProperty("_clickBlockerLayer").intValue = 1 << LayerMask.NameToLayer(InteractableLayer);
            moverSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureInteraction(Camera camera)
        {
            GameObject clickerObject = new GameObject("PrototypeInteractionClicker");
            var clicker = clickerObject.AddComponent<PrototypeInteractionClicker>();
            SerializedObject clickerSo = new SerializedObject(clicker);
            clickerSo.FindProperty("_camera").objectReferenceValue = camera;
            clickerSo.FindProperty("_interactableLayer").intValue = 1 << LayerMask.NameToLayer(InteractableLayer);
            clickerSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateLighting()
        {
            GameObject lightObject = new GameObject("PrototypeDirectionalLight");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.82f;
            light.color = new Color(0.78f, 0.86f, 0.9f);
            lightObject.transform.rotation = Quaternion.Euler(55f, -35f, 0f);
        }

        private static void EnsureProjectFolders()
        {
            EnsureFolder("Assets", "_BloodDirective");
            EnsureFolder("Assets/_BloodDirective", "Scenes");
            EnsureFolder("Assets/_BloodDirective/Scenes", "Prototypes");
            EnsureFolder("Assets/_BloodDirective", "Art");
            EnsureFolder("Assets/_BloodDirective/Art", "Materials");
            EnsureFolder("Assets/_BloodDirective/Art/Materials", "Prototype");
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, child);
        }

        private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, string layerName, Transform parent, bool keepCollider = true)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.layer = LayerMask.NameToLayer(layerName);
            cube.GetComponent<Renderer>().sharedMaterial = material;

            if (!keepCollider)
                Object.DestroyImmediate(cube.GetComponent<Collider>());

            return cube;
        }

        private static Material CreateMaterial(string name, Color color)
        {
            string path = $"{MaterialPath}/{name}.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
                return material;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("Standard");

            material = new Material(shader)
            {
                name = name,
                color = color
            };

            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static void EnsureLayer(int index, string layerName)
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            SerializedProperty layer = layers.GetArrayElementAtIndex(index);
            if (string.IsNullOrWhiteSpace(layer.stringValue))
                layer.stringValue = layerName;
            else if (layer.stringValue != layerName)
                Debug.LogWarning($"[MovementPrototype04Builder] Layer {index} is already '{layer.stringValue}', expected '{layerName}'.");
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}