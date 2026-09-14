using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Applies visual-only environment dressing to the validated Bunker Breach slice.
    /// Collision remains owned by the prior vertical slice scene.
    /// </summary>
    public static class BunkerBreachVerticalSlice03Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice02.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice03.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string SourceRootName = "BunkerBreachVerticalSlice02_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice03_Root";
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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 03 - Environment Presentation")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice02Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice03Builder] Vertical Slice 02 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice03Builder] Vertical Slice 02 root was not found.");
                return;
            }

            root.name = SliceRootName;
            BuildEnvironment(root);
            ApplyAtmosphere();

            Selection.activeGameObject = root.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildEnvironment(Transform root)
        {
            GameObject visuals = new GameObject("EnvironmentPresentation_VisualOnly");
            visuals.transform.SetParent(root, false);
            visuals.layer = LayerMask.NameToLayer(VisualOnlyLayer);

            Material floorPlate = CreateMaterial("Slice03_FloorPlate", new Color(0.045f, 0.075f, 0.095f), 0.7f, 0.28f);
            Material floorInset = CreateMaterial("Slice03_FloorInset", new Color(0.018f, 0.032f, 0.045f), 0.55f, 0.2f);
            Material concrete = CreateMaterial("Slice03_Concrete", new Color(0.16f, 0.18f, 0.18f), 0.15f, 0.18f);
            Material metal = CreateMaterial("Slice03_DarkMetal", new Color(0.035f, 0.055f, 0.065f), 0.82f, 0.36f);
            Material trim = CreateMaterial("Slice03_Trim", new Color(0.11f, 0.15f, 0.16f), 0.9f, 0.3f);
            Material hazard = CreateMaterial("Slice03_Hazard", new Color(0.92f, 0.31f, 0.08f), 0.35f, 0.25f);
            Material cyan = CreateEmissiveMaterial("Slice03_CyanLight", new Color(0.15f, 0.85f, 1f), 2.5f);
            Material red = CreateEmissiveMaterial("Slice03_RedAlarm", new Color(1f, 0.08f, 0.035f), 2.2f);

            BuildFloorModules(visuals.transform, floorPlate, floorInset);
            BuildWallCladding(visuals.transform, concrete, trim, hazard);
            BuildContainmentHardware(visuals.transform, metal, trim, cyan, red);
            BuildPracticalLighting(visuals.transform, cyan, red);
        }

        private static void BuildFloorModules(Transform parent, Material floorPlate, Material floorInset)
        {
            for (int z = -4; z <= 4; z += 2)
            {
                for (int x = -4; x <= 4; x += 2)
                {
                    CreateVisualCube($"MainFloorPlate_{x}_{z}", new Vector3(x, 0.115f, z), new Vector3(1.82f, 0.025f, 1.82f), floorPlate, parent);
                    CreateVisualCube($"MainFloorInset_{x}_{z}", new Vector3(x, 0.13f, z), new Vector3(1.48f, 0.018f, 1.48f), floorInset, parent);
                }
            }

            for (int z = 6; z <= 8; z += 2)
            {
                for (int x = -2; x <= 2; x += 2)
                {
                    CreateVisualCube($"AlcoveFloorPlate_{x}_{z}", new Vector3(x, 0.115f, z), new Vector3(1.82f, 0.025f, 1.82f), floorPlate, parent);
                    CreateVisualCube($"AlcoveFloorInset_{x}_{z}", new Vector3(x, 0.13f, z), new Vector3(1.48f, 0.018f, 1.48f), floorInset, parent);
                }
            }

            CreateVisualCube("ContainmentStrip_VisualOnly", new Vector3(0f, 0.14f, 7f), new Vector3(4.2f, 0.02f, 0.22f), floorPlate, parent);
        }

        private static void BuildWallCladding(Transform parent, Material concrete, Material trim, Material hazard)
        {
            for (int z = -3; z <= 3; z += 2)
            {
                CreateVisualCube($"WestWallPanel_{z}", new Vector3(-5.97f, 1.5f, z), new Vector3(0.045f, 2.35f, 1.65f), concrete, parent);
                CreateVisualCube($"EastWallPanel_{z}", new Vector3(5.97f, 1.5f, z), new Vector3(0.045f, 2.35f, 1.65f), concrete, parent);
                CreateVisualCube($"WestWallTrim_{z}", new Vector3(-5.92f, 2.52f, z), new Vector3(0.08f, 0.08f, 1.72f), trim, parent);
                CreateVisualCube($"EastWallTrim_{z}", new Vector3(5.92f, 2.52f, z), new Vector3(0.08f, 0.08f, 1.72f), trim, parent);
            }

            for (int x = -4; x <= 4; x += 2)
            {
                CreateVisualCube($"SouthWallPanel_{x}", new Vector3(x, 1.5f, -4.97f), new Vector3(1.65f, 2.35f, 0.045f), concrete, parent);
                CreateVisualCube($"SouthWallTrim_{x}", new Vector3(x, 2.52f, -4.92f), new Vector3(1.72f, 0.08f, 0.08f), trim, parent);
            }

            CreateVisualCube("NorthWallHazardLeft_VisualOnly", new Vector3(-4f, 1.3f, 4.97f), new Vector3(1.6f, 0.14f, 0.055f), hazard, parent);
            CreateVisualCube("NorthWallHazardRight_VisualOnly", new Vector3(4f, 1.3f, 4.97f), new Vector3(1.6f, 0.14f, 0.055f), hazard, parent);
            CreateVisualCube("AlcoveWestPanel_VisualOnly", new Vector3(-2.47f, 1.5f, 7f), new Vector3(0.045f, 2.35f, 3.25f), concrete, parent);
            CreateVisualCube("AlcoveEastPanel_VisualOnly", new Vector3(2.47f, 1.5f, 7f), new Vector3(0.045f, 2.35f, 3.25f), concrete, parent);
            CreateVisualCube("AlcoveNorthPanel_VisualOnly", new Vector3(0f, 1.5f, 8.97f), new Vector3(4.25f, 2.35f, 0.045f), concrete, parent);
        }

        private static void BuildContainmentHardware(Transform parent, Material metal, Material trim, Material cyan, Material red)
        {
            CreateVisualCube("WestCoverCasing_VisualOnly", new Vector3(-2.4f, 0.84f, 1.6f), new Vector3(1.58f, 1.68f, 1.38f), metal, parent);
            CreateVisualCube("EastCoverCasing_VisualOnly", new Vector3(3f, 0.84f, -1.2f), new Vector3(1.58f, 1.68f, 1.38f), metal, parent);
            CreateVisualCube("WestCoverBand_VisualOnly", new Vector3(-2.4f, 1.35f, 2.31f), new Vector3(1.62f, 0.13f, 0.055f), trim, parent);
            CreateVisualCube("EastCoverBand_VisualOnly", new Vector3(3f, 1.35f, -0.49f), new Vector3(1.62f, 0.13f, 0.055f), trim, parent);

            CreateVisualCube("ContainmentConsoleBase_VisualOnly", new Vector3(-1.95f, 0.72f, 7.95f), new Vector3(0.42f, 1.38f, 0.95f), metal, parent);
            CreateVisualCube("ContainmentConsoleScreen_VisualOnly", new Vector3(-1.72f, 1.22f, 7.95f), new Vector3(0.04f, 0.44f, 0.62f), cyan, parent);
            CreateVisualCube("ExtractionConsoleBase_VisualOnly", new Vector3(5.7f, 0.72f, 2.9f), new Vector3(0.28f, 1.38f, 0.78f), metal, parent);
            CreateVisualCube("ExtractionConsoleAlarm_VisualOnly", new Vector3(5.52f, 1.22f, 2.9f), new Vector3(0.04f, 0.24f, 0.42f), red, parent);

            CreateVisualCube("OverheadConduitWest_VisualOnly", new Vector3(-4.9f, 2.85f, 0f), new Vector3(0.14f, 0.14f, 8.7f), trim, parent);
            CreateVisualCube("OverheadConduitEast_VisualOnly", new Vector3(4.9f, 2.85f, 0f), new Vector3(0.14f, 0.14f, 8.7f), trim, parent);
            CreateVisualCube("AlcoveConduit_VisualOnly", new Vector3(0f, 2.85f, 8.55f), new Vector3(4.2f, 0.14f, 0.14f), trim, parent);
        }

        private static void BuildPracticalLighting(Transform parent, Material cyan, Material red)
        {
            CreateLightFixture(parent, "IntakeLight", new Vector3(0f, 3.05f, -2.5f), new Vector3(2.8f, 0.08f, 0.28f), cyan, new Color(0.33f, 0.82f, 1f), 1.5f, 6f);
            CreateLightFixture(parent, "BreachLight", new Vector3(0f, 3.05f, 1.2f), new Vector3(2.1f, 0.08f, 0.28f), cyan, new Color(0.25f, 0.74f, 1f), 1.2f, 5f);
            CreateLightFixture(parent, "ContainmentLight", new Vector3(0f, 3.02f, 7.1f), new Vector3(1.8f, 0.08f, 0.28f), cyan, new Color(0.2f, 0.9f, 1f), 1.3f, 5f);
            CreateLightFixture(parent, "AlarmWest", new Vector3(-5.72f, 2.2f, 2.8f), new Vector3(0.12f, 0.18f, 0.18f), red, new Color(1f, 0.08f, 0.04f), 1.4f, 3.5f);
            CreateLightFixture(parent, "AlarmEast", new Vector3(5.72f, 2.2f, 1.5f), new Vector3(0.12f, 0.18f, 0.18f), red, new Color(1f, 0.08f, 0.04f), 1.2f, 3f);
        }

        private static void CreateLightFixture(Transform parent, string name, Vector3 position, Vector3 scale, Material material, Color lightColor, float intensity, float range)
        {
            GameObject fixture = CreateVisualCube(name + "_VisualOnly", position, scale, material, parent);
            GameObject lightObject = new GameObject(name + "_Light");
            lightObject.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            lightObject.transform.SetParent(fixture.transform, false);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = lightColor;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
        }

        private static GameObject CreateVisualCube(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            cube.transform.SetParent(parent);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(cube.GetComponent<Collider>());
            return cube;
        }

        private static Material CreateMaterial(string name, Color color, float metallic, float smoothness)
        {
            string path = $"{MaterialPath}/{name}.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
                return material;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("Standard");

            material = new Material(shader) { name = name, color = color };
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", metallic);
            if (material.HasProperty("_Smoothness"))
                material.SetFloat("_Smoothness", smoothness);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static Material CreateEmissiveMaterial(string name, Color color, float intensity)
        {
            Material material = CreateMaterial(name, color, 0.2f, 0.4f);
            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * intensity);
            }

            return material;
        }

        private static void ApplyAtmosphere()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.018f, 0.028f, 0.04f);
            RenderSettings.fog = false;
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
