# Act 1 Production 01 - Bunker Intake

## Purpose
Begin Act 1 production from a frozen, validated Vertical Slice 06 baseline without modifying the demo scene.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntake.unity`
- Source: `BunkerBreachVerticalSlice06.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 01 - Bunker Intake`

## Original Bunker Kit
- `Assets/_BloodDirective/Prefabs/Environment/Act1Bunker/Act1Bunker_FloorModule.prefab`
- `Assets/_BloodDirective/Prefabs/Environment/Act1Bunker/Act1Bunker_WallModule.prefab`
- `Assets/_BloodDirective/Prefabs/Environment/Act1Bunker/Act1Bunker_ContainmentConsole.prefab`
- `Assets/_BloodDirective/Prefabs/Environment/Act1Bunker/Act1Bunker_PipeModule.prefab`
- `Assets/_BloodDirective/Prefabs/Environment/Act1Bunker/Act1Bunker_LightModule.prefab`

Every kit mesh is `VisualOnly` and has no collider. Walkable and Blocker ownership stays with validated gameplay geometry.

## Mission Content Asset
- `Assets/_BloodDirective/ScriptableObjects/Missions/Act1_BunkerIntake.asset`
- Defines the mission identifier, display name, briefing, primary objective, and biome for future data-driven mission flow.

## Regression Test
- [ ] Unity Console is clear after build and a normal run.
- [ ] The Vertical Slice 06 demo scene is unchanged.
- [ ] Act 1 Bunker Intake completes the full rescue-to-extraction mission route.
- [ ] All bunker-kit prefabs are visual-only and collider-free.
- [ ] The mission definition asset contains the opening Act 1 content metadata.

## Deferred
- Additional Act 1 rooms, final imported art, camera occlusion, dialogue, save/checkpoint flow, loot, and character/class expansion.
