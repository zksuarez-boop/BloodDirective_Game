using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    public sealed class PrototypeHealthBar : MonoBehaviour
    {
        [SerializeField] private PrototypePlayerHealth _health;
        [SerializeField] private Transform _fill;
        [SerializeField] private float _fullWidth = 1.6f;

        private void LateUpdate()
        {
            if (_health == null || _fill == null)
                return;

            Vector3 scale = _fill.localScale;
            scale.x = Mathf.Max(0.02f, _fullWidth * _health.NormalizedHealth);
            _fill.localScale = scale;
        }
    }
}
