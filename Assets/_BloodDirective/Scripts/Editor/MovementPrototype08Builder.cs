using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using BloodDirective.Prototypes.Movement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype08Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype07.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype08.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string VisualOnlyLayer = "VisualOnly";
        private const string InteractableLayer = "Interactable";
        private const string EnemyLayer = "Enemy";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 08")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype07Builder.BuildScene();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);

            EnsureLayer(12, InteractableLayer);
            Transform root = FindTransform(scene, "MovementPrototype07_Root");
            if (root == null)
                return;

            root.name = "MovementPrototype08_Root";
            Transform player = FindTransform(root, "PrototypePlayer");
            Transform enemy = FindTransform(root, "AwareEnemy_Target");
            if (player == null || enemy == null)
                return;

            Material floorMaterial = CreateMaterial("Prototype08_ExtractionFloor", new Color(0.045f, 0.07f, 0.075f));
            Material wallMaterial = CreateMaterial("Prototype08_ExtractionWall", new Color(0.24f, 0.26f, 0.23f));
            Material doorMaterial = CreateMaterial("Prototype08_SealedBlastDoor", new Color(0.07f, 0.08f, 0.085f));
            Material doorOpenMaterial = CreateMaterial("Prototype08_OpenBlastDoor", new Color(0.1f, 0.48f, 0.28f));
            Material aetherLockedMaterial = CreateMaterial("Prototype08_AetherLocked", new Color(0.12f, 0.13f, 0.15f));
            Material aetherReadyMaterial = CreateMaterial("Prototype08_AetherReady", new Color(0.12f, 0.75f, 0.8f));
            Material aetherCollectedMaterial = CreateMaterial("Prototype08_AetherCollected", new Color(0.28f, 0.95f, 0.55f));
            Material exitReadyMaterial = CreateMaterial("Prototype08_ExtractionReady", new Color(0.14f, 0.32f, 0.22f));
            Material exitCompletedMaterial = CreateMaterial("Prototype08_ExtractionComplete", new Color(0.62f, 0.9f, 0.28f));

            BuildExtractionRoom(root, floorMaterial, wallMaterial, doorMaterial, doorOpenMaterial, aetherLockedMaterial, aetherReadyMaterial, aetherCollectedMaterial, exitReadyMaterial, exitCompletedMaterial, player, enemy);
            ConfigurePlayer(player.gameObject);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.035f, 0.043f, 0.045f);
            Selection.activeGameObject = player.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildExtractionRoom(Transform root, Material floorMaterial, Material wallMaterial, Material doorMaterial, Material doorOpenMaterial, Material aetherLockedMaterial, Material aetherReadyMaterial, Material aetherCollectedMaterial, Material exitReadyMaterial, Material exitCompletedMaterial, Transform player, Transform enemy)
        {
            Transform previousNorthWall = FindTransform(root, "AlcoveNorthWall_Blocker");
            if (previousNorthWall != null)
                Object.DestroyImmediate(previousNorthWall.gameObject);

            CreateCube("ExtractionRoom_Walkable", new Vector3(0f, 0f, 11.5f), new Vector3(5f, 0.2f, 4f), floorMaterial, WalkableLayer, root);
            CreateCube("ExtractionRoom_WestWall_Blocker", new Vector3(-2.8f, 1.4f, 11.5f), new Vector3(0.6f, 2.8f, 4.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ExtractionRoom_EastWall_Blocker", new Vector3(2.8f, 1.4f, 11.5f), new Vector3(0.6f, 2.8f, 4.6f), wallMaterial, BlockerLayer, root);
            CreateCube("ExtractionRoom_NorthWall_Blocker", new Vector3(0f, 1.4f, 13.8f), new Vector3(5.6f, 2.8f, 0.6f), wallMaterial, BlockerLayer, root);

            GameObject blastDoor = CreateCube("ExtractionBlastDoor_Locked", new Vector3(0f, 1.4f, 9.3f), new Vector3(3.6f, 2.8f, 0.45f), doorMaterial, BlockerLayer, root);
            var lockedDoor = blastDoor.AddComponent<PrototypeLockedDoor>();
            SerializedObject doorSo = new SerializedObject(lockedDoor);
            doorSo.FindProperty("_blockingCollider").objectReferenceValue = blastDoor.GetComponent<Collider>();
            doorSo.FindProperty("_doorRenderer").objectReferenceValue = blastDoor.GetComponent<Renderer>();
            doorSo.FindProperty("_openedMaterial").objectReferenceValue = doorOpenMaterial;
            doorSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject aether = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            aether.name = "AetherSample_Interactable";
            aether.layer = LayerMask.NameToLayer(InteractableLayer);
            aether.transform.SetParent(root);
            aether.transform.position = new Vector3(0f, 0.75f, 7f);
            aether.transform.localScale = new Vector3(0.65f, 0.75f, 0.65f);
            aether.GetComponent<Renderer>().sharedMaterial = aetherLockedMaterial;

            var pickup = aether.AddComponent<PrototypeAetherPickup>();
            SerializedObject pickupSo = new SerializedObject(pickup);
            pickupSo.FindProperty("_requiredEnemy").objectReferenceValue = enemy.GetComponent<PrototypeHealth>();
            pickupSo.FindProperty("_playerHealth").objectReferenceValue = player.GetComponent<PrototypePlayerHealth>();
            pickupSo.FindProperty("_extractionDoor").objectReferenceValue = lockedDoor;
            pickupSo.FindProperty("_renderer").objectReferenceValue = aether.GetComponent<Renderer>();
            pickupSo.FindProperty("_lockedMaterial").objectReferenceValue = aetherLockedMaterial;
            pickupSo.FindProperty("_availableMaterial").objectReferenceValue = aetherReadyMaterial;
            pickupSo.FindProperty("_collectedMaterial").objectReferenceValue = aetherCollectedMaterial;
            pickupSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject exit = CreateCube("ExtractionZone_Trigger", new Vector3(0f, 0.15f, 12f), new Vector3(2.3f, 0.16f, 1.5f), exitReadyMaterial, VisualOnlyLayer, root);
            BoxCollider exitCollider = exit.GetComponent<BoxCollider>();
            exitCollider.isTrigger = true;
            var extraction = exit.AddComponent<PrototypeExtractionZone>();
            SerializedObject extractionSo = new SerializedObject(extraction);
            extractionSo.FindProperty("_requiredAether").objectReferenceValue = pickup;
            extractionSo.FindProperty("_renderer").objectReferenceValue = exit.GetComponent<Renderer>();
            extractionSo.FindProperty("_readyMaterial").objectReferenceValue = exitReadyMaterial;
            extractionSo.FindProperty("_completedMaterial").objectReferenceValue = exitCompletedMaterial;
            extractionSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigurePlayer(GameObject player)
        {
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody == null)
            {
                playerRigidbody = player.AddComponent<Rigidbody>();
                playerRigidbody.useGravity = false;
                playerRigidbody.isKinematic = true;
            }

            PrototypeClickMover mover = player.GetComponent<PrototypeClickMover>();
            SerializedObject moverSo = new SerializedObject(mover);
            moverSo.FindProperty("_clickBlockerLayer").intValue = (1 << LayerMask.NameToLayer(EnemyLayer)) | (1 << LayerMask.NameToLayer(InteractableLayer));
            moverSo.ApplyModifiedPropertiesWithoutUndo();

            GameObject clickerObject = new GameObject("PrototypeInteractionClicker");
            var clicker = clickerObject.AddComponent<PrototypeInteractionClicker>();
            SerializedObject clickerSo = new SerializedObject(clicker);
            Transform cameraTransform = FindTransform(SceneManager.GetActiveScene(), "PrototypeIsometricCamera");
            clickerSo.FindProperty("_camera").objectReferenceValue = cameraTransform != null ? cameraTransform.GetComponent<Camera>() : null;
            clickerSo.FindProperty("_interactableLayer").intValue = 1 << LayerMask.NameToLayer(InteractableLayer);
            clickerSo.ApplyModifiedPropertiesWithoutUndo();
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

        private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, string layerName, Transform parent)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.layer = LayerMask.NameToLayer(layerName);
            cube.GetComponent<Renderer>().sharedMaterial = material;
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

            material = new Material(shader) { name = name, color = color };
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
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
