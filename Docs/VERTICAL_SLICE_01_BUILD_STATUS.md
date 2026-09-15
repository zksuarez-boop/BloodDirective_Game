# Vertical Slice 01 Build Status

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice01.unity`
- Source baseline: `MovementPrototype12.unity`
- Scene root: `BunkerBreachVerticalSlice01_Root`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 01 - Bunker Breach`

## Proven Player Loop
1. Move through the bunker using left-click movement and the isometric camera.
2. Rescue the scientist.
3. Defeat the breach Grey.
4. Collect one Aether sample.
5. Return to the scientist and spend the Aether calibration.
6. Defeat the activated extraction guard.
7. Pass the opened blast door and step onto the extraction pad.

## Current Greybox Area Map
- Intake / briefing threshold: player start and scientist rescue.
- Main concrete corridor: click-to-move route with wall and cover collision.
- Security checkpoint: breach Grey encounter.
- Containment observation area: Aether sample.
- Breach hallway: route back to the scientist and calibration point.
- Extraction threshold: activated guard, opened blast door, and extraction pad.

## Regression Checklist
- [ ] Console is clear after the scene is built.
- [ ] Player can move and cannot pass through walls or cover.
- [ ] Scientist rescue updates the objective.
- [ ] The initial Grey can be defeated.
- [ ] Aether collection changes the counter from 0 to 1.
- [ ] Returning to the scientist spends the Aether and raises player damage from 1 to 2.
- [ ] The extraction guard activates only after calibration and dies in one calibrated hit.
- [ ] The blast door opens and the extraction pad completes the slice.
- [ ] No Console errors or warnings occur during a normal run.

## Explicitly Deferred
- Final environment art, authored dialogue, save/checkpoint flow, inventory, passive tree, multiple enemy groups, procedural layout, multiplayer, and additional acts.

## Promotion Rule
The Vertical Slice 01 scene is a stable integration target. Movement Prototypes 01 through 12 remain unchanged and continue to serve as focused regression references.
