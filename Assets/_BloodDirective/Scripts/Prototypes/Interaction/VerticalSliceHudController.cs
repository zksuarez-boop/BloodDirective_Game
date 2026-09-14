using BloodDirective.Prototypes.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class VerticalSliceHudController : MonoBehaviour
    {
        [SerializeField] private PrototypePlayerHealth _playerHealth;
        [SerializeField] private PrototypePlayerAttack _playerAttack;
        [SerializeField] private PrototypeAetherWallet _wallet;
        [SerializeField] private PrototypeScientistRescue _scientist;
        [SerializeField] private PrototypeHealth _breachGrey;
        [SerializeField] private PrototypeAetherPickup _aether;
        [SerializeField] private PrototypeHealth _extractionGuard;
        [SerializeField] private PrototypeExtractionZone _extraction;
        [SerializeField] private Text _vitalityLabel;
        [SerializeField] private Text _aetherLabel;
        [SerializeField] private Text _weaponLabel;
        [SerializeField] private Text _instructionLabel;

        private void Update()
        {
            RefreshVitality();
            RefreshAether();
            RefreshWeapon();
            RefreshInstruction();
        }

        private void RefreshVitality()
        {
            if (_vitalityLabel == null || _playerHealth == null)
                return;

            _vitalityLabel.text = _playerHealth.IsDefeated
                ? "VITALITY // CRITICAL"
                : $"VITALITY // {_playerHealth.CurrentHealth:00} / {_playerHealth.MaxHealth:00}";
            _vitalityLabel.color = _playerHealth.IsDefeated ? new Color(1f, 0.25f, 0.2f) : new Color(0.74f, 0.94f, 1f);
        }

        private void RefreshAether()
        {
            if (_aetherLabel == null)
                return;

            int amount = _wallet != null ? _wallet.Amount : 0;
            _aetherLabel.text = $"AETHER // {amount:00}";
            _aetherLabel.color = amount > 0 ? new Color(0.2f, 0.92f, 1f) : new Color(0.38f, 0.52f, 0.62f);
        }

        private void RefreshWeapon()
        {
            if (_weaponLabel == null || _playerAttack == null)
                return;

            bool calibrated = _scientist != null && _scientist.IsCalibrated;
            _weaponLabel.text = calibrated
                ? $"WEAPON // AETHER CALIBRATED // DMG {_playerAttack.AttackDamage:00}"
                : $"WEAPON // STANDARD // DMG {_playerAttack.AttackDamage:00}";
            _weaponLabel.color = calibrated ? new Color(0.32f, 1f, 0.62f) : new Color(0.92f, 0.72f, 0.3f);
        }

        private void RefreshInstruction()
        {
            if (_instructionLabel == null)
                return;

            if (_extraction != null && _extraction.IsComplete)
            {
                SetInstruction("EXTRACTION CONFIRMED", new Color(0.72f, 1f, 0.35f));
                return;
            }
            if (_scientist != null && !_scientist.IsRescued)
            {
                SetInstruction("CLICK SCIENTIST TO RESCUE", new Color(0.84f, 0.92f, 1f));
                return;
            }
            if (_breachGrey != null && !_breachGrey.IsDefeated)
            {
                SetInstruction("CLICK GREY OR PRESS SPACE TO ATTACK", new Color(1f, 0.72f, 0.28f));
                return;
            }
            if (_aether != null && !_aether.IsCollected)
            {
                SetInstruction("CLICK THE CYAN AETHER SAMPLE", new Color(0.2f, 0.92f, 1f));
                return;
            }
            if (_scientist != null && !_scientist.IsCalibrated)
            {
                SetInstruction("RETURN TO SCIENTIST FOR CALIBRATION", new Color(0.35f, 1f, 0.55f));
                return;
            }
            if (_extractionGuard != null && !_extractionGuard.IsDefeated)
            {
                SetInstruction("NEUTRALIZE EXTRACTION GUARD", new Color(1f, 0.4f, 0.26f));
                return;
            }

            SetInstruction("MOVE TO THE EXTRACTION PAD", new Color(0.72f, 1f, 0.35f));
        }

        private void SetInstruction(string message, Color color)
        {
            _instructionLabel.text = message;
            _instructionLabel.color = color;
        }
    }
}
