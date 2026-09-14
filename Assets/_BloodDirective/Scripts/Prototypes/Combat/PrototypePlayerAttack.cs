using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypePlayerAttack : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private int _attackDamage = 1;
        [SerializeField] private float _attackRange = 2.2f;
        [SerializeField] private float _attackCooldown = 0.35f;

        private readonly Collider[] _enemyHits = new Collider[12];
        private float _nextAttackTime;
        private PrototypePlayerHealth _playerHealth;
        private bool _isAetherCalibrated;

        public int AttackDamage => _attackDamage;
        public bool IsAetherCalibrated => _isAetherCalibrated;
        public event Action AttackPerformed;
        public event Action AetherCalibrated;

        private void Awake()
        {
            // Earlier combat prototypes intentionally have no player health component.
            _playerHealth = GetComponent<PrototypePlayerHealth>();
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            if (_playerHealth != null && _playerHealth.IsDefeated)
                return;

            TryClickAttack();
            TryKeyboardAttack();
        }

        private void TryClickAttack()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;
            if (_camera == null)
                return;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f, _enemyLayer, QueryTriggerInteraction.Ignore))
                return;

            Attack(hit.collider.GetComponentInParent<PrototypeHealth>());
        }

        private void TryKeyboardAttack()
        {
            if (Keyboard.current == null || !Keyboard.current.spaceKey.wasPressedThisFrame)
                return;

            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _enemyHits, _enemyLayer, QueryTriggerInteraction.Ignore);
            PrototypeHealth nearest = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                PrototypeHealth health = _enemyHits[i].GetComponentInParent<PrototypeHealth>();
                if (health == null || health.IsDefeated)
                    continue;

                float distance = Vector3.Distance(transform.position, health.transform.position);
                if (distance < nearestDistance)
                {
                    nearest = health;
                    nearestDistance = distance;
                }
            }

            Attack(nearest);
        }

        private void Attack(PrototypeHealth target)
        {
            if (_playerHealth != null && _playerHealth.IsDefeated)
                return;
            if (target == null || target.IsDefeated)
                return;
            if (Time.time < _nextAttackTime)
                return;
            if (Vector3.Distance(transform.position, target.transform.position) > _attackRange)
                return;

            _nextAttackTime = Time.time + _attackCooldown;
            target.TakeDamage(_attackDamage);
            AttackPerformed?.Invoke();
        }

        public void ApplyAetherCalibration(int damageBonus)
        {
            if (_isAetherCalibrated)
                return;

            _isAetherCalibrated = true;
            _attackDamage += Mathf.Max(1, damageBonus);
            AetherCalibrated?.Invoke();
        }
    }
}
