using UnityEngine;

namespace BloodDirective.Prototypes.Combat
{
    [DisallowMultipleComponent]
    public sealed class VerticalSliceWorldHealthBar : MonoBehaviour
    {
        [SerializeField] private PrototypeHealth _health;
        [SerializeField] private Transform _fill;
        [SerializeField] private float _fullWidth = 1.2f;
        [SerializeField] private float _heightOffset = 2f;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_health == null || _fill == null)
                return;

            Vector3 scale = _fill.localScale;
            scale.x = Mathf.Max(0.02f, _fullWidth * _health.NormalizedHealth);
            _fill.localScale = scale;

            transform.localPosition = new Vector3(0f, _heightOffset, 0f);
            if (_camera != null)
                transform.rotation = Quaternion.LookRotation(_camera.transform.forward, _camera.transform.up);

            gameObject.SetActive(!_health.IsDefeated);
        }
    }
}
