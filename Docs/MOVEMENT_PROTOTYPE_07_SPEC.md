# Movement Prototype 07 - Enemy Contact Damage + Player Health

## Purpose
Prove the first danger loop after movement, collision, enemy damage, awareness, and death behavior have passed.

## Player Behavior To Prove
- The player can still click-to-move through the blocked multi-room layout.
- The enemy detects the player, turns toward the player, and moves toward the player.
- If the enemy reaches contact range, player health decreases on a short cooldown.
- The player can still move away from danger.
- The player can still attack and defeat the enemy.
- Once defeated, the enemy stops moving and stops damaging the player.

## Existing Behavior That Must Not Regress
- Movement stays click-to-move.
- Walls, cover blocks, and map boundaries still block movement.
- Clicking directly on the enemy still attacks instead of moving through the enemy.
- Enemy health, damage, and defeat behavior still work from Prototype 05 and 06.
- The Console should stay clean except for true warnings/errors.

## Not Included Yet
- Player death screen.
- Potions, healing, loot, XP, or skills.
- Animation controllers, imported models, VFX, or final UI.
- Full pathfinding/chase AI across complex layouts.

## Implementation Notes
- Contact damage is separate from chase movement so we can tune danger without touching movement controls.
- Enemy chase uses non-alloc capsule casting against blockers to avoid walking through test walls.
- Player health is represented by a small world-space placeholder bar attached to the player.
- All Inspector fields are private SerializeField values.

## Unity Test Checklist
1. Stop Play Mode before running the builder.
2. Run Blood Directive > Reboot > Build Movement Prototype 07.
3. Press Play.
4. Confirm click-to-move and collision still behave correctly.
5. Approach the enemy and confirm it turns and moves toward you.
6. Let the enemy reach you and confirm the player changes damaged color and the health bar shrinks.
7. Move away and confirm you can escape contact damage.
8. Attack with click or Space and confirm the enemy can be defeated.
9. Confirm the defeated enemy stops chasing and stops damaging the player.
10. Restart Play Mode and let the enemy defeat the player. Confirm neither clicking the enemy nor pressing Space damages it afterward.
11. Confirm the Console has no unused-display-name warning after recompilation.

## Pass Criteria
Prototype 07 passes when the player can move, take visible contact damage, escape, and defeat the enemy without breaking earlier movement/combat rules.
