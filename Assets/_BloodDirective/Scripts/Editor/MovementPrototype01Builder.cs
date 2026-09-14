using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype01Builder
    {
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype01.unity";
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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 01")]
        public static void BuildScene()
        {
            EnsureProjectFolders();
            EnsureLayer(8, WalkableLayer);
            EnsureLayer(9, BlockerLayer);
            EnsureLayer(10, PlayerLayer);
            EnsureLayer(11, VisualOnlyLayer);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MovementPrototype01";

            Material floorMaterial = CreateMaterial("Prototype_DarkConcreteFloor", new Color(0.06f, 0.09f, 0.10f));
            Material wallMaterial = CreateMaterial("Prototype_ConcreteWall", new Color(0.28f, 0.30f, 0.27f));
            Material blockerMaterial = CreateMaterial("Prototype_BlockerDark", new Color(0.12f, 0.14f, 0.14f));
            Material playerMaterial = CreateMaterial("Prototype_PlayerOlive", new Color(0.24f, 0.34f, 0.22f));
            Material accentMaterial = CreateMaterial("Prototype_AlarmRed", new Color(0.9f, 0.08f, 0.05f));

            GameObject root = new GameObject("MovementPrototype01_Root");

            GameObject walkable = CreateCube("WalkableFloor", new Vector3(0f, 0f, 12f), new Vector3(6f, 0.2f, 28f), floorMaterial, WalkableLayer, root.transform);
            walkable.GetComponent<BoxCollider>().size = Vector3.one;

            CreateCube("LeftWall_Blocker", new Vector3(-3.4f, 1.5f, 12f), new Vector3(0.6f, 3f, 28f), wallMaterial, BlockerLayer, root.transform);
            CreateCube("RightWall_Blocker", new Vector3(3.4f, 1.5f, 12f), new Vector3(0.6f, 3f, 28f), wallMaterial, BlockerLayer, root.transform);
            CreateCube("StartBackWall_Blocker", new Vector3(0f, 1.5f, -2.2f), new Vector3(7.4f, 3f, 0.6f), wallMaterial, BlockerLayer, root.transform);
            CreateCube("ClosedSideBlocker_Test", new Vector3(1.4f, 1f, 8f), new Vector3(1.8f, 2f, 2f), blockerMaterial, BlockerLayer, root.transform);

            for (int i = 0; i < 8; i++)
            {
                float z = -0.75f + i * 3.25f;
                CreateCube($"FloorSeam_VisualOnly_{i:00}", new Vector3(0f, 0.12f, z), new Vector3(5.5f, 0.04f, 0.04f), blockerMaterial, VisualOnlyLayer, root.transform, false);
            }

            CreateCube("OverheadPipe_VisualOnly_Left", new Vector3(-1.8f, 3.1f, 11f), new Vector3(0.18f, 0.18f, 24f), blockerMaterial, VisualOnlyLayer, root.transform, false);
            CreateCube("OverheadPipe_VisualOnly_Right", new Vector3(1.8f, 3.1f, 11f), new Vector3(0.18f, 0.18f, 24f), blockerMaterial, VisualOnlyLayer, root.transform, false);
            CreateCube("Fluorescent_VisualOnly_Start", new Vector3(0f, 3.3f, 2f), new Vector3(2.4f, 0.08f, 0.35f), CreateMaterial("Prototype_ColdLight", new Color(0.85f, 0.95f, 1f)), VisualOnlyLayer, root.transform, false);
            CreateCube("Fluorescent_VisualOnly_End", new Vector3(0f, 3.3f, 19f), new Vector3(2.4f, 0.08f, 0.35f), CreateMaterial("Prototype_ColdLight", new Color(0.85f, 0.95f, 1f)), VisualOnlyLayer, root.transform, false);
            CreateCube("AlarmLight_VisualOnly", new Vector3(-3.05f, 2.2f, 6f), new Vector3(0.2f, 0.2f, 0.2f), accentMaterial, VisualOnlyLayer, root.transform, false);

            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "PrototypePlayer";
            player.layer = LayerMask.NameToLayer(PlayerLayer);
            player.transform.SetParent(root.transform);
            player.transform.position = new Vector3(0f, 1f, 0f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            player.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            var mover = player.AddComponent<PrototypeClickMover>();
            SerializedObject moverSo = new SerializedObject(mover);
            moverSo.FindProperty("_walkableLayer").intValue = 1 << LayerMask.NameToLayer(WalkableLayer);
            moverSo.FindProperty("_blockerLayer").intValue = 1 << LayerMask.NameToLayer(BlockerLayer);
            moverSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject cameraObject = new GameObject("PrototypeIsometricCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.015f, 0.02f, 0.025f);
            camera.fieldOfView = 45f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 250f;
            cameraObject.tag = "MainCamera";
            var cameraFollow = cameraObject.AddComponent<PrototypeIsometricCamera>();
            cameraFollow.SetTarget(player.transform);
            moverSo = new SerializedObject(mover);
            moverSo.FindProperty("_camera").objectReferenceValue = camera;
            moverSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject lightObject = new GameObject("PrototypeDirectionalLight");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            light.color = new Color(0.82f, 0.88f, 0.92f);
            lightObject.transform.rotation = Quaternion.Euler(55f, -35f, 0f);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.08f, 0.09f, 0.09f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
                Debug.LogWarning($"[MovementPrototype01Builder] Layer {index} is already '{layer.stringValue}', expected '{layerName}'.");
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}

