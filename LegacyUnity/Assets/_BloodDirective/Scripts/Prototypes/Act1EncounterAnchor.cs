using BloodDirective.Data;
using UnityEngine;

namespace BloodDirective.Prototypes
{
    /// <summary>
    /// Associates an authored Act 1 scene location with its data-driven encounter definition.
    /// </summary>
    public sealed class Act1EncounterAnchor : MonoBehaviour
    {
        [SerializeField] private EncounterDefinition _definition;

        public EncounterDefinition Definition => _definition;
    }
}
