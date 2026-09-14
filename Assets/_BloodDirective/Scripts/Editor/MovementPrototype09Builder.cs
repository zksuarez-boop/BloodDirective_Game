using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype09Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype08.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype09.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string InteractableLayer = "Interactable";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 09")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype08Builder.BuildScene();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, "MovementPrototype08_Root");
            if (root == null)
                return;

            root.name = "MovementPrototype09_Root";
            Transform player = FindTransform(root, "PrototypePlayer");
            Transform aether = FindTransform(root, "AetherSample_Interactable");
            if (player == null || aether == null)
                return;

            Material waitingMaterial = CreateMaterial("Prototype09_ScientistWaiting", new Color(0.28f, 0.34f, 0.38f));
            Material rescuedMaterial = CreateMaterial("Prototype09_ScientistRescued", new Color(0.3f, 0.75f, 0.55f));
            GameObject scientist = CreateScientist(root, player, waitingMaterial, rescuedMaterial);

            PrototypeAetherPickup pickup = aether.GetComponent<PrototypeAetherPickup>();
            if (pickup == null)
                return;

            SerializedObject pickupSo = new SerializedObject(pickup);
            pickupSo.FindProperty("_requiredScientist").objectReferenceValue = scientist.GetComponent<PrototypeScientistRescue>();
            pickupSo.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = scientist;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static GameObject CreateScientist(Transform root, Transform player, Material waitingMaterial, Material rescuedMaterial)
        {
            GameObject scientist = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            scientist.name = "ScientistRescue_Interactable";
            scientist.layer = LayerMask.NameToLayer(InteractableLayer);
            scientist.transform.SetParent(root);
            scientist.transform.position = new Vector3(-4.15f, 1f, -2.6f);
            scientist.transform.localScale = new Vector3(0.85f, 1.05f, 0.85f);
            scientist.GetComponent<Renderer>().sharedMaterial = waitingMaterial;

            var rescue = scientist.AddComponent<PrototypeScientistRescue>();
            SerializedObject rescueSo = new SerializedObject(rescue);
            rescueSo.FindProperty("_playerHealth").objectReferenceValue = player.GetComponent<PrototypePlayerHealth>();
            rescueSo.FindProperty("_renderer").objectReferenceValue = scientist.GetComponent<Renderer>();
            rescueSo.FindProperty("_waitingMaterial").objectReferenceValue = waitingMaterial;
            rescueSo.FindProperty("_rescuedMaterial").objectReferenceValue = rescuedMaterial;
            rescueSo.ApplyModifiedPropertiesWithoutUndo();

            return scientist;
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
    }
}
