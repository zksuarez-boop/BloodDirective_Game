# Act 1 Production 05 - Bunker Intake Expansion Foundation

## Purpose
Promote the passed Level Blockout Prototype 01 into the larger Act 1 Bunker Intake production foundation while preserving the prototype as a regression reference.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeExpansion01.unity`
- Source: `Assets/_BloodDirective/Scenes/Prototypes/LevelBlockoutPrototype01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 05 - Bunker Intake Expansion`

## Level Data
- Asset: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Act1_BunkerIntakeExpansion.asset`
- Mission: `Act1_BunkerIntake.asset`
- Biome: `underground_bunker`
- Room count: `5`

The level asset is data only. It establishes an Act 1 content record without adding a runtime level manager or changing scene flow.

## Regression Contract
- Level Blockout Prototype 01 remains unchanged.
- Movement, collision, camera, encounters, mission gates, HUD, and mission presentation remain unchanged.
- No HUD placement, enemy, loot, dialogue, checkpoint, save, or new-mechanics work is included.

## Test Checklist
- [ ] Unity Console is clear after build and a normal run.
- [ ] Level Blockout Prototype 01 is unchanged.
- [ ] All five connected spaces are traversable as passed in the prototype.
- [ ] Player cannot cross existing or new blocker geometry.
- [ ] The normal rescue-to-extraction route still completes.
- [ ] The level definition references the Bunker Intake mission and reports five rooms.
