using System;
using BloodDirective.Prototypes.Combat;
using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeExtractionZone : MonoBehaviour
    {
        [SerializeField] private PrototypeAetherPickup _requiredAether;
        [SerializeField] private PrototypeScientistRescue _requiredScientist;
        [SerializeField] private PrototypeHealth _requiredGuard;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _readyMaterial;
        [SerializeField] private Material _completedMaterial;

        private bool _isComplete;

        public bool IsComplete => _isComplete;
        public event Action ExtractionCompleted;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isComplete || !other.CompareTag("Player"))
                return;
            if (_requiredAether == null || !_requiredAether.IsCollected)
                return;
            if (_requiredScientist != null && !_requiredScientist.IsCalibrated)
                return;
            if (_requiredGuard != null && !_requiredGuard.IsDefeated)
                return;

            PrototypePlayerHealth health = other.GetComponent<PrototypePlayerHealth>();
            if (health != null && health.IsDefeated)
                return;

            _isComplete = true;
            if (_renderer != null && _completedMaterial != null)
                _renderer.sharedMaterial = _completedMaterial;
            ExtractionCompleted?.Invoke();
        }

        public void MarkReady()
        {
            if (_isComplete || _renderer == null || _readyMaterial == null)
                return;

            _renderer.sharedMaterial = _readyMaterial;
        }
    }
}
