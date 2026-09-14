using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class VerticalSliceActorPresentation : MonoBehaviour
    {
        [SerializeField] private PrototypeHealth _enemyHealth;
        [SerializeField] private PrototypePlayerHealth _playerHealth;
        [SerializeField] private PrototypePlayerAttack _playerAttack;
        [SerializeField] private Renderer[] _bodyRenderers;
        [SerializeField] private Renderer _weaponRenderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hitMaterial;
        [SerializeField] private Material _defeatedMaterial;
        [SerializeField] private Material _weaponMaterial;
        [SerializeField] private Material _calibratedWeaponMaterial;
        [SerializeField] private float _hitFlashDuration = 0.14f;

        private float _hitFlashEndsAt;
        private bool _attackFlashActive;
        private Material _currentBodyMaterial;
        private Material _currentWeaponMaterial;

        private void OnEnable()
        {
            if (_enemyHealth != null)
            {
                _enemyHealth.Damaged += ShowHitFeedback;
                _enemyHealth.Defeated += ShowDefeatedFeedback;
            }
            if (_playerHealth != null)
            {
                _playerHealth.Damaged += ShowHitFeedback;
                _playerHealth.Defeated += ShowDefeatedFeedback;
            }
            if (_playerAttack != null)
            {
                _playerAttack.AttackPerformed += ShowAttackFeedback;
                _playerAttack.AetherCalibrated += RefreshWeaponMaterial;
            }

            RefreshPresentation();
        }

        private void OnDisable()
        {
            if (_enemyHealth != null)
            {
                _enemyHealth.Damaged -= ShowHitFeedback;
                _enemyHealth.Defeated -= ShowDefeatedFeedback;
            }
            if (_playerHealth != null)
            {
                _playerHealth.Damaged -= ShowHitFeedback;
                _playerHealth.Defeated -= ShowDefeatedFeedback;
            }
            if (_playerAttack != null)
            {
                _playerAttack.AttackPerformed -= ShowAttackFeedback;
                _playerAttack.AetherCalibrated -= RefreshWeaponMaterial;
            }
        }

        private void Update()
        {
            if (IsDefeated())
            {
                ApplyBodyMaterial(_defeatedMaterial);
                return;
            }

            if (Time.time >= _hitFlashEndsAt)
            {
                ApplyBodyMaterial(_defaultMaterial);
                if (_attackFlashActive)
                {
                    _attackFlashActive = false;
                    RefreshWeaponMaterial();
                }
            }
        }

        private void ShowHitFeedback()
        {
            _hitFlashEndsAt = Time.time + _hitFlashDuration;
            ApplyBodyMaterial(_hitMaterial);
        }

        private void ShowDefeatedFeedback()
        {
            ApplyBodyMaterial(_defeatedMaterial);
        }

        private void ShowAttackFeedback()
        {
            if (_weaponRenderer == null || _hitMaterial == null)
                return;

            _currentWeaponMaterial = _hitMaterial;
            _weaponRenderer.sharedMaterial = _hitMaterial;
            _attackFlashActive = true;
            _hitFlashEndsAt = Time.time + _hitFlashDuration;
        }

        private void RefreshPresentation()
        {
            ApplyBodyMaterial(_defaultMaterial);
            RefreshWeaponMaterial();
        }

        private void RefreshWeaponMaterial()
        {
            if (_weaponRenderer == null)
                return;

            bool calibrated = _playerAttack != null && _playerAttack.IsAetherCalibrated;
            Material nextMaterial = calibrated && _calibratedWeaponMaterial != null
                ? _calibratedWeaponMaterial
                : _weaponMaterial;
            if (_currentWeaponMaterial == nextMaterial)
                return;

            _currentWeaponMaterial = nextMaterial;
            _weaponRenderer.sharedMaterial = nextMaterial;
        }

        private bool IsDefeated()
        {
            return (_enemyHealth != null && _enemyHealth.IsDefeated)
                || (_playerHealth != null && _playerHealth.IsDefeated);
        }

        private void ApplyBodyMaterial(Material material)
        {
            if (material == null || _bodyRenderers == null || _currentBodyMaterial == material)
                return;

            _currentBodyMaterial = material;
            foreach (Renderer bodyRenderer in _bodyRenderers)
            {
                if (bodyRenderer != null)
                    bodyRenderer.sharedMaterial = material;
            }
        }
    }
}
