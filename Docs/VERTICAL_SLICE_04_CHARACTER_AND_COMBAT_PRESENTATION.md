# Vertical Slice 04 - Character and Combat Presentation

## Goal
Improve character silhouette and combat readability without replacing the validated actor gameplay bodies.

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice04.unity`
- Source: `BunkerBreachVerticalSlice03.unity`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 04 - Character and Combat Presentation`

## Included
- Visual-only modular operator rig with vest, helmet, backpack, and Aether rifle.
- Visual-only Grey rigs with readable heads, limbs, and red eye accents.
- Event-driven body hit flash, defeat presentation, weapon attack flash, and Aether-calibrated weapon material.
- Hidden original capsule renderers that remain the authoritative source for click movement, health, combat, and AI.

## Regression Test
- [ ] Console is clear after build and a normal run.
- [ ] Operator movement and facing remain responsive.
- [ ] Grey chase and attack behavior remain unchanged.
- [ ] Player and Grey hit feedback occurs only after real damage is applied.
- [ ] Defeated player cannot attack; defeated Grey cannot chase or attack.
- [ ] Rifle changes to the calibrated state only after the scientist consumes Aether.
- [ ] Full rescue-to-extraction route remains complete.
- [ ] All rig primitives are `VisualOnly` and have no colliders.

## Deferred
- Imported models, skeletal animation clips, VFX prefabs, sound, final character materials, and additional enemy types.
