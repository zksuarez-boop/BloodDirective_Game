# Act 1 Production 02 - Bunker Intake Layout Pass

## Purpose
Create an authored environmental layout from the validated Bunker Intake foundation while preserving the proven gameplay route and collision ownership.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeLayout01.unity`
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntake.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 02 - Bunker Intake Layout`

## Authored Visual Zones
1. Intake
2. Breach Checkpoint
3. Containment Annex
4. Calibration Station
5. Extraction Passage

The scene instantiates the original Act 1 bunker wall, containment-console, pipe, and light prefabs under `Act1BunkerIntake_AuthoredLayout_VisualOnly`. These are scene-level visual placements only.

## Regression Contract
- The source production scene remains unchanged.
- The passed movement, combat, scientist, Aether, door, extraction, HUD, and camera systems remain unchanged.
- The authored-layout root and every referenced kit mesh use `VisualOnly`; the kit has no colliders.
- Existing Walkable and Blocker objects retain exclusive collision ownership.
- No roof, camera-occlusion, new enemy, loot, dialogue, or mission-rule work is included.

## Test Checklist
- [ ] Unity Console is clear after the build and a normal run.
- [ ] The Act 1 Production 01 source scene is unchanged.
- [ ] The full rescue-to-extraction route still completes in the new layout scene.
- [ ] Player movement cannot pass through validated gameplay blockers or leave the playable area.
- [ ] Inspecting authored-layout instances shows no collider components.
- [ ] The five authored zones read as distinct environmental landmarks without blocking the camera.
