# Movement Prototype 01 Spec

## Purpose
Build and verify the first playable foundation for Blood Directive: reliable isometric click-to-move in one authored bunker hallway.

This prototype exists to prove movement and camera before combat, enemies, story scripting, loot, multiplayer, or imported character art.

## Target Scene
Create one test scene for movement validation.

Recommended scene name:
`MovementPrototype01`

Recommended location:
`Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype01.unity`

## Player Goal
The player should be able to left-click through a short bunker hallway and move reliably from start to end without getting stuck.

## Player Controls
- Left mouse click on walkable floor: move to clicked location.
- Holding left mouse is optional for the first version; single-click movement is required.
- Mouse wheel zoom may be added if camera exists, but zoom is not the primary test.
- No keyboard movement required for this prototype.

## Required Behavior
- Player starts near the beginning of the hallway.
- Player moves to clicked floor positions.
- Player rotates toward movement direction.
- Player stops near the target point.
- Walls block movement.
- Closed blocker pieces block movement.
- Visual-only props do not block movement.
- Camera follows the player smoothly.
- Camera angle feels isometric ARPG, not top-down ceiling view.

## Technical Methodology
Use authored gameplay collision, not visual mesh collision.

Recommended layer model:
- `Walkable`: invisible/simple floor surfaces that receive movement clicks.
- `Blocker`: walls, closed doors, and hard collision objects.
- `VisualOnly`: visual bunker art, props, pipes, decals, lights, ceiling hints.
- `Player`: player object.

Movement flow:
1. Mouse click raycasts against `Walkable` only.
2. Controller stores the clicked destination.
3. Player moves by script toward the destination.
4. A capsule-style blocker check prevents movement through `Blocker` geometry.
5. Visual meshes do not affect movement unless intentionally assigned to `Blocker`.

## Recommended Movement Approach
For Prototype 01, use script-driven movement rather than NavMesh.

Reason:
- The previous project repeatedly failed because runtime geometry and NavMesh produced tiny movement islands.
- Script-driven movement with clean blocker checks is easier to reason about for the first authored corridor.
- NavMesh can be reconsidered later for enemy pathfinding or larger layouts.

Initial movement settings:
- move speed: 5 units/second
- stopping distance: 0.15 units
- player radius: 0.35 units
- player height: 1.8 units
- rotation speed: immediate or fast turn, approximately 540 degrees/second

## Camera Requirements
Camera should use a fixed isometric ARPG angle.

Initial camera settings:
- angle: approximately 45-55 degrees downward
- yaw: diagonal view across the hallway, not straight top-down
- follow smoothing: modest, no laggy delay
- framing: player slightly below center so forward path is visible
- no ceiling objects blocking camera

## Hallway Greybox Requirements
The prototype hallway should be simple but correctly structured.

Required pieces:
- rectangular walkable corridor floor
- left and right wall blockers
- one open doorway/pass-through
- one closed blocker area or side wall to test collision
- visual-only ceiling hint or overhead pipe that does not block camera or movement

Approximate size:
- corridor length: 20-30 world units
- corridor width: 5-7 world units
- wall height: 2.5-4 world units

## Visual Standard For Prototype
Use simple materials only.

Allowed:
- dark floor material
- concrete wall material
- subtle lane line or floor seam
- one or two placeholder lights

Do not add:
- imported art packs
- final character model
- enemies
- loot
- objective system
- procedural map generation
- complex post-processing

## Test Procedure
1. Open `MovementPrototype01`.
2. Press Play.
3. Click 10 different floor positions along the hallway.
4. Confirm player moves every time.
5. Click beyond a wall or closed blocker.
6. Confirm player does not pass through blockers.
7. Click near visual-only props if present.
8. Confirm visual-only props do not trap the player.
9. Move from hallway start to hallway end.
10. Confirm camera follows and keeps the path readable.
11. Confirm Console has no errors.

## Acceptance Criteria
Prototype 01 passes only when:
- movement is reliable for repeated clicks
- player can traverse the full hallway
- blockers stop the player
- visual-only objects do not interfere
- camera remains readable
- no compile errors
- no runtime errors during normal movement test

## Failure Handling
If movement fails:
- stop adding features
- capture exact click behavior
- capture console logs
- fix movement before camera polish, combat, story, or assets

## Explicit Non-Goals
This prototype does not include:
- combat
- enemies
- Aether pickups
- scientist NPC
- multiplayer
- final UI
- animation blending
- imported Meshy/Asset Store character
- procedural generation
- full bunker visuals

## Next Phase After Acceptance
After Movement Prototype 01 passes, build Camera/Layout Prototype 02 or extend this scene into the first greybox Bunker Breach corridor.
