using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Builds a larger connected bunker route to validate map-scale traversal from the passed Act 1 mission scene.
    /// </summary>
    public static class LevelBlockoutPrototype01Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeMission01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Prototypes";
        private const string ScenePath = SceneDirectory + "/LevelBlockoutPrototype01.unity";
        private const string SourceRootName = "Act1_BunkerIntakeMission01_Root";
        private const string PrototypeRootName = "LevelBlockoutPrototype01_Root";
        private const string ExpansionRootName = "LevelBlockout01_Expansion";
        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string VisualOnlyLayer = "VisualOnly";
        private const string MaterialDirectory = "Assets/_BloodDirective/Art/Materials/Act1Bunker";

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

        [MenuItem("Blood Directive/Reboot/Build Level Blockout Prototype 01")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                Act1BunkerIntakeMissionBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[LevelBlockoutPrototype01Builder] Act 1 Production 04 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            if (root == null)
            {
                Debug.LogError("[LevelBlockoutPrototype01Builder] Act 1 Production 04 root was not found.");
                return;
            }

            root.name = PrototypeRootName;
            Transform existingExpansion = root.Find(ExpansionRootName);
            if (existingExpansion != null)
                Object.DestroyImmediate(existingExpansion.gameObject);

            OpenEastServiceDoorway(root);
            BuildExpansion(root);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void OpenEastServiceDoorway(Transform root)
        {
            DestroyNamed(root, "EastWall_Blocker");
            DestroyNamed(root, "EastWallPanel_1");
            DestroyNamed(root, "EastWallPanel_3");
            DestroyNamed(root, "EastWallTrim_1");
            DestroyNamed(root, "EastWallTrim_3");

            Material concrete = LoadMaterial("Act1Bunker_Concrete.mat");
            Material trim = LoadMaterial("Act1Bunker_Trim.mat");
            CreateBlockerWithVisual(root, "IntakeEastWallSouth", new Vector3(6.3f, 1.4f, -2.15f), new Vector3(0.6f, 2.8f, 6.3f), concrete);
            CreateBlockerWithVisual(root, "IntakeEastWallNorth", new Vector3(6.3f, 1.4f, 4.65f), new Vector3(0.6f, 2.8f, 1.3f), concrete);
            CreateVisualCube(root, "EastServiceDoorHeader_VisualOnly", new Vector3(6.3f, 2.72f, 2.05f), new Vector3(0.7f, 0.14f, 3.85f), trim);
        }

        private static void BuildExpansion(Transform root)
        {
            GameObject expansion = new GameObject(ExpansionRootName);
            expansion.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            expansion.transform.SetParent(root, false);

            Material floor = LoadMaterial("Act1Bunker_FloorPlate.mat");
            Material inset = LoadMaterial("Act1Bunker_FloorInset.mat");
            Material concrete = LoadMaterial("Act1Bunker_Concrete.mat");
            Material trim = LoadMaterial("Act1Bunker_Trim.mat");
            Material cyan = LoadMaterial("Act1Bunker_Cyan.mat");
            Material warning = LoadMaterial("Act1Bunker_Warning.mat");

            CreateRoomFloor(expansion.transform, "EastServiceCorridor", new Vector3(9f, 0f, 2f), new Vector3(6f, 0.2f, 4f), floor, inset);
            CreateRoomFloor(expansion.transform, "LogisticsBay", new Vector3(15f, 0f, 2f), new Vector3(6f, 0.2f, 10f), floor, inset);
            CreateRoomFloor(expansion.transform, "RelayCorridor", new Vector3(15f, 0f, 9.5f), new Vector3(4f, 0.2f, 5f), floor, inset);
            CreateRoomFloor(expansion.transform, "RelayChamber", new Vector3(15f, 0f, 15f), new Vector3(10f, 0.2f, 6f), floor, inset);

            BuildOuterBlockers(expansion.transform, concrete);
            BuildBlockoutLandmarks(expansion.transform, trim, cyan, warning);
        }

        private static void BuildOuterBlockers(Transform parent, Material concrete)
        {
            CreateBlockerWithVisual(parent, "ServiceCorridorSouthWall", new Vector3(9f, 1.4f, -0.3f), new Vector3(6.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "ServiceCorridorNorthWall", new Vector3(9f, 1.4f, 4.3f), new Vector3(6.6f, 2.8f, 0.6f), concrete);

            CreateBlockerWithVisual(parent, "LogisticsEastWall", new Vector3(18.3f, 1.4f, 2f), new Vector3(0.6f, 2.8f, 10.6f), concrete);
            CreateBlockerWithVisual(parent, "LogisticsSouthWall", new Vector3(15f, 1.4f, -3.3f), new Vector3(6.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "LogisticsNorthWallLeft", new Vector3(11.75f, 1.4f, 7.3f), new Vector3(1.5f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "LogisticsNorthWallRight", new Vector3(18.25f, 1.4f, 7.3f), new Vector3(1.5f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "LogisticsWestWallSouth", new Vector3(11.7f, 1.4f, -1.6f), new Vector3(0.6f, 2.8f, 2.8f), concrete);
            CreateBlockerWithVisual(parent, "LogisticsWestWallNorth", new Vector3(11.7f, 1.4f, 5.6f), new Vector3(0.6f, 2.8f, 2.8f), concrete);

            CreateBlockerWithVisual(parent, "RelayCorridorWestWall", new Vector3(12.7f, 1.4f, 9.5f), new Vector3(0.6f, 2.8f, 5.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayCorridorEastWall", new Vector3(17.3f, 1.4f, 9.5f), new Vector3(0.6f, 2.8f, 5.6f), concrete);

            CreateBlockerWithVisual(parent, "RelayChamberWestWall", new Vector3(9.7f, 1.4f, 15f), new Vector3(0.6f, 2.8f, 6.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayChamberEastWall", new Vector3(20.3f, 1.4f, 15f), new Vector3(0.6f, 2.8f, 6.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayChamberNorthWall", new Vector3(15f, 1.4f, 18.3f), new Vector3(10.6f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayChamberSouthWallLeft", new Vector3(10.85f, 1.4f, 11.7f), new Vector3(2.3f, 2.8f, 0.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayChamberSouthWallRight", new Vector3(19.15f, 1.4f, 11.7f), new Vector3(2.3f, 2.8f, 0.6f), concrete);

            CreateBlockerWithVisual(parent, "LogisticsCrateBlocker", new Vector3(15.6f, 0.85f, 1.4f), new Vector3(1.8f, 1.7f, 1.6f), concrete);
            CreateBlockerWithVisual(parent, "RelayCoreBlocker", new Vector3(15f, 1.15f, 15f), new Vector3(2.2f, 2.3f, 2.2f), concrete);
        }

        private static void BuildBlockoutLandmarks(Transform parent, Material trim, Material cyan, Material warning)
        {
            CreateVisualCube(parent, "ServiceConduit_VisualOnly", new Vector3(9f, 2.78f, 2f), new Vector3(0.16f, 0.16f, 4.1f), trim);
            CreateVisualCube(parent, "LogisticsConduit_VisualOnly", new Vector3(15f, 2.78f, 2f), new Vector3(6.1f, 0.16f, 0.16f), trim);
            CreateVisualCube(parent, "RelayChamberConduit_VisualOnly", new Vector3(15f, 2.78f, 15f), new Vector3(10.1f, 0.16f, 0.16f), trim);
            CreateVisualCube(parent, "LogisticsWarningStrip_VisualOnly", new Vector3(15f, 0.145f, -2.8f), new Vector3(5.6f, 0.02f, 0.16f), warning);
            CreateVisualCube(parent, "RelayCoreAccent_VisualOnly", new Vector3(15f, 2.35f, 15f), new Vector3(1.3f, 0.12f, 1.3f), cyan);

            CreateLight(parent, "ServiceLight", new Vector3(9f, 2.75f, 2f), cyan, new Color(0.2f, 0.82f, 1f), 1.1f, 4.5f);
            CreateLight(parent, "LogisticsLight", new Vector3(15f, 2.75f, 2f), cyan, new Color(0.18f, 0.72f, 1f), 1.2f, 5f);
            CreateLight(parent, "RelayLight", new Vector3(15f, 2.75f, 9.5f), cyan, new Color(0.2f, 0.82f, 1f), 1.1f, 4.5f);
            CreateLight(parent, "RelayCoreLight", new Vector3(15f, 2.65f, 15f), cyan, new Color(0.18f, 0.95f, 1f), 1.45f, 5.5f);
        }

        private static void CreateRoomFloor(Transform parent, string name, Vector3 position, Vector3 size, Material floor, Material inset)
        {
            CreateWalkableVolume(parent, name + "_Walkable", position, size);
            CreateVisualCube(parent, name + "_FloorPlate_VisualOnly", new Vector3(position.x, 0.115f, position.z), new Vector3(size.x, 0.025f, size.z), floor);
            CreateVisualCube(parent, name + "_FloorInset_VisualOnly", new Vector3(position.x, 0.13f, position.z), new Vector3(size.x - 0.38f, 0.018f, size.z - 0.38f), inset);
        }

        private static void CreateWalkableVolume(Transform parent, string name, Vector3 position, Vector3 size)
        {
            GameObject walkable = new GameObject(name);
            walkable.layer = LayerMask.NameToLayer(WalkableLayer);
            walkable.transform.SetParent(parent, false);
            walkable.transform.position = position;
            BoxCollider collider = walkable.AddComponent<BoxCollider>();
            collider.size = size;
        }

        private static void CreateBlockerWithVisual(Transform parent, string name, Vector3 position, Vector3 size, Material material)
        {
            GameObject blocker = new GameObject(name + "_Blocker");
            blocker.layer = LayerMask.NameToLayer(BlockerLayer);
            blocker.transform.SetParent(parent, false);
            blocker.transform.position = position;
            BoxCollider collider = blocker.AddComponent<BoxCollider>();
            collider.size = size;

            CreateVisualCube(parent, name + "_VisualOnly", position, size, material);
        }

        private static void CreateVisualCube(Transform parent, string name, Vector3 position, Vector3 size, Material material)
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = name;
            visual.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            visual.transform.SetParent(parent, false);
            visual.transform.position = position;
            visual.transform.localScale = size;
            visual.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(visual.GetComponent<Collider>());
        }

        private static void CreateLight(Transform parent, string name, Vector3 position, Material housingMaterial, Color color, float intensity, float range)
        {
            CreateVisualCube(parent, name + "Housing_VisualOnly", position, new Vector3(1.8f, 0.08f, 0.24f), housingMaterial);
            GameObject lightObject = new GameObject(name + "_VisualOnly");
            lightObject.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.position = position;
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
        }

        private static Material LoadMaterial(string fileName)
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialDirectory + "/" + fileName);
            if (material == null)
                throw new System.InvalidOperationException("Missing required Act 1 bunker material: " + fileName);

            return material;
        }

        private static void DestroyNamed(Transform root, string objectName)
        {
            Transform target = FindTransform(root, objectName);
            if (target != null)
                Object.DestroyImmediate(target.gameObject);
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
