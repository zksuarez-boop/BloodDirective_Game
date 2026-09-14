# Vertical Slice 02 - Feedback and HUD

## Goal
Make the validated Bunker Breach loop readable at a glance without adding a new game mechanic or modifying collision, movement, or combat rules.

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice02.unity`
- Source: `BunkerBreachVerticalSlice01.unity`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 02 - Feedback and HUD`

## Included Feedback
- Top-left vitality, Aether, and weapon-status panels.
- Objective panel driven by the existing objective state.
- Bottom-center contextual instruction driven by the current verified step.
- Visual-only health bars over the breach Grey and extraction guard.
- Aether-calibrated weapon status after the scientist spends the Aether sample.

## Regression Test
- [ ] No Console warnings or errors after build and a normal run.
- [ ] Movement, walls, doors, and all interactions retain Vertical Slice 01 behavior.
- [ ] Vitality falls when the player is damaged and becomes critical at death.
- [ ] Aether changes from 00 to 01 on collection, then returns to 00 at calibration.
- [ ] Weapon status changes from Standard DMG 01 to Aether Calibrated DMG 02.
- [ ] Enemy health bars reduce correctly and disappear at death.
- [ ] Objective and instruction state progress through rescue, combat, Aether, calibration, guard, and extraction.

## Explicitly Deferred
- Final art, sound, animation, inventory, checkpoint/save flow, and all additional gameplay systems.
