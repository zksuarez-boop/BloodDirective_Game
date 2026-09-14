using UnityEngine;

namespace BloodDirective.Data
{
    [CreateAssetMenu(fileName = "MissionDefinition", menuName = "Blood Directive/Missions/Mission Definition")]
    public sealed class MissionDefinition : ScriptableObject
    {
        [SerializeField] private string _missionId;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(2, 4)] private string _briefing;
        [SerializeField] private string _primaryObjective;
        [SerializeField] private string _biomeId;

        public string MissionId => _missionId;
        public string DisplayName => _displayName;
        public string Briefing => _briefing;
        public string PrimaryObjective => _primaryObjective;
        public string BiomeId => _biomeId;
    }
}
