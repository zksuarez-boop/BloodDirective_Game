# Movement Prototype 03 Spec

## Purpose

Validate basic interaction clicks without weakening the movement foundation from Prototypes 01 and 02.

Prototype 03 proves that terminals, panels, locked doors, and objective markers can exist in the bunker layout without accidentally becoming walkable space or movement blockers unless intentionally configured.

## Scene

Scene path:

`Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype03.unity`

Unity menu builder:

`Blood Directive > Reboot > Build Movement Prototype 03`

## Must Prove

- The player can move through the ready room, corridor, and control room.
- The player cannot pass through walls, the server rack, the pillar, or the locked blast door.
- The player cannot leave the authored walkable floor.
- Door headers, lights, pipe pieces, and floor markings do not block movement.
- Clicking an interactable object activates it instead of issuing a movement command through it.
- Clicking a wall or non-walkable black space does not move the player.
- Clicking near an interactable on visible floor still moves the player normally.
- Unity reports no compile errors, play-mode errors, or warnings caused by this prototype.

## Controls

- Left mouse click on `Walkable`: move.
- Left mouse click on `Interactable`: activate.
- Left mouse click on `Blocker` or empty space: no movement.

## Implementation Rules

- Keep movement script-driven.
- Keep gameplay collision on simple blocker shapes.
- Keep decorative scene pieces on `VisualOnly` with colliders removed.
- Keep interactables on the `Interactable` layer.
- Do not add enemies, combat, UI, inventory, save data, or imported character art.
- Do not make the locked door open yet; this prototype only tests interaction click safety.

## Pass Criteria

Prototype 03 passes when the player can navigate the rooms exactly as Prototype 02 allowed, interactables visibly activate when clicked, and interaction clicks do not create accidental movement destinations.

## Failure Criteria

Prototype 03 fails if interaction objects block normal nearby movement, clicks through interactables move the player, visuals block the player, walls are passable, or Unity reports new errors/warnings.