# Act 1 Production 03 - Bunker Intake Encounter Pass

## Purpose
Promote the validated bunker layout into an encounter-authored production scene. This pass makes the opening Act 1 pacing explicit in reusable data without changing the passed gameplay rules.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeEncounter01.unity`
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeLayout01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 03 - Bunker Intake Encounters`

## Encounter Assets
1. `Act1_IntakeSecurity.asset` — one Grey blocks the intake route.
2. `Act1_ContainmentCalibration.asset` — scientist rescue, Aether recovery, and calibration beat.
3. `Act1_ExtractionGuard.asset` — one Grey guards the extraction approach.

All assets live under `Assets/_BloodDirective/ScriptableObjects/Encounters/Act1/` and are referenced by passive `Act1EncounterAnchor` scene components. Anchors are content metadata only; they do not spawn, move, damage, or gate gameplay.

## Regression Contract
- Act 1 Production 02 remains unchanged.
- Existing Grey placement, scientist flow, Aether flow, calibrated-access door, and extraction conditions remain unchanged.
- No new runtime encounter manager, spawn logic, AI behavior, collision, camera, UI, loot, or dialogue is introduced.
- Anchors use `VisualOnly` and contain no renderers or colliders.

## Test Checklist
- [ ] Unity Console is clear after build and a normal run.
- [ ] The Act 1 Production 02 source scene remains unchanged.
- [ ] Intake Grey defeat works as before.
- [ ] Scientist rescue, Aether collection, and calibration work as before.
- [ ] Calibrated access unlocks the route to the extraction Grey.
- [ ] Extraction completes only after the extraction Grey is defeated.
- [ ] Each anchor references the matching encounter definition asset.
