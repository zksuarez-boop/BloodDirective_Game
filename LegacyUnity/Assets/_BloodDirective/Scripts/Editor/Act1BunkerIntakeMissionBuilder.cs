using BloodDirective.Data;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Adds Act 1 mission-definition briefing presentation to the passed encounter scene.
    /// </summary>
    public static class Act1BunkerIntakeMissionBuilder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeEncounter01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntakeMission01.unity";
        private const string SourceRootName = "Act1_BunkerIntakeEncounter01_Root";
        private const string MissionRootName = "Act1_BunkerIntakeMission01_Root";
        private const string PresentationRootName = "Act1BunkerIntake_MissionPresentation";
        private const string MissionPath = "Assets/_BloodDirective/ScriptableObjects/Missions/Act1_BunkerIntake.asset";

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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 04 - Bunker Intake Mission Presentation")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                Act1BunkerIntakeEncounterBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeMissionBuilder] Act 1 Production 03 was not found or built.");
                return;
            }

            MissionDefinition mission = AssetDatabase.LoadAssetAtPath<MissionDefinition>(MissionPath);
            if (mission == null)
            {
                Debug.LogError("[Act1BunkerIntakeMissionBuilder] Act 1 Bunker Intake mission definition was not found.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeMissionBuilder] Act 1 Production 03 root was not found.");
                return;
            }

            root.name = MissionRootName;
            Transform existingPresentation = root.Find(PresentationRootName);
            if (existingPresentation != null)
                Object.DestroyImmediate(existingPresentation.gameObject);

            BuildMissionPresentation(root, mission);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildMissionPresentation(Transform root, MissionDefinition mission)
        {
            GameObject presentationRoot = new GameObject(PresentationRootName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            presentationRoot.transform.SetParent(root, false);

            Canvas canvas = presentationRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 35;

            CanvasScaler scaler = presentationRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject panel = CreateBriefingPanel(presentationRoot.transform, out Text label);
            Act1MissionBriefingPresenter presenter = presentationRoot.AddComponent<Act1MissionBriefingPresenter>();
            SerializedObject presenterSo = new SerializedObject(presenter);
            presenterSo.FindProperty("_mission").objectReferenceValue = mission;
            presenterSo.FindProperty("_briefingPanel").objectReferenceValue = panel;
            presenterSo.FindProperty("_briefingLabel").objectReferenceValue = label;
            presenterSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static GameObject CreateBriefingPanel(Transform parent, out Text label)
        {
            GameObject panel = new GameObject("MissionBriefingPanel", typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1f, 1f);
            panelRect.anchorMax = new Vector2(1f, 1f);
            panelRect.pivot = new Vector2(1f, 1f);
            panelRect.anchoredPosition = new Vector2(-30f, -28f);
            panelRect.sizeDelta = new Vector2(480f, 130f);
            panel.GetComponent<Image>().color = new Color(0.01f, 0.025f, 0.04f, 0.94f);

            GameObject labelObject = new GameObject("MissionBriefingLabel", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(panel.transform, false);
            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(20f, 14f);
            labelRect.offsetMax = new Vector2(-20f, -14f);

            label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 18;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.UpperLeft;
            label.color = new Color(0.76f, 0.92f, 1f);
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            return panel;
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
