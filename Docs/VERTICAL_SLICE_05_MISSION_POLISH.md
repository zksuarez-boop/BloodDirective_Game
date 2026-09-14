# Vertical Slice 05 - Mission Polish

## Goal
Give the validated Bunker Breach loop a readable mission arc without adding new gameplay mechanics.

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice05.unity`
- Source: `BunkerBreachVerticalSlice04.unity`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 05 - Mission Polish`

## Included
- Operation title and non-blocking deployment status.
- Event-driven callouts for scientist rescue, Grey defeat, Aether collection, weapon calibration, guard activation, guard defeat, and extraction.
- Elapsed run timer.
- Mission-complete panel with a replay command.
- Vertical Slice 05 is added to the Unity Build Settings so `R` can reload the active mission scene after completion.

## Regression Test
- [ ] Console is clear after build and a normal run.
- [ ] Mission title, timer, and initial mission status are visible.
- [ ] Each callout occurs only after its corresponding real state change.
- [ ] The mission-complete panel appears only after entering extraction.
- [ ] Pressing `R` after completion restarts the mission scene.
- [ ] All movement, combat, interaction, HUD, environment, and actor checks from prior slices still pass.

## Deferred
- Voice acting, audio clips, dialogue choices, save/checkpoint flow, cinematics, and final UI art.
