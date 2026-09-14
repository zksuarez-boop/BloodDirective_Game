# Movement Prototype 06 - Basic Enemy Reaction

## Purpose
Prove the first enemy awareness behavior without changing the movement, collision, interaction, or damage systems that already passed testing.

## Player Behavior To Prove
- The player can click to move through the same blocked multi-room test space.
- The player can still attack the enemy by clicking it or pressing Space when in range.
- The enemy visibly reacts when the player enters awareness range by changing color and turning toward the player.

## Existing Behavior That Must Not Regress
- Movement stays click-to-move.
- The player cannot walk through walls, cover blocks, or outside walkable floor pieces.
- Clicking directly on the enemy should attack instead of issuing a move command through the enemy.
- Enemy health/damage/defeat from Prototype 05 still works.

## Not Included Yet
- Enemy chase movement.
- Enemy attacks or player damage.
- Loot, XP, skills, animation controllers, imported models, or final art.

## Implementation Notes
- Enemy awareness is distance-based and allocation-free during Update.
- The enemy uses a child facing marker so rotation is visible while still using placeholder geometry.
- All fields are private with SerializeField to keep Unity Inspector support without public mutable state.

## Unity Test Checklist
1. Stop Play Mode before running the builder.
2. Run Blood Directive > Reboot > Build Movement Prototype 06.
3. Press Play.
4. Click around the room and confirm movement/collision still works.
5. Move near the enemy and confirm it changes alert color and turns toward the player.
6. Move away and confirm it returns to idle color.
7. Move into attack range and confirm click attack and Space attack still damage/defeat the enemy.

## Pass Criteria
Prototype 06 passes when movement and enemy damage still work, and the enemy has a clear idle-to-alert reaction when the player approaches.
