# Movement Prototype 05 Spec

## Purpose

Validate the first enemy contact without adding AI, loot, skills, inventory, or full combat systems.

Prototype 05 proves that movement and blocker rules still work when an enemy target exists, and that a single player attack can damage and defeat that target.

## Scene

Scene path:

`Assets/_BloodDirective/Scenes/Prototypes/MovementPrototype05.unity`

Unity menu builder:

`Blood Directive > Reboot > Build Movement Prototype 05`

## Must Prove

- The player can move around the combat room normally.
- Walls and cover blockers stop movement.
- Decorative pipes, lights, floor joints, and range markers do not block movement.
- Clicking the enemy does not create a movement command through the enemy.
- Clicking the enemy within range damages it.
- Pressing Space near the enemy damages the nearest valid enemy.
- The enemy changes material when damaged.
- The enemy changes material again when defeated.
- The defeated enemy no longer receives damage.
- Unity reports no compile errors, play-mode errors, or normal-play Console messages.

## Controls

- Left mouse click on `Walkable`: move.
- Left mouse click on enemy while in range: attack.
- Space while near enemy: attack nearest enemy.
- Left mouse click on wall, cover, or empty space: no movement.

## Implementation Rules

- Keep movement script-driven.
- Do not use NavMesh yet.
- Do not add enemy AI or chasing.
- Do not add loot, XP, UI, floating text, skills, cooldown UI, save data, or imported art.
- Keep enemy health intentionally simple.
- Use non-alloc physics checks where reasonable.
- Keep normal gameplay Console output clean.

## Pass Criteria

Prototype 05 passes when the player can move, cannot pass blockers, can attack the stationary enemy, and the enemy visibly transitions from healthy to damaged to defeated without breaking movement.

## Failure Criteria

Prototype 05 fails if movement regresses, clicking the enemy moves the player into invalid space, enemy attacks do nothing in range, the enemy can be damaged after defeat, decorative objects block movement, or Unity reports new errors/warnings.