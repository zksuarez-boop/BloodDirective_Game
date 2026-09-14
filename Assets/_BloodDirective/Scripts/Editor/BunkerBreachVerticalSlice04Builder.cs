using System.Collections.Generic;
using BloodDirective.Prototypes.Combat;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    /// <summary>
    /// Replaces placeholder capsules with visual-only actor rigs while retaining their gameplay roots.
    /// </summary>
    public static class BunkerBreachVerticalSlice04Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice03.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice04.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string SourceRootName = "BunkerBreachVerticalSlice03_Root";
        private const string SliceRootName = "BunkerBreachVerticalSlice04_Root";
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

        [MenuItem("Blood Directive/Reboot/Build Vertical Slice 04 - Character and Combat Presentation")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                BunkerBreachVerticalSlice03Builder.BuildScene();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice04Builder] Vertical Slice 03 was not found or built.");
                return;
            }

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, SourceRootName);
            Transform player = FindTransform(scene, "PrototypePlayer");
            Transform breachGrey = FindTransform(scene, "AwareEnemy_Target");
            Transform extractionGuard = FindTransform(scene, "ExtractionGuard_Target");
            if (root == null || player == null || breachGrey == null || extractionGuard == null)
            {
                Debug.LogError("[BunkerBreachVerticalSlice04Builder] Required Vertical Slice 03 actors were not found.");
                return;
            }

            root.name = SliceRootName;
            HidePlaceholderRenderer(player);
            HidePlaceholderRenderer(breachGrey);
            HidePlaceholderRenderer(extractionGuard);

            Material operatorBody = CreateMaterial("Slice04_OperatorBody", new Color(0.12f, 0.22f, 0.16f), 0.4f, 0.25f);
            Material operatorHit = CreateEmissiveMaterial("Slice04_OperatorHit", new Color(1f, 0.34f, 0.08f), 1.7f);
            Material operatorDefeated = CreateMaterial("Slice04_OperatorDefeated", new Color(0.018f, 0.02f, 0.022f), 0.1f, 0.05f);
            Material weapon = CreateMaterial("Slice04_Weapon", new Color(0.09f, 0.12f, 0.13f), 0.82f, 0.4f);
            Material calibratedWeapon = CreateEmissiveMaterial("Slice04_CalibratedWeapon", new Color(0.12f, 0.9f, 1f), 2.4f);
            Material operatorAccent = CreateMaterial("Slice04_OperatorAccent", new Color(0.46f, 0.54f, 0.4f), 0.35f, 0.2f);
            Material greyBody = CreateMaterial("Slice04_GreyBody", new Color(0.43f, 0.48f, 0.46f), 0.2f, 0.3f);
            Material greyHit = CreateEmissiveMaterial("Slice04_GreyHit", new Color(1f, 0.14f, 0.05f), 1.8f);
            Material greyDefeated = CreateMaterial("Slice04_GreyDefeated", new Color(0.025f, 0.027f, 0.03f), 0.1f, 0.04f);
            Material greyEye = CreateEmissiveMaterial("Slice04_GreyEye", new Color(1f, 0.05f, 0.025f), 2.2f);

            BuildOperatorRig(player, operatorBody, operatorHit, operatorDefeated, weapon, calibratedWeapon, operatorAccent);
            BuildGreyRig(breachGrey, "BreachGrey", greyBody, greyHit, greyDefeated, greyEye);
            BuildGreyRig(extractionGuard, "ExtractionGuard", greyBody, greyHit, greyDefeated, greyEye);

            Selection.activeGameObject = root.gameObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void BuildOperatorRig(Transform actor, Material bodyMaterial, Material hitMaterial, Material defeatedMaterial, Material weaponMaterial, Material calibratedWeaponMaterial, Material accentMaterial)
        {
            GameObject rig = CreateRigRoot("OperatorRig_VisualOnly", actor);
            var bodyRenderers = new List<Renderer>();

            AddPart(rig.transform, "Torso", PrimitiveType.Cube, new Vector3(0f, 1.02f, 0f), Vector3.zero, new Vector3(0.62f, 0.72f, 0.34f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "Vest", PrimitiveType.Cube, new Vector3(0f, 1.12f, 0.2f), Vector3.zero, new Vector3(0.68f, 0.35f, 0.1f), accentMaterial, bodyRenderers);
            AddPart(rig.transform, "Head", PrimitiveType.Sphere, new Vector3(0f, 1.7f, 0.02f), Vector3.zero, new Vector3(0.37f, 0.4f, 0.37f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "Helmet", PrimitiveType.Cube, new Vector3(0f, 1.94f, -0.02f), Vector3.zero, new Vector3(0.42f, 0.12f, 0.4f), accentMaterial, bodyRenderers);
            AddPart(rig.transform, "LeftArm", PrimitiveType.Capsule, new Vector3(-0.44f, 1.03f, 0f), new Vector3(0f, 0f, -7f), new Vector3(0.16f, 0.35f, 0.16f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "RightArm", PrimitiveType.Capsule, new Vector3(0.44f, 1.03f, 0f), new Vector3(0f, 0f, 7f), new Vector3(0.16f, 0.35f, 0.16f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "LeftLeg", PrimitiveType.Capsule, new Vector3(-0.19f, 0.35f, 0f), Vector3.zero, new Vector3(0.17f, 0.35f, 0.17f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "RightLeg", PrimitiveType.Capsule, new Vector3(0.19f, 0.35f, 0f), Vector3.zero, new Vector3(0.17f, 0.35f, 0.17f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "Backpack", PrimitiveType.Cube, new Vector3(0f, 1.1f, -0.25f), Vector3.zero, new Vector3(0.44f, 0.48f, 0.18f), accentMaterial, bodyRenderers);
            Renderer weaponRenderer = AddPart(rig.transform, "AetherRifle", PrimitiveType.Cube, new Vector3(0.37f, 1.08f, 0.45f), new Vector3(0f, 0f, 10f), new Vector3(0.12f, 0.12f, 0.9f), weaponMaterial, null);

            ConfigurePresentation(rig, null, actor.GetComponent<PrototypePlayerHealth>(), actor.GetComponent<PrototypePlayerAttack>(), bodyRenderers, weaponRenderer, bodyMaterial, hitMaterial, defeatedMaterial, weaponMaterial, calibratedWeaponMaterial);
        }

        private static void BuildGreyRig(Transform actor, string rigName, Material bodyMaterial, Material hitMaterial, Material defeatedMaterial, Material eyeMaterial)
        {
            GameObject rig = CreateRigRoot(rigName + "Rig_VisualOnly", actor);
            var bodyRenderers = new List<Renderer>();

            AddPart(rig.transform, "Torso", PrimitiveType.Capsule, new Vector3(0f, 1.02f, 0f), Vector3.zero, new Vector3(0.36f, 0.5f, 0.29f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "Head", PrimitiveType.Sphere, new Vector3(0f, 1.7f, 0.05f), Vector3.zero, new Vector3(0.48f, 0.38f, 0.4f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "LeftArm", PrimitiveType.Capsule, new Vector3(-0.37f, 1.08f, 0f), new Vector3(0f, 0f, -12f), new Vector3(0.11f, 0.34f, 0.11f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "RightArm", PrimitiveType.Capsule, new Vector3(0.37f, 1.08f, 0f), new Vector3(0f, 0f, 12f), new Vector3(0.11f, 0.34f, 0.11f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "LeftLeg", PrimitiveType.Capsule, new Vector3(-0.16f, 0.36f, 0f), Vector3.zero, new Vector3(0.12f, 0.33f, 0.12f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "RightLeg", PrimitiveType.Capsule, new Vector3(0.16f, 0.36f, 0f), Vector3.zero, new Vector3(0.12f, 0.33f, 0.12f), bodyMaterial, bodyRenderers);
            AddPart(rig.transform, "LeftEye", PrimitiveType.Sphere, new Vector3(-0.16f, 1.73f, 0.36f), Vector3.zero, new Vector3(0.09f, 0.07f, 0.04f), eyeMaterial, null);
            AddPart(rig.transform, "RightEye", PrimitiveType.Sphere, new Vector3(0.16f, 1.73f, 0.36f), Vector3.zero, new Vector3(0.09f, 0.07f, 0.04f), eyeMaterial, null);

            ConfigurePresentation(rig, actor.GetComponent<PrototypeHealth>(), null, null, bodyRenderers, null, bodyMaterial, hitMaterial, defeatedMaterial, null, null);
        }

        private static GameObject CreateRigRoot(string name, Transform actor)
        {
            GameObject rig = new GameObject(name);
            rig.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            rig.transform.SetParent(actor, false);
            rig.transform.localPosition = Vector3.zero;
            rig.transform.localRotation = Quaternion.identity;
            return rig;
        }

        private static Renderer AddPart(Transform parent, string name, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale, Material material, List<Renderer> bodyRenderers)
        {
            GameObject part = GameObject.CreatePrimitive(primitiveType);
            part.name = name + "_VisualOnly";
            part.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localEulerAngles = localEulerAngles;
            part.transform.localScale = localScale;
            Renderer renderer = part.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            Object.DestroyImmediate(part.GetComponent<Collider>());
            bodyRenderers?.Add(renderer);
            return renderer;
        }

        private static void ConfigurePresentation(GameObject rig, PrototypeHealth enemyHealth, PrototypePlayerHealth playerHealth, PrototypePlayerAttack playerAttack, List<Renderer> bodyRenderers, Renderer weaponRenderer, Material defaultMaterial, Material hitMaterial, Material defeatedMaterial, Material weaponMaterial, Material calibratedWeaponMaterial)
        {
            VerticalSliceActorPresentation presentation = rig.AddComponent<VerticalSliceActorPresentation>();
            SerializedObject presentationSo = new SerializedObject(presentation);
            presentationSo.FindProperty("_enemyHealth").objectReferenceValue = enemyHealth;
            presentationSo.FindProperty("_playerHealth").objectReferenceValue = playerHealth;
            presentationSo.FindProperty("_playerAttack").objectReferenceValue = playerAttack;
            SerializedProperty renderers = presentationSo.FindProperty("_bodyRenderers");
            renderers.arraySize = bodyRenderers.Count;
            for (int index = 0; index < bodyRenderers.Count; index++)
                renderers.GetArrayElementAtIndex(index).objectReferenceValue = bodyRenderers[index];
            presentationSo.FindProperty("_weaponRenderer").objectReferenceValue = weaponRenderer;
            presentationSo.FindProperty("_defaultMaterial").objectReferenceValue = defaultMaterial;
            presentationSo.FindProperty("_hitMaterial").objectReferenceValue = hitMaterial;
            presentationSo.FindProperty("_defeatedMaterial").objectReferenceValue = defeatedMaterial;
            presentationSo.FindProperty("_weaponMaterial").objectReferenceValue = weaponMaterial;
            presentationSo.FindProperty("_calibratedWeaponMaterial").objectReferenceValue = calibratedWeaponMaterial;
            presentationSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void HidePlaceholderRenderer(Transform actor)
        {
            Renderer renderer = actor.GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = false;
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
            Material material = CreateMaterial(name, color, 0.25f, 0.38f);
            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * intensity);
            }

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
