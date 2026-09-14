# Vertical Slice 03 - Environment Presentation

## Goal
Establish the Bunker Breach visual direction without changing the validated movement and collision contract.

## Scene
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice03.unity`
- Source: `BunkerBreachVerticalSlice02.unity`
- Rebuild menu: `Blood Directive > Reboot > Build Vertical Slice 03 - Environment Presentation`

## Visual Pass
- Modular steel floor plates with recessed panel insets.
- Concrete wall cladding and dark metal trim.
- Containment and extraction wall hardware integrated around existing blocker geometry.
- Cool cyan containment lighting and limited red alarm accents.
- Cover casing and overhead conduits.

## Collision Contract
- Every object added by this pass is named `VisualOnly`, uses the `VisualOnly` layer, and has its collider removed at build time.
- Existing Walkable and Blocker objects remain the only movement and collision authority.
- No new gameplay objects, interactions, or combat rules are added.

## Regression Test
- [ ] Console is clear after build and a normal run.
- [ ] The player still cannot pass through the original walls or cover.
- [ ] The player can still reach the scientist, Grey, Aether, calibration point, guard, blast door, and extraction pad.
- [ ] UI and world health bars from Vertical Slice 02 still work.
- [ ] The full rescue-to-extraction sequence completes.
- [ ] New visuals do not obscure the player, objectives, or interactable targets from the isometric camera.

## Deferred
- Imported environment assets, final textures, mesh LODs, baked lighting, decals, animated props, audio, and final art polish.
