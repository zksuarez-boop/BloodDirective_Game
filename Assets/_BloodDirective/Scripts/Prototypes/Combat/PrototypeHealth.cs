using System;
using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypeHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _damagedMaterial;
        [SerializeField] private Material _defeatedMaterial;

        private int _currentHealth;
        private bool _isDefeated;
        private bool _canTakeDamage = true;

        public bool IsDefeated => _isDefeated;
        public bool CanTakeDamage => _canTakeDamage;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public float NormalizedHealth => _maxHealth <= 0 ? 0f : (float)_currentHealth / _maxHealth;
        public event Action Damaged;
        public event Action Defeated;

        private void Awake()
        {
            _currentHealth = Mathf.Max(1, _maxHealth);
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
        }

        public void TakeDamage(int amount)
        {
            if (_isDefeated || !_canTakeDamage)
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

        public void SetDamageable(bool canTakeDamage)
        {
            _canTakeDamage = canTakeDamage;
        }

        private void ApplyMaterial(Material material)
        {
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
