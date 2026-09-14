# Movement Prototype 04 Spec

## Purpose

Validate the first tiny gameplay chain: interact with a terminal, open a locked door, and move into a new exit area.

Prototype 04 keeps movement, blockers, and interaction isolated while proving that a gameplay object can change collision safely at runtime.

## Scene

Scene path:

`Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype04.unity`

Unity menu builder:

`Blood Directive > Reboot > Build Movement Prototype 04`

## Must Prove

- The player can move normally through the ready room, corridor, and control room.
- The locked blast door blocks movement before terminal activation.
- Clicking the terminal activates it and opens the blast door.
- Once opened, the door no longer blocks movement.
- The player can move through the opened doorway into the exit vestibule.
- The exit zone visibly changes when reached.
- Walls, server rack, and pillar still block movement.
- Door headers, lights, pipes, and floor details do not block movement.
- Clicking terminal/interactable objects does not issue movement through them.
- Unity reports no compile errors, play-mode errors, or normal-play Console messages.

## Controls

- Left mouse click on `Walkable`: move.
- Left mouse click on `Interactable`: activate.
- Left mouse click on `Blocker` or empty space: no movement.

## Implementation Rules

- Keep movement script-driven.
- Do not use NavMesh yet.
- Use simple primitive colliders for gameplay collision.
- Use visual-only objects without colliders for decoration.
- Door opening may disable only the door blocker collider.
- Do not add enemies, combat, inventory, save data, UI, imported art, or story dialogue.

## Pass Criteria

Prototype 04 passes when the player can activate the terminal, open the door, enter the exit vestibule, and trigger the exit marker while movement rules from Prototypes 01-03 remain intact.

## Failure Criteria

Prototype 04 fails if the door is passable before activation, remains blocked after activation, interactions create unwanted movement, the exit zone does not react, walls become passable, visuals block movement, or Unity reports new errors/warnings.