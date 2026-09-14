# Vertical Slice 05.1 - Mission Gate Enforcement

## Goal
Correct the Bunker Breach progression loophole identified during the Vertical Slice 05 playthrough.

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05_1.unity`
- Source: `BunkerBreachVerticalSlice05.unity`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 05.1 - Mission Gate Enforcement`

## Enforced Chain
1. Rescue the scientist.
2. Defeat the breach Grey.
3. Collect Aether.
4. Return to the scientist and complete weapon calibration; the blast door opens.
5. Defeat the activated extraction guard.
6. Extraction pad activates.
7. Reach extraction to complete the mission.

## Implementation
- Aether collection does not open the blast door in this scene.
- Extraction requires both scientist calibration and guard defeat.
- A visual-independent mission exit gate opens the access door after calibration, then enables the extraction pad only after the guard-death event.
- Optional serialized gate fields default to the previous behavior, preserving earlier prototypes and slices.

## Regression Test
- [ ] No Console warnings or errors after build and a normal run.
- [ ] Aether cannot be collected before scientist rescue and breach-Grey defeat.
- [ ] Collecting Aether leaves the blast door closed.
- [ ] Extraction cannot complete before calibration.
- [ ] Calibration activates the guard and opens the blast door to the guard area.
- [ ] The extraction pad remains invalid until the guard is defeated.
- [ ] Extraction completes only after all preceding steps.
- [ ] `R` replay still reloads the corrected scene after mission completion.
