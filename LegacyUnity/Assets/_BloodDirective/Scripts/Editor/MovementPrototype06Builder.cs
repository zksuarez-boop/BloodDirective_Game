using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype06Builder
    {
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype06.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string PlayerLayer = "Player";
        private const string VisualOnlyLayer = "VisualOnly";
        private const string EnemyLayer = "Enemy";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 06")]
        public static void BuildScene()
        {
            EnsureProjectFolders();
            EnsureLayer(8, WalkableLayer);
            EnsureLayer(9, BlockerLayer);
            EnsureLayer(10, PlayerLayer);
            EnsureLayer(11, VisualOnlyLayer);
            EnsureLayer(13, EnemyLayer);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MovementPrototype06";

            Material floorMaterial = CreateMaterial("Prototype06_DarkConcreteFloor", new Color(0.04f, 0.055f, 0.065f));
            Material wallMaterial = CreateMaterial("Prototype06_BunkerConcrete", new Color(0.24f, 0.26f, 0.23f));
            Material metalMaterial = CreateMaterial("Prototype06_DarkMetal", new Color(0.055f, 0.065f, 0.07f));
            Material playerMaterial = CreateMaterial("Prototype_PlayerOlive", new Color(0.24f, 0.34f, 0.22f));
            Material enemyIdleMaterial = CreateMaterial("Prototype06_EnemyIdle", new Color(0.48f, 0.12f, 0.12f));
            Material enemyAlertMaterial = CreateMaterial("Prototype06_EnemyAlert", new Color(0.95f, 0.46f, 0.08f));
            Material enemyDamagedMaterial = CreateMaterial("Prototype06_EnemyDamaged", new Color(0.9f, 0.22f, 0.06f));
            Material enemyDefeatedMaterial = CreateMaterial("Prototype06_EnemyDefeated", new Color(0.08f, 0.08f, 0.08f));
            Material lightMaterial = CreateMaterial("Prototype06_ColdTubeLight", new Color(0.86f, 0.95f, 1f));
            Material rangeMaterial = CreateMaterial("Prototype06_AwarenessRangeMarker", new Color(0.16f, 0.32f, 0.34f));
            Material facingMaterial = CreateMaterial("Prototype06_EnemyFacingMarker", new Color(1f, 0.12f, 0.05f));

            GameObject root = new GameObject("MovementPrototype06_Root");
            BuildWalkableLayout(root.transform, floorMaterial, metalMaterial);
            BuildBlockers(root.transform, wallMaterial, metalMaterial);
            BuildVisualOnlyDetails(root.transform, metalMaterial, lightMaterial, rangeMaterial);

            GameObject player = CreatePlayer(root.transform, playerMaterial);
            GameObject enemy = CreateEnemy(root.transform, enemyIdleMaterial, enemyAlertMaterial, enemyDamagedMaterial, enemyDefeatedMaterial, facingMaterial, player.transform);
            Camera camera = CreateCamera(player.transform);
            ConfigureMover(player, camera);
            ConfigureAttack(player, camera);
            CreateLighting();

            Selection.activeGameObject = enemy;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.035f, 0.043f, 0.045f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildWalkableLayout(Transform root, Material floorMaterial, Material guideMaterial)
        {
            CreateCube("CombatRoom_Walkable", new Vector3(0f, 0f, 0f), new Vector3(12f, 0.2f, 10f), floorMaterial, WalkableLayer, root);
            CreateCube("NorthAlcove_Walkable", new Vector3(0f, 0f, 7f), new Vector3(5f, 0.2f, 4f), floorMaterial, WalkableLayer, root);

            for (int i = 0; i < 7; i++)
                CreateCube($"CombatRoom_FloorJoint_VisualOnly_{i:00}", new Vector3(0f, 0.13f, -3.6f + i * 1.2f), new Vector3(11.2f, 0.035f, 0.035f), guideMaterial, VisualOnlyLayer, root, false);

            for (int i = 0; i < 6; i++)
                CreateCube($"CombatRoom_CrossJoint_VisualOnly_{i:00}", new Vector3(-4.5f + i * 1.8f, 0.13f, 0f), new Vector3(0.035f, 0.035f, 9.2f), guideMaterial, VisualOnlyLayer, root, false);
        }

        private static void BuildBlockers(Transform root, Material wallMaterial, Material metalMaterial)
        {
            CreateCube("WestWall_Blocker", new Vector3(-6.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 10.6f), wallMaterial, BlockerLayer, root);
            CreateCube("EastWall_Blocker", new Vector3(6.3f, 1.4f, 0f), new Vector3(0.6f, 2.8f, 10.6f), wallMaterial, BlockerLayer, root);
            CreateCube("SouthWall_Blocker", new Vector3(0f, 1.4f, -5.3f), new Vector3(12.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("NorthWallLeft_Blocker", new Vector3(-4f, 1.4f, 5.3f), new Vector3(4.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("NorthWallRight_Blocker", new Vector3(4f, 1.4f, 5.3f), new Vector3(4.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("AlcoveWestWall_Blocker", new Vector3(-2.8f, 1.4f, 7f), new Vector3(0.6f, 2.8f, 4.6f), wallMaterial, BlockerLayer, root);
            CreateCube("AlcoveEastWall_Blocker", new Vector3(2.8f, 1.4f, 7f), new Vector3(0.6f, 2.8f, 4.6f), wallMaterial, BlockerLayer, root);
            CreateCube("AlcoveNorthWall_Blocker", new Vector3(0f, 1.4f, 9.3f), new Vector3(5.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);
            CreateCube("CoverBlocker_Left", new Vector3(-2.4f, 0.75f, 1.6f), new Vector3(1.4f, 1.5f, 1.2f), metalMaterial, BlockerLayer, root);
            CreateCube("CoverBlocker_Right", new Vector3(3f, 0.75f, -1.2f), new Vector3(1.4f, 1.5f, 1.2f), metalMaterial, BlockerLayer, root);
        }

        private static void BuildVisualOnlyDetails(Transform root, Material metalMaterial, Material lightMaterial, Material rangeMaterial)
        {
            CreateCube("OverheadPipe_VisualOnly", new Vector3(-1.9f, 3.2f, 1.8f), new Vector3(0.16f, 0.16f, 9f), metalMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_CombatRoom_VisualOnly", new Vector3(0f, 3.28f, -1.8f), new Vector3(2.8f, 0.08f, 0.35f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("ColdLight_Alcove_VisualOnly", new Vector3(0f, 3.28f, 7.4f), new Vector3(2f, 0.08f, 0.32f), lightMaterial, VisualOnlyLayer, root, false);
            CreateCube("EnemyAwarenessRange_VisualOnly", new Vector3(0f, 0.16f, 3.2f), new Vector3(7.8f, 0.035f, 7.8f), rangeMaterial, VisualOnlyLayer, root, false);
        }

        private static GameObject CreatePlayer(Transform root, Material playerMaterial)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "PrototypePlayer";
            player.tag = "Player";
            player.layer = LayerMask.NameToLayer(PlayerLayer);
            player.transform.SetParent(root);
            player.transform.position = new Vector3(0f, 1f, -3.5f);
            player.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            player.AddComponent<PrototypeClickMover>();
            return player;
        }

        private static GameObject CreateEnemy(Transform root, Material idleMaterial, Material alertMaterial, Material damagedMaterial, Material defeatedMaterial, Material facingMaterial, Transform target)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "AwareEnemy_Target";
            enemy.layer = LayerMask.NameToLayer(EnemyLayer);
            enemy.transform.SetParent(root);
            enemy.transform.position = new Vector3(0f, 1f, 3.2f);
            enemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            enemy.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            enemy.GetComponent<Renderer>().sharedMaterial = idleMaterial;
            CreateEnemyFacingIndicator(enemy.transform, facingMaterial);

            var health = enemy.AddComponent<PrototypeHealth>();
            SerializedObject healthSo = new SerializedObject(health);
            healthSo.FindProperty("_maxHealth").intValue = 3;
            healthSo.FindProperty("_renderer").objectReferenceValue = enemy.GetComponent<Renderer>();
            healthSo.FindProperty("_damagedMaterial").objectReferenceValue = damagedMaterial;
            healthSo.FindProperty("_defeatedMaterial").objectReferenceValue = defeatedMaterial;
            healthSo.ApplyModifiedPropertiesWithoutUndo();

            var awareness = enemy.AddComponent<PrototypeEnemyAwareness>();
            SerializedObject awarenessSo = new SerializedObject(awareness);
            awarenessSo.FindProperty("_target").objectReferenceValue = target;
            awarenessSo.FindProperty("_renderer").objectReferenceValue = enemy.GetComponent<Renderer>();
            awarenessSo.FindProperty("_idleMaterial").objectReferenceValue = idleMaterial;
            awarenessSo.FindProperty("_alertMaterial").objectReferenceValue = alertMaterial;
            awarenessSo.FindProperty("_awarenessRadius").floatValue = 4f;
            awarenessSo.FindProperty("_turnSpeed").floatValue = 420f;
            awarenessSo.ApplyModifiedPropertiesWithoutUndo();

            return enemy;
        }

        private static void CreateEnemyFacingIndicator(Transform enemy, Material facingMaterial)
        {
            GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            indicator.name = "EnemyFacingMarker_VisualOnly";
            indicator.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            indicator.transform.SetParent(enemy, false);
            indicator.transform.localPosition = new Vector3(0f, 0.3f, 0.72f);
            indicator.transform.localScale = new Vector3(0.18f, 0.12f, 0.8f);
            indicator.GetComponent<Renderer>().sharedMaterial = facingMaterial;
            Object.DestroyImmediate(indicator.GetComponent<Collider>());
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
            moverSo.FindProperty("_clickBlockerLayer").intValue = 1 << LayerMask.NameToLayer(EnemyLayer);
            moverSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureAttack(GameObject player, Camera camera)
        {
            var attack = player.AddComponent<PrototypePlayerAttack>();
            SerializedObject attackSo = new SerializedObject(attack);
            attackSo.FindProperty("_camera").objectReferenceValue = camera;
            attackSo.FindProperty("_enemyLayer").intValue = 1 << LayerMask.NameToLayer(EnemyLayer);
            attackSo.FindProperty("_attackDamage").intValue = 1;
            attackSo.FindProperty("_attackRange").floatValue = 2.2f;
            attackSo.FindProperty("_attackCooldown").floatValue = 0.35f;
            attackSo.ApplyModifiedPropertiesWithoutUndo();
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
                Debug.LogWarning($"[MovementPrototype06Builder] Layer {index} is already '{layer.stringValue}', expected '{layerName}'.");
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
