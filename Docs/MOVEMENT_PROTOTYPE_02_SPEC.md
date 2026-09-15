# Movement Prototype 02 Spec

## Purpose

Validate room-to-room click movement before adding combat, imported art, UI, enemies, or story systems.

Prototype 02 must prove that the player can move through a small bunker layout without movement being broken by walls, blockers, door headers, overhead pipes, or decorative details.

## Scene

Scene path:

`Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype02.unity`

Unity menu builder:

`Blood Directive > Reboot > Build Movement Prototype 02`

## Must Prove

- The player can click-move inside the starting room.
- The player can move through the first doorway into the corridor.
- The player can turn right through the second doorway into the connected room.
- The player cannot move through concrete walls.
- The player cannot move through the test crate or pillar.
- The player cannot leave the walkable bunker floor.
- Visual-only ceiling/pipe/light pieces do not block movement.
- No console warnings or errors appear after opening and playing the scene.

## Controls

- Left mouse click: move to clicked floor location.
- Clicks only count when they hit objects on the `Walkable` layer.
- Movement is stopped only by objects on the `Blocker` layer.

## Implementation Rules

- Keep player movement script-driven for now.
- Do not use NavMesh in this prototype.
- Do not add combat, enemies, inventory, UI, or lore triggers.
- Keep decorative objects on `VisualOnly` with colliders removed.
- Keep gameplay collision on simple blocker shapes that are easy to inspect.

## Pass Criteria

Prototype 02 passes when the player can move from the start room, through the corridor, into the east room, while all walls and test blockers correctly prevent illegal movement.

## Failure Criteria

Prototype 02 fails if any decorative object blocks movement, any wall can be crossed, the player cannot enter a connected room, clicks do nothing, or Unity reports compile errors.