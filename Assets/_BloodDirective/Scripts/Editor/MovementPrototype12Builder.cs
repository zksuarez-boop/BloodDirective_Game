using BloodDirective.Prototypes.Combat;
using BloodDirective.Prototypes.Interaction;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodDirective.EditorTools
{
    public static class MovementPrototype12Builder
    {
        private const string BaseScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype11.unity";
        private const string ScenePath = "Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype12.unity";
        private const string MaterialPath = "Assets/_BloodDirective/Art/Materials/Prototype";
        private const string EnemyLayer = "Enemy";
        private const string BlockerLayer = "Blocker";
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

        [MenuItem("Blood Directive/Reboot/Build Movement Prototype 12")]
        public static void BuildScene()
        {
            if (Application.isPlaying)
                return;

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(BaseScenePath) == null)
                MovementPrototype11Builder.BuildScene();

            Scene scene = EditorSceneManager.OpenScene(BaseScenePath, OpenSceneMode.Single);
            Transform root = FindTransform(scene, "MovementPrototype11_Root");
            if (root == null)
                return;

            root.name = "MovementPrototype12_Root";
            Transform player = FindTransform(root, "PrototypePlayer");
            PrototypeScientistRescue scientist = FindTransform(root, "ScientistRescue_Interactable")?.GetComponent<PrototypeScientistRescue>();
            PrototypeAetherWallet wallet = root.GetComponent<PrototypeAetherWallet>();
            PrototypeObjectiveState objectiveState = root.GetComponent<PrototypeObjectiveState>();
            if (player == null || scientist == null || wallet == null || objectiveState == null)
                return;

            Material scientistCalibratedMaterial = CreateMaterial("Prototype12_ScientistCalibrated", new Color(0.18f, 0.92f, 0.9f));
            ConfigureScientist(scientist, wallet, player.GetComponent<PrototypePlayerAttack>(), scientistCalibratedMaterial);

            Material dormantMaterial = CreateMaterial("Prototype12_GuardDormant", new Color(0.12f, 0.13f, 0.15f));
            Material activeMaterial = CreateMaterial("Prototype12_GuardActive", new Color(0.64f, 0.1f, 0.12f));
            Material damagedMaterial = CreateMaterial("Prototype12_GuardDamaged", new Color(0.95f, 0.34f, 0.06f));
            Material defeatedMaterial = CreateMaterial("Prototype12_GuardDefeated", new Color(0.08f, 0.08f, 0.08f));
            Material facingMaterial = CreateMaterial("Prototype12_GuardFacing", new Color(1f, 0.12f, 0.05f));
            GameObject guard = CreateGuard(root, player, scientist, dormantMaterial, activeMaterial, damagedMaterial, defeatedMaterial, facingMaterial);

            SerializedObject objectiveSo = new SerializedObject(objectiveState);
            objectiveSo.FindProperty("_extractionGuard").objectReferenceValue = guard.GetComponent<PrototypeHealth>();
            objectiveSo.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = guard;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ConfigureScientist(PrototypeScientistRescue scientist, PrototypeAetherWallet wallet, PrototypePlayerAttack playerAttack, Material calibratedMaterial)
        {
            SerializedObject scientistSo = new SerializedObject(scientist);
            scientistSo.FindProperty("_calibratedMaterial").objectReferenceValue = calibratedMaterial;
            scientistSo.FindProperty("_wallet").objectReferenceValue = wallet;
            scientistSo.FindProperty("_playerAttack").objectReferenceValue = playerAttack;
            scientistSo.FindProperty("_calibrationCost").intValue = 1;
            scientistSo.FindProperty("_damageBonus").intValue = 1;
            scientistSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static GameObject CreateGuard(Transform root, Transform player, PrototypeScientistRescue scientist, Material dormantMaterial, Material activeMaterial, Material damagedMaterial, Material defeatedMaterial, Material facingMaterial)
        {
            GameObject guard = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            guard.name = "ExtractionGuard_Target";
            guard.layer = LayerMask.NameToLayer(EnemyLayer);
            guard.transform.SetParent(root);
            guard.transform.position = new Vector3(0f, 1f, 11.4f);
            guard.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            guard.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            guard.GetComponent<Renderer>().sharedMaterial = dormantMaterial;
            CreateFacingIndicator(guard.transform, facingMaterial);

            var health = guard.AddComponent<PrototypeHealth>();
            SerializedObject healthSo = new SerializedObject(health);
            healthSo.FindProperty("_maxHealth").intValue = 2;
            healthSo.FindProperty("_renderer").objectReferenceValue = guard.GetComponent<Renderer>();
            healthSo.FindProperty("_damagedMaterial").objectReferenceValue = damagedMaterial;
            healthSo.FindProperty("_defeatedMaterial").objectReferenceValue = defeatedMaterial;
            healthSo.ApplyModifiedPropertiesWithoutUndo();

            var awareness = guard.AddComponent<PrototypeEnemyAwareness>();
            SerializedObject awarenessSo = new SerializedObject(awareness);
            awarenessSo.FindProperty("_target").objectReferenceValue = player;
            awarenessSo.FindProperty("_renderer").objectReferenceValue = guard.GetComponent<Renderer>();
            awarenessSo.FindProperty("_idleMaterial").objectReferenceValue = activeMaterial;
            awarenessSo.FindProperty("_alertMaterial").objectReferenceValue = activeMaterial;
            awarenessSo.FindProperty("_awarenessRadius").floatValue = 4f;
            awarenessSo.FindProperty("_turnSpeed").floatValue = 420f;
            awarenessSo.ApplyModifiedPropertiesWithoutUndo();

            var chase = guard.AddComponent<PrototypeEnemyChase>();
            SerializedObject chaseSo = new SerializedObject(chase);
            chaseSo.FindProperty("_target").objectReferenceValue = player;
            chaseSo.FindProperty("_blockerLayer").intValue = 1 << LayerMask.NameToLayer(BlockerLayer);
            chaseSo.FindProperty("_activationRadius").floatValue = 4f;
            chaseSo.FindProperty("_stopDistance").floatValue = 1.25f;
            chaseSo.FindProperty("_moveSpeed").floatValue = 2.3f;
            chaseSo.ApplyModifiedPropertiesWithoutUndo();

            var contactDamage = guard.AddComponent<PrototypeEnemyContactDamage>();
            SerializedObject contactSo = new SerializedObject(contactDamage);
            contactSo.FindProperty("_targetHealth").objectReferenceValue = player.GetComponent<PrototypePlayerHealth>();
            contactSo.FindProperty("_contactDistance").floatValue = 1.35f;
            contactSo.FindProperty("_damage").intValue = 1;
            contactSo.FindProperty("_damageInterval").floatValue = 0.85f;
            contactSo.ApplyModifiedPropertiesWithoutUndo();

            var calibrationGuard = guard.AddComponent<PrototypeCalibrationGuard>();
            SerializedObject calibrationSo = new SerializedObject(calibrationGuard);
            calibrationSo.FindProperty("_scientist").objectReferenceValue = scientist;
            calibrationSo.FindProperty("_health").objectReferenceValue = health;
            calibrationSo.FindProperty("_renderer").objectReferenceValue = guard.GetComponent<Renderer>();
            calibrationSo.FindProperty("_dormantMaterial").objectReferenceValue = dormantMaterial;
            calibrationSo.FindProperty("_activatedMaterial").objectReferenceValue = activeMaterial;
            calibrationSo.FindProperty("_awareness").objectReferenceValue = awareness;
            calibrationSo.FindProperty("_chase").objectReferenceValue = chase;
            calibrationSo.FindProperty("_contactDamage").objectReferenceValue = contactDamage;
            calibrationSo.ApplyModifiedPropertiesWithoutUndo();

            return guard;
        }

        private static void CreateFacingIndicator(Transform enemy, Material facingMaterial)
        {
            GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            indicator.name = "ExtractionGuardFacing_VisualOnly";
            indicator.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            indicator.transform.SetParent(enemy, false);
            indicator.transform.localPosition = new Vector3(0f, 0.3f, 0.72f);
            indicator.transform.localScale = new Vector3(0.18f, 0.12f, 0.8f);
            indicator.GetComponent<Renderer>().sharedMaterial = facingMaterial;
            Object.DestroyImmediate(indicator.GetComponent<Collider>());
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
    }
}
