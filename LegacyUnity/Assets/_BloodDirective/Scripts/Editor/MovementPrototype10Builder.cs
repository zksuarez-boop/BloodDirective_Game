using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype10Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype09.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype10.unity";

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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 10")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype09Builder.BuildScene();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, "MovementPrototype09_Root");
            if (root == null)
                return;

            root.name = "MovementPrototype10_Root";
            PrototypeScientistRescue scientist = FindTransform(root, "ScientistRescue_Interactable")?.GetComponent<PrototypeScientistRescue>();
            PrototypeHealth grey = FindTransform(root, "AwareEnemy_Target")?.GetComponent<PrototypeHealth>();
            PrototypeAetherPickup aether = FindTransform(root, "AetherSample_Interactable")?.GetComponent<PrototypeAetherPickup>();
            PrototypeExtractionZone extraction = FindTransform(root, "ExtractionZone_Trigger")?.GetComponent<PrototypeExtractionZone>();
            if (scientist == null || grey == null || aether == null || extraction == null)
                return;

            PrototypeObjectiveDisplay display = CreateObjectiveDisplay(root);
            var state = root.gameObject.AddComponent<PrototypeObjectiveState>();
            SerializedObject stateSo = new SerializedObject(state);
            stateSo.FindProperty("_scientist").objectReferenceValue = scientist;
            stateSo.FindProperty("_grey").objectReferenceValue = grey;
            stateSo.FindProperty("_aether").objectReferenceValue = aether;
            stateSo.FindProperty("_extraction").objectReferenceValue = extraction;
            stateSo.FindProperty("_display").objectReferenceValue = display;
            stateSo.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = root.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static PrototypeObjectiveDisplay CreateObjectiveDisplay(Transform parent)
        {
            GameObject canvasObject = new GameObject("PrototypeObjectiveCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(parent, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject panelObject = new GameObject("ObjectivePanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(canvasObject.transform, false);
            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 1f);
            panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.pivot = new Vector2(0.5f, 1f);
            panelRect.anchoredPosition = new Vector2(0f, -28f);
            panelRect.sizeDelta = new Vector2(500f, 58f);
            panelObject.GetComponent<Image>().color = new Color(0.015f, 0.025f, 0.035f, 0.86f);

            GameObject labelObject = new GameObject("ObjectiveLabel", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(panelObject.transform, false);
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(16f, 8f);
            labelRect.offsetMax = new Vector2(-16f, -8f);

            Text label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 24;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;

            var display = canvasObject.AddComponent<PrototypeObjectiveDisplay>();
            SerializedObject displaySo = new SerializedObject(display);
            displaySo.FindProperty("_label").objectReferenceValue = label;
            displaySo.ApplyModifiedPropertiesWithoutUndo();
            return display;
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
