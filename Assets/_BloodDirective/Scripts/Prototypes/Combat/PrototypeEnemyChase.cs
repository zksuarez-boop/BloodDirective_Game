using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypeEnemyChase : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private LayerMask _blockerLayer;
        [SerializeField] private float _activationRadius = 4f;
        [SerializeField] private float _stopDistance = 1.25f;
        [SerializeField] private float _moveSpeed = 2.3f;
        [SerializeField] private float _turnSpeed = 420f;
        [SerializeField] private float _capsuleRadius = 0.38f;
        [SerializeField] private float _capsuleHeight = 1.8f;

        private readonly RaycastHit[] _blockerHits = new RaycastHit[4];
        private PrototypeHealth _health;
        private PrototypePlayerHealth _targetHealth;

        private void Awake()
        {
            _health = GetComponent<PrototypeHealth>();
            if (_target != null)
                _targetHealth = _target.GetComponent<PrototypePlayerHealth>();
        }

        private void Update()
        {
            if (_target == null || (_health != null && _health.IsDefeated) || (_targetHealth != null && _targetHealth.IsDefeated))
                return;

            Vector3 toTarget = _target.position - transform.position;
            toTarget.y = 0f;
            float distanceSqr = toTarget.sqrMagnitude;
            if (distanceSqr > _activationRadius * _activationRadius || distanceSqr <= _stopDistance * _stopDistance)
                return;

            Vector3 direction = toTarget.normalized;
            RotateToward(direction);
            Move(direction);
        }

        private void RotateToward(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
        }

        private void Move(Vector3 direction)
        {
            float distance = _moveSpeed * Time.deltaTime;
            Vector3 bottom = transform.position + Vector3.up * _capsuleRadius;
            Vector3 top = transform.position + Vector3.up * Mathf.Max(_capsuleRadius, _capsuleHeight - _capsuleRadius);
            int hitCount = Physics.CapsuleCastNonAlloc(bottom, top, _capsuleRadius, direction, _blockerHits, distance, _blockerLayer, QueryTriggerInteraction.Ignore);
            if (hitCount > 0)
                return;

            transform.position += direction * distance;
        }
    }
}
