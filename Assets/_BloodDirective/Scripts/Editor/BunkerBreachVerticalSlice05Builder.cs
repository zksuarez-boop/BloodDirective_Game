using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Adds mission briefing, state callouts, timing, completion, and replay flow to Bunker Breach.
    /// </summary>
    public static class BunkerBreachVerticalSlice05Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice04.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05.unity";
        private const string SourceRootName = "BunkerBreachVerticalSlice04_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice05_Root";

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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 05 - Mission Polish")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice04Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice05Builder] Vertical Slice 04 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            PrototypeScientistRescue scientist = FindTransform(scene, "ScientistRescue_Interactable")?.GetComponent<PrototypeScientistRescue>();
            PrototypeHealth breachGrey = FindTransform(scene, "AwareEnemy_Target")?.GetComponent<PrototypeHealth>();
            PrototypeAetherPickup aether = FindTransform(scene, "AetherSample_Interactable")?.GetComponent<PrototypeAetherPickup>();
            PrototypeCalibrationGuard guard = FindTransform(scene, "ExtractionGuard_Target")?.GetComponent<PrototypeCalibrationGuard>();
            PrototypeHealth guardHealth = FindTransform(scene, "ExtractionGuard_Target")?.GetComponent<PrototypeHealth>();
            PrototypeExtractionZone extraction = FindTransform(scene, "ExtractionZone_Trigger")?.GetComponent<PrototypeExtractionZone>();
            if (root == null || scientist == null || breachGrey == null || aether == null || guard == null || guardHealth == null || extraction == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice05Builder] Required Vertical Slice 04 components were not found.");
                return;
            }

            root.name = SliceRootName;
            BuildMissionCanvas(root, scientist, breachGrey, aether, guard, guardHealth, extraction);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = root.gameObject;
        }

        private static void BuildMissionCanvas(Transform root, PrototypeScientistRescue scientist, PrototypeHealth breachGrey, PrototypeAetherPickup aether, PrototypeCalibrationGuard guard, PrototypeHealth guardHealth, PrototypeExtractionZone extraction)
        {
            GameObject canvasObject = new GameObject("VerticalSlice05_MissionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(root, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            Color background = new Color(0.01f, 0.02f, 0.032f, 0.91f);
            Text title = CreatePanelText(canvasObject.transform, "MissionBrief", new Vector2(0f, -96f), new Vector2(640f, 52f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), TextAnchor.MiddleCenter, background, new Color(0.72f, 0.9f, 1f), 20);
            title.text = "OPERATION // BUNKER BREACH";
            Text status = CreatePanelText(canvasObject.transform, "MissionStatus", new Vector2(0f, -158f), new Vector2(760f, 48f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), TextAnchor.MiddleCenter, background, new Color(0.72f, 0.9f, 1f), 17);
            Text timer = CreatePanelText(canvasObject.transform, "MissionTimer", new Vector2(28f, 30f), new Vector2(250f, 46f), new Vector2(0f, 0f), new Vector2(0f, 0f), TextAnchor.MiddleLeft, background, new Color(0.63f, 0.74f, 0.82f), 16);
            GameObject completionPanel = CreateCompletionPanel(canvasObject.transform, background, out Text completion);

            VerticalSliceMissionFlowController controller = canvasObject.AddComponent<VerticalSliceMissionFlowController>();
            SerializedObject controllerSo = new SerializedObject(controller);
            controllerSo.FindProperty("_scientist").objectReferenceValue = scientist;
            controllerSo.FindProperty("_breachGrey").objectReferenceValue = breachGrey;
            controllerSo.FindProperty("_aether").objectReferenceValue = aether;
            controllerSo.FindProperty("_guard").objectReferenceValue = guard;
            controllerSo.FindProperty("_guardHealth").objectReferenceValue = guardHealth;
            controllerSo.FindProperty("_extraction").objectReferenceValue = extraction;
            controllerSo.FindProperty("_statusLabel").objectReferenceValue = status;
            controllerSo.FindProperty("_timerLabel").objectReferenceValue = timer;
            controllerSo.FindProperty("_completionLabel").objectReferenceValue = completion;
            controllerSo.FindProperty("_completionPanel").objectReferenceValue = completionPanel;
            controllerSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static GameObject CreateCompletionPanel(Transform parent, Color background, out Text label)
        {
            GameObject panel = new GameObject("MissionCompletePanel", typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(560f, 260f);
            panel.GetComponent<Image>().color = background;

            label = CreateLabel(panel.transform, "MissionCompleteLabel", TextAnchor.MiddleCenter, new Color(0.72f, 1f, 0.4f), 26, new Vector2(28f, 24f), new Vector2(-28f, -24f));
            return panel;
        }

        private static Text CreatePanelText(Transform parent, string name, Vector2 position, Vector2 size, Vector2 anchor, Vector2 pivot, TextAnchor alignment, Color background, Color textColor, int fontSize)
        {
            GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            panel.GetComponent<Image>().color = background;
            return CreateLabel(panel.transform, name + "Label", alignment, textColor, fontSize, new Vector2(16f, 0f), new Vector2(-16f, 0f));
        }

        private static Text CreateLabel(Transform parent, string name, TextAnchor alignment, Color color, int fontSize, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject labelObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(parent, false);
            RectTransform rect = labelObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Text label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = fontSize;
            label.fontStyle = FontStyle.Bold;
            label.alignment = alignment;
            label.color = color;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            return label;
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene.path == scenePath)
                    return;
            }

            var updatedScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            scenes.CopyTo(updatedScenes, 0);
            updatedScenes[updatedScenes.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = updatedScenes;
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
