using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeExitZone : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _reachedMaterial;

        private bool _isReached;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isReached || !other.CompareTag("Player"))
                return;

            _isReached = true;
            if (_renderer != null && _reachedMaterial != null)
                _renderer.sharedMaterial = _reachedMaterial;
        }
    }
}