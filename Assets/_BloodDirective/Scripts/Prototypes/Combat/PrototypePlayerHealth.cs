using System;
using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypePlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _healthyMaterial;
        [SerializeField] private Material _damagedMaterial;
        [SerializeField] private Material _defeatedMaterial;

        private int _currentHealth;
        private bool _isDefeated;

        public bool IsDefeated => _isDefeated;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public float NormalizedHealth => _maxHealth <= 0 ? 0f : (float)_currentHealth / _maxHealth;
        public event Action Damaged;
        public event Action Defeated;

        private void Awake()
        {
            _maxHealth = Mathf.Max(1, _maxHealth);
            _currentHealth = _maxHealth;
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();

            ApplyMaterial(_healthyMaterial);
        }

        public void TakeDamage(int amount)
        {
            if (_isDefeated)
                return;

            _currentHealth = Mathf.Max(0, _currentHealth - Mathf.Max(1, amount));
            Damaged?.Invoke();
            if (_currentHealth == 0)
            {
                _isDefeated = true;
                ApplyMaterial(_defeatedMaterial);
                Defeated?.Invoke();
                return;
            }

            ApplyMaterial(_damagedMaterial);
        }

        private void ApplyMaterial(Material material)
        {
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
