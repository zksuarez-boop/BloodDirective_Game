using UnityEngine;
using UnityEngine.UI;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeObjectiveDisplay : MonoBehaviour
    {
        [SerializeField] private Text _label;

        public void SetObjective(string objectiveText, Color objectiveColor)
        {
            if (_label == null)
                return;

            _label.text = objectiveText;
            _label.color = objectiveColor;
        }
    }
}
