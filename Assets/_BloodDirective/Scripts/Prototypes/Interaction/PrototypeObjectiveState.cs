using BloodDirective.Prototypes.Combat;
using UnityEngine;

namespace BloodDirective.Prototypes.Interaction
{
    [DisallowMultipleComponent]
    public sealed class PrototypeObjectiveState : MonoBehaviour
    {
        [SerializeField] private PrototypeScientistRescue _scientist;
        [SerializeField] private PrototypeHealth _grey;
        [SerializeField] private PrototypeAetherPickup _aether;
        [SerializeField] private PrototypeExtractionZone _extraction;
        [SerializeField] private PrototypeHealth _extractionGuard;
        [SerializeField] private PrototypeObjectiveDisplay _display;

        private ObjectiveStep _currentStep = (ObjectiveStep)(-1);

        private enum ObjectiveStep
        {
            RescueScientist,
            NeutralizeGrey,
            RecoverAether,
            CalibrateWeapon,
            NeutralizeExtractionGuard,
            ReachExtraction,
            MissionComplete
        }

        private void Update()
        {
            ObjectiveStep nextStep = ResolveStep();
            if (nextStep == _currentStep)
                return;

            _currentStep = nextStep;
            ApplyDisplay(nextStep);
        }

        private ObjectiveStep ResolveStep()
        {
            if (_extraction != null && _extraction.IsComplete)
                return ObjectiveStep.MissionComplete;
            if (_aether != null && _aether.IsCollected)
            {
                if (_scientist != null && !_scientist.IsCalibrated)
                    return ObjectiveStep.CalibrateWeapon;
                if (_extractionGuard != null && !_extractionGuard.IsDefeated)
                    return ObjectiveStep.NeutralizeExtractionGuard;
                return ObjectiveStep.ReachExtraction;
            }
            if (_grey != null && _grey.IsDefeated)
                return ObjectiveStep.RecoverAether;
            if (_scientist != null && _scientist.IsRescued)
                return ObjectiveStep.NeutralizeGrey;

            return ObjectiveStep.RescueScientist;
        }

        private void ApplyDisplay(ObjectiveStep step)
        {
            if (_display == null)
                return;

            switch (step)
            {
                case ObjectiveStep.NeutralizeGrey:
                    _display.SetObjective("OBJECTIVE: Neutralize the Grey", new Color(1f, 0.72f, 0.2f));
                    break;
                case ObjectiveStep.RecoverAether:
                    _display.SetObjective("OBJECTIVE: Recover Aether", new Color(0.2f, 0.92f, 1f));
                    break;
                case ObjectiveStep.CalibrateWeapon:
                    _display.SetObjective("OBJECTIVE: Return to the scientist", new Color(0.35f, 1f, 0.55f));
                    break;
                case ObjectiveStep.NeutralizeExtractionGuard:
                    _display.SetObjective("OBJECTIVE: Neutralize extraction guard", new Color(1f, 0.72f, 0.2f));
                    break;
                case ObjectiveStep.ReachExtraction:
                    _display.SetObjective("OBJECTIVE: Reach extraction", new Color(0.35f, 1f, 0.55f));
                    break;
                case ObjectiveStep.MissionComplete:
                    _display.SetObjective("MISSION COMPLETE", new Color(0.72f, 1f, 0.35f));
                    break;
                default:
                    _display.SetObjective("OBJECTIVE: Rescue the scientist", new Color(0.9f, 0.94f, 1f));
                    break;
            }
        }
    }
}
