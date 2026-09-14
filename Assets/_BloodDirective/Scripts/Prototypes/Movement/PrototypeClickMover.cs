using UnityEngine;
using UnityEngine.InputSystem;

namespace BloodDirective.Prototypes.Movement
{
    /// <summary>
    /// Minimal isometric ARPG click movement for the first reboot prototype.
    /// Visual art is intentionally ignored; only Walkable receives clicks and Blocker stops movement.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrototypeClickMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _walkableLayer;
        [SerializeField] private LayerMask _blockerLayer;
        [SerializeField] private LayerMask _clickBlockerLayer;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 540f;
        [SerializeField] private float _stoppingDistance = 0.15f;
        [SerializeField] private float _capsuleRadius = 0.35f;
        [SerializeField] private float _capsuleHeight = 1.8f;

        private readonly RaycastHit[] _blockerHits = new RaycastHit[8];
        private Vector3 _destination;
        private bool _hasDestination;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;

            _destination = transform.position;
        }

        private void Update()
        {
            HandleClickInput();
            MoveTowardDestination();
        }

        private void HandleClickInput()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;
            if (_camera == null)
                return;

            Vector2 cursor = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(cursor);

            if (_clickBlockerLayer.value != 0 && Physics.Raycast(ray, 500f, _clickBlockerLayer, QueryTriggerInteraction.Ignore))
                return;

            if (!Physics.Raycast(ray, out RaycastHit hit, 500f, _walkableLayer, QueryTriggerInteraction.Ignore))
                return;

            _destination = hit.point;
            _destination.y = transform.position.y;
            _hasDestination = true;
        }

        private void MoveTowardDestination()
        {
            if (!_hasDestination)
                return;

            Vector3 toTarget = _destination - transform.position;
            toTarget.y = 0f;

            if (toTarget.magnitude <= _stoppingDistance)
            {
                _hasDestination = false;
                return;
            }

            Vector3 direction = toTarget.normalized;
            float stepDistance = Mathf.Min(_moveSpeed * Time.deltaTime, toTarget.magnitude);

            RotateToward(direction);

            if (IsBlocked(direction, stepDistance))
            {
                _hasDestination = false;
                return;
            }

            transform.position += direction * stepDistance;
        }

        private void RotateToward(Vector3 direction)
        {
            if (direction.sqrMagnitude <= 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime);
        }

        private bool IsBlocked(Vector3 direction, float distance)
        {
            Vector3 center = transform.position;
            Vector3 bottom = center + Vector3.up * _capsuleRadius;
            Vector3 top = center + Vector3.up * Mathf.Max(_capsuleRadius, _capsuleHeight - _capsuleRadius);

            int hitCount = Physics.CapsuleCastNonAlloc(
                bottom,
                top,
                _capsuleRadius,
                direction,
                _blockerHits,
                distance + 0.03f,
                _blockerLayer,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = _blockerHits[i].collider;
                if (hitCollider != null && !hitCollider.transform.IsChildOf(transform))
                    return true;
            }

            return false;
        }
    }
}
