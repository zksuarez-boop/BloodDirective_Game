using BloodDirective.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class Act1MissionBriefingPresenter : MonoBehaviour
    {
        [SerializeField] private MissionDefinition _mission;
        [SerializeField] private GameObject _briefingPanel;
        [SerializeField] private Text _briefingLabel;
        [SerializeField] private float _displayDuration = 6f;

        private float _visibleUntil;

        private void Awake()
        {
            if (_briefingPanel != null)
                _briefingPanel.SetActive(true);

            if (_briefingLabel != null && _mission != null)
                _briefingLabel.text = $"OPERATION // {_mission.DisplayName.ToUpperInvariant()}\n{_mission.Briefing}";

            _visibleUntil = Time.unscaledTime + _displayDuration;
        }

        private void Update()
        {
            if (_briefingPanel != null && _briefingPanel.activeSelf && Time.unscaledTime >= _visibleUntil)
                _briefingPanel.SetActive(false);
        }
    }
}
