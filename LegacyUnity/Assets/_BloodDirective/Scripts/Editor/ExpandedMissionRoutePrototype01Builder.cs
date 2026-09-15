using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Stages the proven Bunker Intake mission across the larger validated five-space blockout.
    /// </summary>
    public static class ExpandedMissionRoutePrototype01Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeExpansion01.unity";
        private const string SceneDirectory = "Assets/_BloodDirective/Scenes/Prototypes";
        private const string ScenePath = SceneDirectory + "/ExpandedMissionRoutePrototype01.unity";
        private const string SourceRootName = "Act1_BunkerIntakeExpansion01_Root";
        private const string PrototypeRootName = "ExpandedMissionRoutePrototype01_Root";
        private const string RouteMarkerRootName = "ExpandedMissionRoute01_Markers";
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

        [MenuItem("Blood Directive/Reboot/Build Expanded Mission Route Prototype 01")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                Act1BunkerIntakeExpansionBuilder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[ExpandedMissionRoutePrototype01Builder] Act 1 Production 05 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            Transform breachGrey = FindTransform(scene, "AwareEnemy_Target");
            Transform scientist = FindTransform(scene, "ScientistRescue_Interactable");
            Transform aether = FindTransform(scene, "AetherSample_Interactable");
            if (root == null || breachGrey == null || scientist == null || aether == null)
            {
                Debug.LogError("[ExpandedMissionRoutePrototype01Builder] Required Bunker Intake mission objects were not found.");
                return;
            }

            root.name = PrototypeRootName;
            Transform existingMarkers = root.Find(RouteMarkerRootName);
            if (existingMarkers != null)
                Object.DestroyImmediate(existingMarkers.gameObject);

            SetPosition(breachGrey, 9f, 2f);
            SetPosition(scientist, 14.2f, 4.8f);
            SetPosition(aether, 18f, 15f);
            CreateRouteMarkers(root);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void SetPosition(Transform target, float x, float z)
        {
            Vector3 position = target.position;
            target.position = new Vector3(x, position.y, z);
        }

        private static void CreateRouteMarkers(Transform root)
        {
            GameObject markerRoot = new GameObject(RouteMarkerRootName);
            markerRoot.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            markerRoot.transform.SetParent(root, false);

            CreateMarker(markerRoot.transform, "ServiceCorridor_BreachGreyMarker", new Vector3(9f, 0f, 2f));
            CreateMarker(markerRoot.transform, "LogisticsBay_ScientistMarker", new Vector3(14.2f, 0f, 4.8f));
            CreateMarker(markerRoot.transform, "RelayChamber_AetherMarker", new Vector3(18f, 0f, 15f));
            CreateMarker(markerRoot.transform, "ContainmentAnnex_CalibrationReturnMarker", new Vector3(14.2f, 0f, 4.8f));
            CreateMarker(markerRoot.transform, "ExtractionPassage_GuardAndPadMarker", new Vector3(0f, 0f, 11.4f));
        }

        private static void CreateMarker(Transform parent, string name, Vector3 position)
        {
            GameObject marker = new GameObject(name);
            marker.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            marker.transform.SetParent(parent, false);
            marker.transform.position = position;
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
