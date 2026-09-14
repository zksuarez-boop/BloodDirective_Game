using UnityEngine;
using UnityEngine.Events;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeInteractable : MonoBehaviour
    {
        [SerializeField] private string _displayName = "Interactable";
        [SerializeField] private Material _activatedMaterial;
        [SerializeField] private UnityEvent _activated = new UnityEvent();

        private Renderer _renderer;
        private bool _isActivated;

        public UnityEvent Activated => _activated;
        public string DisplayName => _displayName;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Activate()
        {
            if (_isActivated)
                return;

            _isActivated = true;
            if (_renderer != null && _activatedMaterial != null)
                _renderer.sharedMaterial = _activatedMaterial;

            _activated?.Invoke();
        }
    }
}
