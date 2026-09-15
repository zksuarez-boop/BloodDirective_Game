using BloodDirective.Prototypes.Combat;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class VerticalSliceMissionFlowController : MonoBehaviour
    {
        [SerializeField] private PrototypeScientistRescue _scientist;
        [SerializeField] private PrototypeHealth _breachGrey;
        [SerializeField] private PrototypeAetherPickup _aether;
        [SerializeField] private PrototypeCalibrationGuard _guard;
        [SerializeField] private PrototypeHealth _guardHealth;
        [SerializeField] private PrototypeExtractionZone _extraction;
        [SerializeField] private Text _statusLabel;
        [SerializeField] private Text _timerLabel;
        [SerializeField] private Text _completionLabel;
        [SerializeField] private GameObject _completionPanel;
        [SerializeField] private float _statusDuration = 3.5f;

        private float _runStartedAt;
        private float _statusEndsAt;
        private int _lastDisplayedSecond = -1;
        private bool _isComplete;

        private void Awake()
        {
            _runStartedAt = Time.time;
            if (_completionPanel != null)
                _completionPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (_scientist != null)
            {
                _scientist.ScientistRescued += OnScientistRescued;
                _scientist.WeaponCalibrated += OnWeaponCalibrated;
            }
            if (_breachGrey != null)
                _breachGrey.Defeated += OnBreachGreyDefeated;
            if (_aether != null)
                _aether.AetherCollected += OnAetherCollected;
            if (_guard != null)
                _guard.Activated += OnGuardActivated;
            if (_guardHealth != null)
                _guardHealth.Defeated += OnGuardDefeated;
            if (_extraction != null)
                _extraction.ExtractionCompleted += OnExtractionCompleted;

            PublishStatus("MISSION LINK ESTABLISHED // RESCUE THE SCIENTIST", new Color(0.7f, 0.9f, 1f));
        }

        private void OnDisable()
        {
            if (_scientist != null)
            {
                _scientist.ScientistRescued -= OnScientistRescued;
                _scientist.WeaponCalibrated -= OnWeaponCalibrated;
            }
            if (_breachGrey != null)
                _breachGrey.Defeated -= OnBreachGreyDefeated;
            if (_aether != null)
                _aether.AetherCollected -= OnAetherCollected;
            if (_guard != null)
                _guard.Activated -= OnGuardActivated;
            if (_guardHealth != null)
                _guardHealth.Defeated -= OnGuardDefeated;
            if (_extraction != null)
                _extraction.ExtractionCompleted -= OnExtractionCompleted;
        }

        private void Update()
        {
            RefreshTimer();
            if (!_isComplete && Time.time >= _statusEndsAt && _statusLabel != null)
                _statusLabel.text = string.Empty;

            if (_isComplete && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        private void OnScientistRescued()
        {
            PublishStatus("SCIENTIST SECURED // BREACH CONTACT CONFIRMED", new Color(0.35f, 1f, 0.6f));
        }

        private void OnBreachGreyDefeated()
        {
            PublishStatus("BREACH GREY NEUTRALIZED // RECOVER AETHER", new Color(1f, 0.74f, 0.3f));
        }

        private void OnAetherCollected()
        {
            PublishStatus("AETHER ACQUIRED // RETURN TO SCIENTIST", new Color(0.18f, 0.92f, 1f));
        }

        private void OnWeaponCalibrated()
        {
            PublishStatus("AETHER CALIBRATION COMPLETE // WEAPON OUTPUT INCREASED", new Color(0.35f, 1f, 0.6f));
        }

        private void OnGuardActivated()
        {
            PublishStatus("EXTRACTION GUARD ACTIVATED", new Color(1f, 0.28f, 0.18f));
        }

        private void OnGuardDefeated()
        {
            PublishStatus("EXTRACTION ROUTE CLEARED", new Color(0.35f, 1f, 0.6f));
        }

        private void OnExtractionCompleted()
        {
            _isComplete = true;
            if (_completionPanel != null)
                _completionPanel.SetActive(true);
            if (_completionLabel != null)
            {
                int elapsedSeconds = Mathf.FloorToInt(Time.time - _runStartedAt);
                _completionLabel.text = $"MISSION COMPLETE\nBUNKER BREACH SECURED\nTIME {FormatTime(elapsedSeconds)}\n\nPRESS R TO REPLAY";
            }
        }

        private void RefreshTimer()
        {
            if (_timerLabel == null)
                return;

            int elapsedSeconds = Mathf.FloorToInt(Time.time - _runStartedAt);
            if (elapsedSeconds == _lastDisplayedSecond)
                return;

            _lastDisplayedSecond = elapsedSeconds;
            _timerLabel.text = $"RUN TIME // {FormatTime(elapsedSeconds)}";
        }

        private void PublishStatus(string message, Color color)
        {
            if (_statusLabel == null)
                return;

            _statusLabel.text = message;
            _statusLabel.color = color;
            _statusEndsAt = Time.time + _statusDuration;
        }

        private static string FormatTime(int elapsedSeconds)
        {
            int minutes = elapsedSeconds / 60;
            int seconds = elapsedSeconds % 60;
            return $"{minutes:00}:{seconds:00}";
        }
    }
}
