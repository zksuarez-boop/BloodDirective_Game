using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class PrototypeEnemyAwareness : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _idleMaterial;
        [SerializeField] private Material _alertMaterial;
        [SerializeField] private float _awarenessRadius = 5f;
        [SerializeField] private float _turnSpeed = 360f;

        private PrototypeHealth _health;
        private bool _isAlert;

        private void Awake()
        {
            _health = GetComponent<PrototypeHealth>();
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
        }

        private void Update()
        {
            if (_target == null || (_health != null && _health.IsDefeated))
                return;

            Vector3 toTarget = _target.position - transform.position;
            toTarget.y = 0f;

            bool shouldAlert = toTarget.sqrMagnitude <= _awarenessRadius * _awarenessRadius;
            SetAlertState(shouldAlert);

            if (!shouldAlert || toTarget.sqrMagnitude <= 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
        }

        private void SetAlertState(bool shouldAlert)
        {
            if (_isAlert == shouldAlert)
                return;

            _isAlert = shouldAlert;
            Material material = shouldAlert ? _alertMaterial : _idleMaterial;
            if (_renderer != null && material != null)
                _renderer.sharedMaterial = material;
        }
    }
}
