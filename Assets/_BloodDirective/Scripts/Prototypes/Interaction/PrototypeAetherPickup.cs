using System;
using BloodDirective.Prototypes.Combat;
using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeAetherPickup : MonoBehaviour
    {
        [SerializeField] private PrototypeHealth _requiredEnemy;
        [SerializeField] private PrototypeScientistRescue _requiredScientist;
        [SerializeField] private PrototypePlayerHealth _playerHealth;
        [SerializeField] private PrototypeAetherWallet _wallet;
        [SerializeField] private PrototypeLockedDoor _extractionDoor;
        [SerializeField] private bool _opensExtractionDoor = true;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _lockedMaterial;
        [SerializeField] private Material _availableMaterial;
        [SerializeField] private Material _collectedMaterial;

        private bool _isAvailable;
        private bool _isCollected;

        public bool IsCollected => _isCollected;
        public event Action AetherCollected;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();

            ApplyMaterial(_lockedMaterial);
        }

        private void Update()
        {
            if (_isAvailable || _isCollected)
                return;
            if (_requiredScientist != null && !_requiredScientist.IsRescued)
                return;
            if (_requiredEnemy != null && !_requiredEnemy.IsDefeated)
                return;

            _isAvailable = true;
            ApplyMaterial(_availableMaterial);
        }

        public void TryCollect()
        {
            if (_isCollected || !_isAvailable)
                return;
            if (_playerHealth != null && _playerHealth.IsDefeated)
                return;

            _isCollected = true;
            ApplyMaterial(_collectedMaterial);
            if (_wallet != null)
                _wallet.Add(1);
            if (_opensExtractionDoor && _extractionDoor != null)
                _extractionDoor.Open();
            AetherCollected?.Invoke();
        }

        private void ApplyMaterial(Material material)
        {
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
