using BloodDirective.Prototypes.Combat;
using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeMissionExitGate : MonoBehaviour
    {
        [SerializeField] private PrototypeScientistRescue _scientist;
        [SerializeField] private PrototypeHealth _requiredGuard;
        [SerializeField] private PrototypeLockedDoor _door;
        [SerializeField] private PrototypeExtractionZone _extraction;

        private bool _isAccessUnlocked;
        private bool _isExtractionReady;

        private void OnEnable()
        {
            if (_scientist != null)
                _scientist.WeaponCalibrated += EvaluateGate;
            if (_requiredGuard != null)
                _requiredGuard.Defeated += EvaluateGate;

            EvaluateGate();
        }

        private void OnDisable()
        {
            if (_scientist != null)
                _scientist.WeaponCalibrated -= EvaluateGate;
            if (_requiredGuard != null)
                _requiredGuard.Defeated -= EvaluateGate;
        }

        private void EvaluateGate()
        {
            if (_scientist == null)
                return;
            if (!_scientist.IsCalibrated)
                return;

            if (!_isAccessUnlocked)
            {
                _isAccessUnlocked = true;
                if (_door != null)
                    _door.Open();
            }

            if (_isExtractionReady || _requiredGuard == null || !_requiredGuard.IsDefeated)
                return;

            _isExtractionReady = true;
            if (_extraction != null)
                _extraction.MarkReady();
        }
    }
}
