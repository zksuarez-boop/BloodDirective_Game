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
    /// Adds a visual-only feedback layer to the verified Bunker Breach gameplay scene.
    /// </summary>
    public static class BunkerBreachVerticalSlice02Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice01.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice02.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string SourceRootName = "BunkerBreachVerticalSlice01_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice02_Root";
        private const string VisualOnlyLayer = "VisualOnly";

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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 02 - Feedback and HUD")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice01Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice02Builder] Vertical Slice 01 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            Transform player = FindTransform(scene, "PrototypePlayer");
            PrototypeScientistRescue scientist = FindTransform(scene, "ScientistRescue_Interactable")?.GetComponent<PrototypeScientistRescue>();
            PrototypeHealth breachGrey = FindTransform(scene, "AwareEnemy_Target")?.GetComponent<PrototypeHealth>();
            PrototypeAetherPickup aether = FindTransform(scene, "AetherSample_Interactable")?.GetComponent<PrototypeAetherPickup>();
            PrototypeHealth extractionGuard = FindTransform(scene, "ExtractionGuard_Target")?.GetComponent<PrototypeHealth>();
            PrototypeExtractionZone extraction = FindTransform(scene, "ExtractionZone_Trigger")?.GetComponent<PrototypeExtractionZone>();
            if (root == null || player == null || scientist == null || breachGrey == null || aether == null || extractionGuard == null || extraction == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice02Builder] Required Vertical Slice 01 objects were not found.");
                return;
            }

            root.name = SliceRootName;
            Transform legacyCanvas = FindTransform(root, "PrototypeObjectiveCanvas");
            if (legacyCanvas != null)
                Object.DestroyImmediate(legacyCanvas.gameObject);

            PrototypePlayerHealth playerHealth = player.GetComponent<PrototypePlayerHealth>();
            PrototypePlayerAttack playerAttack = player.GetComponent<PrototypePlayerAttack>();
            PrototypeAetherWallet wallet = root.GetComponent<PrototypeAetherWallet>();
            PrototypeObjectiveState objectiveState = root.GetComponent<PrototypeObjectiveState>();
            if (playerHealth == null || playerAttack == null || wallet == null || objectiveState == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice02Builder] Required Vertical Slice 01 components were not found.");
                return;
            }

            BuildHud(root, objectiveState, playerHealth, playerAttack, wallet, scientist, breachGrey, aether, extractionGuard, extraction);
            AddWorldHealthBar(breachGrey.transform, "BreachGreyHealthBar");
            AddWorldHealthBar(extractionGuard.transform, "ExtractionGuardHealthBar");

            Selection.activeGameObject = root.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildHud(Transform root, PrototypeObjectiveState objectiveState, PrototypePlayerHealth playerHealth, PrototypePlayerAttack playerAttack, PrototypeAetherWallet wallet, PrototypeScientistRescue scientist, PrototypeHealth breachGrey, PrototypeAetherPickup aether, PrototypeHealth extractionGuard, PrototypeExtractionZone extraction)
        {
            GameObject canvasObject = new GameObject("VerticalSlice02_HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(root, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            Color panelColor = new Color(0.012f, 0.025f, 0.04f, 0.91f);
            Text vitality = CreatePanelText(canvasObject.transform, "VitalityPanel", new Vector2(28f, -28f), new Vector2(330f, 58f), TextAnchor.MiddleLeft, panelColor, new Color(0.74f, 0.94f, 1f), 20);
            Text aetherText = CreatePanelText(canvasObject.transform, "AetherPanel", new Vector2(28f, -96f), new Vector2(330f, 58f), TextAnchor.MiddleLeft, panelColor, new Color(0.38f, 0.52f, 0.62f), 20);
            Text weapon = CreatePanelText(canvasObject.transform, "WeaponPanel", new Vector2(28f, -164f), new Vector2(430f, 58f), TextAnchor.MiddleLeft, panelColor, new Color(0.92f, 0.72f, 0.3f), 18);
            Text objective = CreateCenteredText(canvasObject.transform, "ObjectivePanel", new Vector2(0f, -28f), new Vector2(620f, 58f), panelColor, 24);
            Text instruction = CreateBottomText(canvasObject.transform, "InstructionPanel", new Vector2(0f, 32f), new Vector2(660f, 52f), panelColor, 18);

            PrototypeObjectiveDisplay objectiveDisplay = canvasObject.AddComponent<PrototypeObjectiveDisplay>();
            SerializedObject objectiveDisplaySo = new SerializedObject(objectiveDisplay);
            objectiveDisplaySo.FindProperty("_label").objectReferenceValue = objective;
            objectiveDisplaySo.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject stateSo = new SerializedObject(objectiveState);
            stateSo.FindProperty("_display").objectReferenceValue = objectiveDisplay;
            stateSo.ApplyModifiedPropertiesWithoutUndo();

            VerticalSliceHudController hud = canvasObject.AddComponent<VerticalSliceHudController>();
            SerializedObject hudSo = new SerializedObject(hud);
            hudSo.FindProperty("_playerHealth").objectReferenceValue = playerHealth;
            hudSo.FindProperty("_playerAttack").objectReferenceValue = playerAttack;
            hudSo.FindProperty("_wallet").objectReferenceValue = wallet;
            hudSo.FindProperty("_scientist").objectReferenceValue = scientist;
            hudSo.FindProperty("_breachGrey").objectReferenceValue = breachGrey;
            hudSo.FindProperty("_aether").objectReferenceValue = aether;
            hudSo.FindProperty("_extractionGuard").objectReferenceValue = extractionGuard;
            hudSo.FindProperty("_extraction").objectReferenceValue = extraction;
            hudSo.FindProperty("_vitalityLabel").objectReferenceValue = vitality;
            hudSo.FindProperty("_aetherLabel").objectReferenceValue = aetherText;
            hudSo.FindProperty("_weaponLabel").objectReferenceValue = weapon;
            hudSo.FindProperty("_instructionLabel").objectReferenceValue = instruction;
            hudSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Text CreatePanelText(Transform parent, string name, Vector2 position, Vector2 size, TextAnchor alignment, Color panelColor, Color textColor, int fontSize)
        {
            GameObject panel = CreatePanel(parent, name, position, size, new Vector2(0f, 1f), new Vector2(0f, 1f), panelColor);
            return CreateLabel(panel.transform, name + "Label", alignment, textColor, fontSize, new Vector2(18f, 0f), new Vector2(-12f, 0f));
        }

        private static Text CreateCenteredText(Transform parent, string name, Vector2 position, Vector2 size, Color panelColor, int fontSize)
        {
            GameObject panel = CreatePanel(parent, name, position, size, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), panelColor);
            return CreateLabel(panel.transform, name + "Label", TextAnchor.MiddleCenter, new Color(0.9f, 0.94f, 1f), fontSize, new Vector2(12f, 0f), new Vector2(-12f, 0f));
        }

        private static Text CreateBottomText(Transform parent, string name, Vector2 position, Vector2 size, Color panelColor, int fontSize)
        {
            GameObject panel = CreatePanel(parent, name, position, size, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), panelColor);
            return CreateLabel(panel.transform, name + "Label", TextAnchor.MiddleCenter, new Color(0.84f, 0.92f, 1f), fontSize, Vector2.zero, Vector2.zero);
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 position, Vector2 size, Vector2 anchor, Vector2 pivot, Color color)
        {
            GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            panel.GetComponent<Image>().color = color;
            return panel;
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

        private static void AddWorldHealthBar(Transform enemy, string name)
        {
            GameObject container = new GameObject(name);
            container.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            container.transform.SetParent(enemy, false);

            GameObject background = GameObject.CreatePrimitive(PrimitiveType.Cube);
            background.name = "Background_VisualOnly";
            background.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            background.transform.SetParent(container.transform, false);
            background.transform.localScale = new Vector3(1.28f, 0.09f, 0.05f);
            background.GetComponent<Renderer>().sharedMaterial = CreateMaterial("VerticalSlice02_HealthBackground", new Color(0.02f, 0.02f, 0.025f));
            Object.DestroyImmediate(background.GetComponent<Collider>());

            GameObject fill = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fill.name = "Fill_VisualOnly";
            fill.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            fill.transform.SetParent(container.transform, false);
            fill.transform.localPosition = new Vector3(-0.04f, 0f, -0.03f);
            fill.transform.localScale = new Vector3(1.2f, 0.05f, 0.045f);
            fill.GetComponent<Renderer>().sharedMaterial = CreateMaterial("VerticalSlice02_HealthFill", new Color(1f, 0.22f, 0.12f));
            Object.DestroyImmediate(fill.GetComponent<Collider>());

            VerticalSliceWorldHealthBar healthBar = container.AddComponent<VerticalSliceWorldHealthBar>();
            SerializedObject healthBarSo = new SerializedObject(healthBar);
            healthBarSo.FindProperty("_health").objectReferenceValue = enemy.GetComponent<PrototypeHealth>();
            healthBarSo.FindProperty("_fill").objectReferenceValue = fill.transform;
            healthBarSo.ApplyModifiedPropertiesWithoutUndo();
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
