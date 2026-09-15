using UnityEngine;
using UnityEngine.InputSystem;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeInteractionClicker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private float _maxDistance = 500f;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;
            if (_camera == null)
                return;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _interactableLayer, QueryTriggerInteraction.Ignore))
                return;

            PrototypeScientistRescue scientistRescue = hit.collider.GetComponentInParent<PrototypeScientistRescue>();
            if (scientistRescue != null)
            {
                scientistRescue.TryRescue();
                return;
            }

            PrototypeAetherPickup aetherPickup = hit.collider.GetComponentInParent<PrototypeAetherPickup>();
            if (aetherPickup != null)
            {
                aetherPickup.TryCollect();
                return;
            }

            PrototypeInteractable interactable = hit.collider.GetComponentInParent<PrototypeInteractable>();
            if (interactable != null)
                interactable.Activate();
        }
    }
}
