using UnityEngine;

namespace BloodDirective.Prototypes.Movement
{
    /// <summary>
    /// Stable prototype camera: fixed isometric angle with light smoothing and no collision logic yet.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrototypeIsometricCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset = new Vector3(-8f, 11f, -8f);
        [SerializeField] private float _followSmoothTime = 0.08f;
        [SerializeField] private float _lookAhead = 1.75f;

        private Vector3 _velocity;

        public void SetTarget(Transform target)
        {
            _target = target;
            SnapToTarget();
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            Vector3 desiredPosition = _target.position + _offset;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref _velocity,
                _followSmoothTime);

            Vector3 lookTarget = _target.position + Vector3.up * _lookAhead;
            transform.rotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
        }

        private void SnapToTarget()
        {
            if (_target == null)
                return;

            transform.position = _target.position + _offset;
            Vector3 lookTarget = _target.position + Vector3.up * _lookAhead;
            transform.rotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
            _velocity = Vector3.zero;
        }
    }
}
