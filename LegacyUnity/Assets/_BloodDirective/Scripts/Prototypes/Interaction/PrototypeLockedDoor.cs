using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeLockedDoor : MonoBehaviour
    {
        [SerializeField] private Collider _blockingCollider;
        [SerializeField] private Renderer _doorRenderer;
        [SerializeField] private Material _openedMaterial;
        [SerializeField] private Vector3 _openedLocalOffset = new Vector3(0f, 2.4f, 0f);

        private Vector3 _closedLocalPosition;
        private bool _isOpen;

        private void Awake()
        {
            if (_blockingCollider == null)
                _blockingCollider = GetComponent<Collider>();
            if (_doorRenderer == null)
                _doorRenderer = GetComponent<Renderer>();

            _closedLocalPosition = transform.localPosition;
        }

        public void Open()
        {
            if (_isOpen)
                return;

            _isOpen = true;
            if (_blockingCollider != null)
                _blockingCollider.enabled = false;
            if (_doorRenderer != null && _openedMaterial != null)
                _doorRenderer.sharedMaterial = _openedMaterial;

            transform.localPosition = _closedLocalPosition + _openedLocalOffset;
        }
    }
}