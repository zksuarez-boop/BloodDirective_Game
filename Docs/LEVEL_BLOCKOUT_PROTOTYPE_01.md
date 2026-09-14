# Level Blockout Prototype 01

## Purpose
Validate click movement, collision ownership, camera follow, and combat readability across a larger connected bunker footprint before committing the layout to Act 1 production.

## Prototype Scene
- Scene: `Assets/_BloodDirective/Scenes/Prototypes/LevelBlockoutPrototype01.unity`
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeMission01.unity`
- Build menu: `Blood Directive > Reboot > Build Level Blockout Prototype 01`

## Layout
The existing Bunker Intake and Containment Annex remain intact. A new east service doorway connects to:
1. East Service Corridor
2. Logistics Bay
3. Relay Corridor
4. Relay Chamber

This produces five connected traversal spaces when combined with the established intake area. The side wing is optional: players can enter it to test map-scale movement, then return to complete the existing rescue-to-extraction mission.

## Collision Contract
- Walkable objects provide only `Walkable` colliders.
- Blocker objects provide only `Blocker` colliders.
- Every rendered floor, wall, light housing, pipe, and accent is `VisualOnly` and collider-free.
- No roof or camera-occlusion behavior is added.

## Explicitly Unchanged
- Existing player movement settings and camera component
- Scientist, Aether, calibration, Grey, door, extraction, HUD, and mission presentation behavior
- Enemy counts, health, damage, AI, objectives, and mission completion rules

## Test Checklist
- [ ] Unity Console is clear after build and a normal run.
- [ ] The Act 1 Production 04 source scene remains unchanged.
- [ ] Player can enter and leave all four new east-wing spaces by click movement.
- [ ] Player cannot pass through any new perimeter wall, crate, or relay core blocker.
- [ ] The camera follows cleanly through the east wing without roof obstruction.
- [ ] Player can return to the original bunker route and complete the full mission.
