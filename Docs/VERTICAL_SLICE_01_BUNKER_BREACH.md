# Vertical Slice 01 - Bunker Breach

## Purpose
Prove that Blood Directive feels like a real playable game before expanding systems.

## Build Status
The first integration scene is `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice01.unity`. It promotes the verified Movement Prototype 12 loop without changing any earlier prototype scene. See `VERTICAL_SLICE_01_BUILD_STATUS.md` for the run checklist and deferred work.

## Scope
One authored scene. No procedural mission system. No full Act 1. No multiplayer implementation. No inventory tree. No final character art requirement.

## Scene Goal
The player moves through a short underground bunker route, reaches a scientist, fights a small Grey breach group, collects Aether, and exits through a blast door/elevator.

## Required Areas
1. Intake / briefing threshold
2. Main concrete corridor
3. Security checkpoint
4. Containment observation room
5. Breach hallway
6. Scientist rescue point
7. Extraction blast door or elevator

## Required Player Actions
- Move by left-clicking floor.
- Interact by left-clicking NPCs or environment objects.
- Fight one enemy group.
- Pick up Aether.
- Reach extraction.

## Required Systems For First Build
Phase 1:
- player placeholder
- click-to-move
- wall blocking
- isometric camera
- greybox bunker hallway

Phase 2:
- interaction prompt
- scientist NPC placeholder
- objective text

Phase 3:
- Short Grey enemy group
- basic player attack
- enemy health/death

Phase 4:
- Aether pickup
- extraction completion

## Test Definition
The slice is acceptable only when:
- player can move through the route reliably
- camera keeps the character and hallway readable
- walls block movement
- player can interact with the scientist
- enemy group can be defeated
- Aether can be collected
- extraction ends the slice
- no console errors occur during a normal playthrough

## Do Not Add Yet
- procedural map generation
- full mission system
- multiple acts
- multiplayer networking
- passive tree
- full inventory
- final animation system
- paid/imported art dependencies
