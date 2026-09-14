using BloodDirective.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Creates the first production Act 1 scene and its original visual-only bunker kit.
    /// </summary>
    public static class Act1BunkerIntakeBuilder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice06.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Act1";
        private const string ScenePath = SceneDirectory + "/Act1_BunkerIntake.unity";
        private const string PrefabDirectory = "Assets/_BloodDirective/Prefabs/Environment/Act1Bunker";
        private const string MaterialDirectory = "Assets/_BloodDirective/Art/Materials/Act1Bunker";
        private const string MissionDirectory = "Assets/_BloodDirective/ScriptableObjects/Missions";
        private const string MissionPath = MissionDirectory + "/Act1_BunkerIntake.asset";
        private const string SourceRootName = "BunkerBreachVerticalSlice06_Root";
        private const string ProductionRootName = "Act1_BunkerIntake_Root";
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

        [MenuItem("Blood Directive/Reboot/Build Act 1 Production 01 - Bunker Intake")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice06Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[Act1BunkerIntakeBuilder] Vertical Slice 06 was not found or built.");
                return;
            }

            EnsureProjectFolders();
            BuildBunkerKit();
            CreateMissionDefinition();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[Act1BunkerIntakeBuilder] Vertical Slice 06 root was not found.");
                return;
            }

            root.name = ProductionRootName;
            AddProductionSceneMarker(root);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = root.gameObject;
        }

        private static void BuildBunkerKit()
        {
            Material floor = CreateMaterial("Act1Bunker_FloorPlate", new Color(0.045f, 0.07f, 0.085f), 0.72f, 0.25f);
            Material inset = CreateMaterial("Act1Bunker_FloorInset", new Color(0.015f, 0.026f, 0.035f), 0.48f, 0.18f);
            Material concrete = CreateMaterial("Act1Bunker_Concrete", new Color(0.17f, 0.19f, 0.19f), 0.12f, 0.15f);
            Material trim = CreateMaterial("Act1Bunker_Trim", new Color(0.055f, 0.08f, 0.09f), 0.82f, 0.3f);
            Material cyan = CreateEmissiveMaterial("Act1Bunker_Cyan", new Color(0.13f, 0.82f, 1f), 2.1f);
            Material warning = CreateEmissiveMaterial("Act1Bunker_Warning", new Color(1f, 0.2f, 0.05f), 1.8f);

            CreateFloorModulePrefab(floor, inset);
            CreateWallModulePrefab(concrete, trim, warning);
            CreateContainmentConsolePrefab(trim, cyan);
            CreatePipeModulePrefab(trim);
            CreateLightModulePrefab(cyan);
        }

        private static void CreateFloorModulePrefab(Material floor, Material inset)
        {
            const string path = PrefabDirectory + "/Act1Bunker_FloorModule.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = CreateVisualRoot("Act1Bunker_FloorModule");
            CreateVisualCube(root.transform, "FloorPlate", Vector3.zero, new Vector3(2f, 0.05f, 2f), floor);
            CreateVisualCube(root.transform, "FloorInset", new Vector3(0f, 0.035f, 0f), new Vector3(1.62f, 0.02f, 1.62f), inset);
            SavePrefab(root, path);
        }

        private static void CreateWallModulePrefab(Material concrete, Material trim, Material warning)
        {
            const string path = PrefabDirectory + "/Act1Bunker_WallModule.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = CreateVisualRoot("Act1Bunker_WallModule");
            CreateVisualCube(root.transform, "ConcretePanel", new Vector3(0f, 1.4f, 0f), new Vector3(2f, 2.8f, 0.1f), concrete);
            CreateVisualCube(root.transform, "TopTrim", new Vector3(0f, 2.6f, -0.06f), new Vector3(2.1f, 0.12f, 0.08f), trim);
            CreateVisualCube(root.transform, "HazardStripe", new Vector3(0f, 1.2f, -0.06f), new Vector3(1.5f, 0.12f, 0.08f), warning);
            SavePrefab(root, path);
        }

        private static void CreateContainmentConsolePrefab(Material trim, Material cyan)
        {
            const string path = PrefabDirectory + "/Act1Bunker_ContainmentConsole.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = CreateVisualRoot("Act1Bunker_ContainmentConsole");
            CreateVisualCube(root.transform, "ConsoleBase", new Vector3(0f, 0.68f, 0f), new Vector3(0.55f, 1.35f, 0.8f), trim);
            CreateVisualCube(root.transform, "ConsoleScreen", new Vector3(0f, 1.18f, 0.42f), new Vector3(0.42f, 0.38f, 0.06f), cyan);
            SavePrefab(root, path);
        }

        private static void CreatePipeModulePrefab(Material trim)
        {
            const string path = PrefabDirectory + "/Act1Bunker_PipeModule.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = CreateVisualRoot("Act1Bunker_PipeModule");
            CreateVisualCube(root.transform, "Pipe", Vector3.zero, new Vector3(0.14f, 0.14f, 4f), trim);
            CreateVisualCube(root.transform, "PipeClampBack", new Vector3(0f, 0f, -1.1f), new Vector3(0.24f, 0.24f, 0.1f), trim);
            CreateVisualCube(root.transform, "PipeClampFront", new Vector3(0f, 0f, 1.1f), new Vector3(0.24f, 0.24f, 0.1f), trim);
            SavePrefab(root, path);
        }

        private static void CreateLightModulePrefab(Material cyan)
        {
            const string path = PrefabDirectory + "/Act1Bunker_LightModule.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                return;

            GameObject root = CreateVisualRoot("Act1Bunker_LightModule");
            CreateVisualCube(root.transform, "LightHousing", Vector3.zero, new Vector3(2f, 0.1f, 0.3f), cyan);
            GameObject lightObject = new GameObject("PointLight_VisualOnly");
            lightObject.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            lightObject.transform.SetParent(root.transform, false);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(0.22f, 0.8f, 1f);
            light.intensity = 1.25f;
            light.range = 5f;
            light.shadows = LightShadows.None;
            SavePrefab(root, path);
        }

        private static void CreateMissionDefinition()
        {
            if (AssetDatabase.LoadAssetAtPath<MissionDefinition>(MissionPath) != null)
                return;

            MissionDefinition definition = ScriptableObject.CreateInstance<MissionDefinition>();
            definition.name = "Act1_BunkerIntake";
            AssetDatabase.CreateAsset(definition, MissionPath);

            SerializedObject definitionSo = new SerializedObject(definition);
            definitionSo.FindProperty("_missionId").stringValue = "act1_bunker_intake";
            definitionSo.FindProperty("_displayName").stringValue = "Bunker Intake";
            definitionSo.FindProperty("_briefing").stringValue = "A containment breach has cut the bunker intake off from extraction. Secure the scientist, recover Aether, calibrate the weapon, and clear the route out.";
            definitionSo.FindProperty("_primaryObjective").stringValue = "Reach extraction with the scientist's Aether calibration.";
            definitionSo.FindProperty("_biomeId").stringValue = "underground_bunker";
            definitionSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AddProductionSceneMarker(Transform root)
        {
            GameObject marker = new GameObject("Act1BunkerIntake_ProductionMarker");
            marker.transform.SetParent(root, false);
            marker.layer = LayerMask.NameToLayer(VisualOnlyLayer);
        }

        private static GameObject CreateVisualRoot(string name)
        {
            GameObject root = new GameObject(name);
            root.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            return root;
        }

        private static void CreateVisualCube(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name + "_VisualOnly";
            cube.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localScale = localScale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(cube.GetComponent<Collider>());
        }

        private static void SavePrefab(GameObject root, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
        }

        private static Material CreateMaterial(string name, Color color, float metallic, float smoothness)
        {
            string path = MaterialDirectory + "/" + name + ".mat";
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
            Material material = CreateMaterial(name, color, 0.2f, 0.34f);
            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * intensity);
            }

            return material;
        }

        private static void EnsureProjectFolders()
        {
            EnsureFolder("Assets/_BloodDirective/Scenes", "Act1");
            EnsureFolder("Assets/_BloodDirective", "Prefabs");
            EnsureFolder("Assets/_BloodDirective/Prefabs", "Environment");
            EnsureFolder("Assets/_BloodDirective/Prefabs/Environment", "Act1Bunker");
            EnsureFolder("Assets/_BloodDirective/Art", "Materials");
            EnsureFolder("Assets/_BloodDirective/Art/Materials", "Act1Bunker");
            EnsureFolder("Assets/_BloodDirective", "ScriptableObjects");
            EnsureFolder("Assets/_BloodDirective/ScriptableObjects", "Missions");
        }

        private static void EnsureFolder(string parentPath, string folderName)
        {
            string path = parentPath + "/" + folderName;
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parentPath, folderName);
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
