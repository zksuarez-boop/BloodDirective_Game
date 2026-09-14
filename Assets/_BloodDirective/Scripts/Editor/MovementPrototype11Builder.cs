using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype11Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype10.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype11.unity";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 11")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype10Builder.BuildScene();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, "MovementPrototype10_Root");
            Transform aether = FindTransform(root, "AetherSample_Interactable");
            Transform canvas = FindTransform(root, "PrototypeObjectiveCanvas");
            if (root == null || aether == null || canvas == null)
                return;

            root.name = "MovementPrototype11_Root";
            var wallet = root.gameObject.AddComponent<PrototypeAetherWallet>();
            PrototypeAetherPickup pickup = aether.GetComponent<PrototypeAetherPickup>();
            SerializedObject pickupSo = new SerializedObject(pickup);
            pickupSo.FindProperty("_wallet").objectReferenceValue = wallet;
            pickupSo.ApplyModifiedPropertiesWithoutUndo();

            CreateAetherCounter(canvas, wallet);
            Selection.activeGameObject = root.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateAetherCounter(Transform canvas, PrototypeAetherWallet wallet)
        {
            GameObject panelObject = new GameObject("AetherCounterPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(canvas, false);
            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1f, 1f);
            panelRect.anchorMax = new Vector2(1f, 1f);
            panelRect.pivot = new Vector2(1f, 1f);
            panelRect.anchoredPosition = new Vector2(-28f, -28f);
            panelRect.sizeDelta = new Vector2(190f, 48f);
            panelObject.GetComponent<Image>().color = new Color(0.015f, 0.025f, 0.035f, 0.86f);

            GameObject labelObject = new GameObject("AetherCounterLabel", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(panelObject.transform, false);
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(12f, 4f);
            labelRect.offsetMax = new Vector2(-12f, -4f);

            Text label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 22;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = new Color(0.2f, 0.92f, 1f);

            var display = panelObject.AddComponent<PrototypeAetherDisplay>();
            SerializedObject displaySo = new SerializedObject(display);
            displaySo.FindProperty("_wallet").objectReferenceValue = wallet;
            displaySo.FindProperty("_label").objectReferenceValue = label;
            displaySo.ApplyModifiedPropertiesWithoutUndo();
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
            if (parent == null)
                return null;

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
