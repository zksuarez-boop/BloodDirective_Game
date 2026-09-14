using System.Collections.Generic;
using UnityEngine;

namespace BloodDirective.Data
{
    [CreateAssetMenu(fileName = "LevelDefinition", menuName = "Blood Directive/Levels/Level Definition")]
    public sealed class LevelDefinition : ScriptableObject
    {
        [SerializeField] private string _levelId;
        [SerializeField] private string _displayName;
        [SerializeField] private string _biomeId;
        [SerializeField] private int _roomCount;
        [SerializeField] private MissionDefinition _mission;
        [SerializeField] private RoomDefinition[] _rooms;

        public string LevelId => _levelId;
        public string DisplayName => _displayName;
        public string BiomeId => _biomeId;
        public int RoomCount => _roomCount;
        public MissionDefinition Mission => _mission;
        public IReadOnlyList<RoomDefinition> Rooms => _rooms;
    }
}
