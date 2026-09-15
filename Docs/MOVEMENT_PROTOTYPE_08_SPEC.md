# Movement Prototype 08 - Aether Pickup and Extraction

## Implementation Audit
- **Player behavior:** defeat the Grey, collect an Aether sample, enter extraction.
- **Regression coverage:** click movement, simple blocker collision, enemy damage/death, player death, and disabled post-death attacks.
- **Genre expectation:** a compact action-RPG objective loop with a visible reward and route change.
- **Not included:** inventory, quest UI, skill upgrades, dialogue, final art, or a reusable mission framework.
- **Changed systems:** one derived greybox scene, interaction click routing, Aether state, locked extraction door, and extraction state.
- **Unity test:** run the Prototype 08 builder in Edit Mode, then complete the checklist below.

## Player Behavior To Prove
- The Aether sample remains unavailable until the Grey is defeated.
- The player can collect available Aether by left-clicking it.
- Collecting Aether opens the extraction blast door.
- Entering the extraction pad after collection completes the scene objective visibly.

## Unity Test Checklist
1. Stop Play Mode and run **Blood Directive > Reboot > Build Movement Prototype 08**.
2. Press Play and verify normal click-to-move and blocker collision.
3. Confirm the Aether sample is dark while the Grey is alive.
4. Defeat the Grey. Confirm the Aether sample becomes cyan.
5. Click the sample. Confirm it becomes green and the blast door rises.
6. Move through the opened doorway and onto the extraction pad. Confirm it turns yellow-green.
7. Restart, let the Grey defeat the player, then confirm the player cannot attack, collect Aether, or complete extraction.
8. Confirm the Console has no warnings or errors.

## Pass Criteria
Prototype 08 passes when a player can complete the visible enemy-to-Aether-to-extraction loop without breaking passed movement and combat behavior.
