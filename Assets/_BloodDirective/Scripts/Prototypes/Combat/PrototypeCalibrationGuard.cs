using System;
using BloodDirective.Prototypes.Interaction;
using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypeCalibrationGuard : MonoBehaviour
    {
        [SerializeField] private PrototypeScientistRescue _scientist;
        [SerializeField] private PrototypeHealth _health;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _dormantMaterial;
        [SerializeField] private Material _activatedMaterial;
        [SerializeField] private Behaviour _awareness;
        [SerializeField] private Behaviour _chase;
        [SerializeField] private Behaviour _contactDamage;

        private bool _isActivated;

        public bool IsActivated => _isActivated;
        public event Action Activated;

        private void Awake()
        {
            if (_health == null)
                _health = GetComponent<PrototypeHealth>();
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();

            if (_health != null)
                _health.SetDamageable(false);
            SetEnemyBehavior(false);
            ApplyMaterial(_dormantMaterial);
        }

        private void Update()
        {
            if (_isActivated || _scientist == null || !_scientist.IsCalibrated)
                return;

            _isActivated = true;
            if (_health != null)
                _health.SetDamageable(true);
            SetEnemyBehavior(true);
            ApplyMaterial(_activatedMaterial);
            Activated?.Invoke();
        }

        private void SetEnemyBehavior(bool isEnabled)
        {
            if (_awareness != null)
                _awareness.enabled = isEnabled;
            if (_chase != null)
                _chase.enabled = isEnabled;
            if (_contactDamage != null)
                _contactDamage.enabled = isEnabled;
        }

        private void ApplyMaterial(Material material)
        {
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
