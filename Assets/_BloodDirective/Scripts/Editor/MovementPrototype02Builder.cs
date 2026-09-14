using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype02Builder
    {
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype02.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string PlayerLayer = "Player";
        private const string VisualOnlyLayer = "VisualOnly";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 02")]
        public static void BuildScene()
        {
            EnsureProjectFolders();
            EnsureLayer(8, WalkableLayer);
            EnsureLayer(9, BlockerLayer);
            EnsureLayer(10, PlayerLayer);
            EnsureLayer(11, VisualOnlyLayer);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MovementPrototype02";

            Material floorMaterial = CreateMaterial("Prototype02_DarkConcreteFloor", new Color(0.055f, 0.075f, 0.085f));
            Material wallMaterial = CreateMaterial("Prototype02_PouredConcreteWall", new Color(0.26f, 0.28f, 0.25f));
            Material darkMaterial = CreateMaterial("Prototype02_DarkMetal", new Color(0.08f, 0.095f, 0.1f));
            Material playerMaterial = CreateMaterial("Prototype_PlayerOlive", new Color(0.24f, 0.34f, 0.22f));
            Material lightMaterial = CreateMaterial("Prototype02_ColdLight", new Color(0.88f, 0.96f, 1f));
            Material alarmMaterial = CreateMaterial("Prototype02_AlarmRed", new Color(0.95f, 0.08f, 0.04f));
            Material guideMaterial = CreateMaterial("Prototype02_TraversalGuide", new Color(0.18f, 0.32f, 0.35f));

            GameObject root = new GameObject("MovementPrototype02_Root");

            BuildWalkableLayout(root.transform, floorMaterial, guideMaterial);
            BuildBlockers(root.transform, wallMaterial, darkMaterial);
            BuildVisualOnlyDetails(root.transform, darkMaterial, lightMaterial, alarmMaterial);

            GameObject player = CreatePlayer(root.transform, playerMaterial);
            Camera camera = CreateCamera(player.transform);
            ConfigureMover(player, camera);
            CreateLighting();

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.045f, 0.055f, 0.055f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildWalkableLayout(Transform root, Material floorMaterial, Material guideMaterial)
        {
            CreateCube("StartRoom_Walkable", new Vector3(0f, 0f, 0f), new Vector3(10f, 0.2f, 8f), floorMaterial, WalkableLayer, root);
            CreateCube("NorthCorridor_Walkable", new Vector3(0f, 0f, 10.5f), new Vector3(4f, 0.2f, 13f), floorMaterial, WalkableLayer, root);
            CreateCube("EastRoom_Walkable", new Vector3(7f, 0f, 12f), new Vector3(10f, 0.2f, 8f), floorMaterial, WalkableLayer, root);

            for (int i = 0; i < 6; i++)
            {
                float z = -2.8f + i * 1.35f;
                CreateCube($"StartRoom_FloorJoint_VisualOnly_{i:00}", new Vector3(0f, 0.13f, z), new Vector3(9.2f, 0.035f, 0.035f), guideMaterial, VisualOnlyLayer, root, false);
            }

            for (int i = 0; i < 7; i++)
            {
                float z = 5f + i * 1.7f;
                CreateCube($"Corridor_FloorJoint_VisualOnly_{i:00}", new Vector3(0f, 0.13f, z), new Vector3(3.5f, 0.035f, 0.035f), guideMaterial, VisualOnlyLayer, root, false);
            }

            for (int i = 0; i < 6; i++)
            {
                float x = 2.9f + i * 1.65f;
                CreateCube($"EastRoom_FloorJoint_VisualOnly_{i:00}", new Vector3(x, 0.13f, 12f), new Vector3(0.035f, 0.035f, 7.3f), guideMaterial, VisualOnlyLayer, root, false);
            }
        }

        private static void BuildBlockers(Transform root, Material wallMaterial, Material darkMaterial)
        {
            CreateCube("StartRoom_WestWall_Blocker", new Vector3(-5.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 8.7f), wallMaterial, BlockerLayer, root);
            CreateCube("StartRoom_EastWall_Blocker", new Vector3(5.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 8.7f), wallMaterial, BlockerLayer, root);
            CreateCube("StartRoom_SouthWall_Blocker", new Vector3(0f, 1.4f, -4.3f), new Vector3(10.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("StartRoom_NorthWallLeft_Blocker", new Vector3(-3.6f, 1.4f, 4.3f), new Vector3(2.8f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("StartRoom_NorthWallRight_Blocker", new Vector3(3.6f, 1.4f, 4.3f), new Vector3(2.8f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            CreateCube("Corridor_WestWall_Blocker", new Vector3(-2.3f, 1.4f, 10.5f), new Vector3(0.6f, 2.8f, 13f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_EastWallLower_Blocker", new Vector3(2.3f, 1.4f, 7.15f), new Vector3(0.6f, 2.8f, 5.9f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_EastWallUpper_Blocker", new Vector3(2.3f, 1.4f, 16.25f), new Vector3(0.6f, 2.8f, 2.5f), wallMaterial, BlockerLayer, root);
            CreateCube("Corridor_NorthCap_Blocker", new Vector3(0f, 1.4f, 17.3f), new Vector3(4.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            CreateCube("EastRoom_NorthWall_Blocker", new Vector3(7f, 1.4f, 16.3f), new Vector3(10.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("EastRoom_SouthWall_Blocker", new Vector3(7f, 1.4f, 7.7f), new Vector3(10.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("EastRoom_EastWall_Blocker", new Vector3(12.3f, 1.4f, 12f), new Vector3(0.6f, 2.8f, 8.6f), wallMaterial, BlockerLayer, root);
            CreateCube("EastRoom_WestWallUpper_Blocker", new Vector3(2.7f, 1.4f, 14.8f), new Vector3(0.6f, 2.8f, 3f), wallMaterial, BlockerLayer, root);
            CreateCube("EastRoom_WestWallLower_Blocker", new Vector3(2.7f, 1.4f, 8.9f), new Vector3(0.6f, 2.8f, 1.8f), wallMaterial, BlockerLayer, root);

            CreateCube("NavigationBlocker_TestCrate", new Vector3(7f, 0.85f, 12f), new Vector3(1.6f, 1.7f, 1.6f), darkMaterial, BlockerLayer, root);
            CreateCube("NavigationBlocker_Pillar", new Vector3(-3.2f, 0.95f, -1.8f), new Vector3(1.1f, 1.9f, 1.1f), darkMaterial, BlockerLayer, root);
        }

        private static void BuildVisualOnlyDetails(Transform root, Material darkMaterial, Material lightMaterial, Material alarmMaterial)
        {
            CreateCube("DoorHeader_StartToCorridor_VisualOnly", new Vector3(0f, 3.05f, 4.3f), new Vector3(4.4f, 0.32f, 0.7f), darkMaterial, VisualOnlyLayer, root, false);
            CreateCube("DoorHeader_CorridorToEastRoom_VisualOnly", new Vector3(2.35f, 3.05f, 12f), new Vector3(0.7f, 0.32f, 4.4f), darkMaterial, VisualOnlyLayer, root, false);
            CreateCube("OverheadPipe_NorthSouth_VisualOnly", new Vector3(-1.15f, 3.25f, 10.5f), new Vector3(0.16f, 0.16f, 12f), darkMaterial, VisualOnlyLayer, root, false);
            CreateCube("OverheadPipe_EastRoom_VisualOnly", new Vector3(7f, 3.2f, 13.2f), new Vector3(8f, 0.16f, 0.16f), darkMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_StartRoom_VisualOnly", new Vector3(0f, 3.28f, -1.4f), new Vector3(2.4f, 0.08f, 0.35f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_Corridor_VisualOnly", new Vector3(0f, 3.28f, 10.5f), new Vector3(1.7f, 0.08f, 0.32f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_EastRoom_VisualOnly", new Vector3(7.4f, 3.28f, 14.2f), new Vector3(2.4f, 0.08f, 0.35f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("AlarmBeacon_Start_VisualOnly", new Vector3(-4.8f, 2.05f, 2.8f), new Vector3(0.22f, 0.22f, 0.22f), alarmMaterial, VisualOnlyLayer, root, false);
            CreateCube("AlarmBeacon_EastRoom_VisualOnly", new Vector3(11.8f, 2.05f, 9.6f), new Vector3(0.22f, 0.22f, 0.22f), alarmMaterial, VisualOnlyLayer, root, false);
        }

        private static GameObject CreatePlayer(Transform root, Material playerMaterial)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "PrototypePlayer";
            player.layer = LayerMask.NameToLayer(PlayerLayer);
            player.transform.SetParent(root);
            player.transform.position = new Vector3(0f, 1f, -2.2f);
            player.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            player.AddComponent<PrototypeClickMover>();
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
            moverSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateLighting()
        {
            GameObject lightObject = new GameObject("PrototypeDirectionalLight");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.85f;
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
                Debug.LogWarning($"[MovementPrototype02Builder] Layer {index} is already '{layer.stringValue}', expected '{layerName}'.");
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}