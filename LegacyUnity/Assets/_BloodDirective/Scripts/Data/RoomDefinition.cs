using UnityEngine;

namespace BloodDirective.Data
{
    [CreateAssetMenu(fileName = "RoomDefinition", menuName = "Blood Directive/Levels/Room Definition")]
    public sealed class RoomDefinition : ScriptableObject
    {
        [SerializeField] private string _roomId;
        [SerializeField] private string _displayName;
        [SerializeField] private int _sequenceIndex;
        [SerializeField] [TextArea(2, 4)] private string _purpose;
        [SerializeField] private EncounterDefinition _encounter;
        [SerializeField] private bool _containsScientist;
        [SerializeField] private bool _containsAether;
        [SerializeField] private bool _isExtraction;

        public string RoomId => _roomId;
        public string DisplayName => _displayName;
        public int SequenceIndex => _sequenceIndex;
        public string Purpose => _purpose;
        public EncounterDefinition Encounter => _encounter;
        public bool ContainsScientist => _containsScientist;
        public bool ContainsAether => _containsAether;
        public bool IsExtraction => _isExtraction;
    }
}
