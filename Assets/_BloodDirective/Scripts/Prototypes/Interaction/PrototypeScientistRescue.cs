using System;
using BloodDirective.Prototypes.Combat;
using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeScientistRescue : MonoBehaviour
    {
        [SerializeField] private PrototypePlayerHealth _playerHealth;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _waitingMaterial;
        [SerializeField] private Material _rescuedMaterial;
        [SerializeField] private Material _calibratedMaterial;
        [SerializeField] private PrototypeAetherWallet _wallet;
        [SerializeField] private PrototypePlayerAttack _playerAttack;
        [SerializeField] private int _calibrationCost = 1;
        [SerializeField] private int _damageBonus = 1;

        private bool _isRescued;
        private bool _isCalibrated;

        public bool IsRescued => _isRescued;
        public bool IsCalibrated => _isCalibrated;
        public event Action ScientistRescued;
        public event Action WeaponCalibrated;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();

            ApplyMaterial(_waitingMaterial);
        }

        public void TryRescue()
        {
            if (_playerHealth != null && _playerHealth.IsDefeated)
                return;

            if (_isRescued)
            {
                TryCalibrate();
                return;
            }

            _isRescued = true;
            ApplyMaterial(_rescuedMaterial);
            ScientistRescued?.Invoke();
        }

        private void TryCalibrate()
        {
            if (_isCalibrated || _wallet == null || _playerAttack == null)
                return;
            if (!_wallet.TrySpend(_calibrationCost))
                return;

            _isCalibrated = true;
            _playerAttack.ApplyAetherCalibration(_damageBonus);
            ApplyMaterial(_calibratedMaterial != null ? _calibratedMaterial : _rescuedMaterial);
            WeaponCalibrated?.Invoke();
        }

        private void ApplyMaterial(Material material)
        {
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
