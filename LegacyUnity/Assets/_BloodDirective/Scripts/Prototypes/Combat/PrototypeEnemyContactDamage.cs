using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypeEnemyContactDamage : MonoBehaviour
    {
        [SerializeField] private PrototypePlayerHealth _targetHealth;
        [SerializeField] private float _contactDistance = 1.35f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _damageInterval = 0.85f;

        private PrototypeHealth _health;
        private float _nextDamageTime;

        private void Awake()
        {
            _health = GetComponent<PrototypeHealth>();
        }

        private void Update()
        {
            if (_targetHealth == null || _targetHealth.IsDefeated || (_health != null && _health.IsDefeated))
                return;
            if (Time.time < _nextDamageTime)
                return;

            Vector3 offset = _targetHealth.transform.position - transform.position;
            offset.y = 0f;
            if (offset.sqrMagnitude > _contactDistance * _contactDistance)
                return;

            _nextDamageTime = Time.time + _damageInterval;
            _targetHealth.TakeDamage(_damage);
        }
    }
}
