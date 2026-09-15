using UnityEngine;

namespace BloodDirective.Data
{
    [CreateAssetMenu(fileName = "EncounterDefinition", menuName = "Blood Directive/Encounters/Encounter Definition")]
    public sealed class EncounterDefinition : ScriptableObject
    {
        [SerializeField] private string _encounterId;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(2, 4)] private string _objective;
        [SerializeField] [TextArea(2, 4)] private string _completionCondition;
        [SerializeField] private int _expectedGreyCount;
        [SerializeField] private bool _requiresScientistCalibration;
        [SerializeField] private bool _requiresAether;

        public string EncounterId => _encounterId;
        public string DisplayName => _displayName;
        public string Objective => _objective;
        public string CompletionCondition => _completionCondition;
        public int ExpectedGreyCount => _expectedGreyCount;
        public bool RequiresScientistCalibration => _requiresScientistCalibration;
        public bool RequiresAether => _requiresAether;
    }
}
