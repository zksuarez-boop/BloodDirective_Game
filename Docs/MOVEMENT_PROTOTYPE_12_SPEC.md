# Movement Prototype 12 - Aether Weapon Calibration

## Implementation Audit
- **Player behavior:** spend collected Aether with the rescued scientist to strengthen the weapon and defeat the extraction guard.
- **Regression coverage:** the full Prototype 11 mission loop, including movement, collision, objective state, Aether counter, player death, and extraction.
- **Genre expectation:** a rare resource creates a visible, immediate character-power improvement.
- **Not included:** equipment inventory, permanent upgrades, save data, skill trees, multiple currencies, or a reusable upgrade system.
- **Changed systems:** one calibration interaction, one guarded resource spend, player attack damage, one dormant extraction enemy, and objective state text.
- **Unity test:** build Prototype 12 in Edit Mode and complete the sequence below.

## Unity Test Checklist
1. Stop Play Mode and run **Blood Directive > Reboot > Build Movement Prototype 12**.
2. Confirm the Hierarchy root is `MovementPrototype12_Root` and the counter starts at `AETHER: 0`.
3. Rescue the scientist, defeat the original Grey, and collect Aether. Confirm the counter reads `AETHER: 1` and the objective asks you to return to the scientist.
4. Click the rescued scientist again. Confirm the scientist becomes cyan, the counter becomes `AETHER: 0`, and the objective changes to the extraction guard.
5. Confirm the dormant guard did not chase or take damage before calibration.
6. Defeat the two-health extraction guard with one post-calibration attack.
7. Reach extraction and confirm `MISSION COMPLETE`.
8. Confirm the Console has no warnings or errors.

## Pass Criteria
Prototype 12 passes when one Aether unit is spent exactly once to make a visible and testable weapon-damage upgrade without breaking the established mission loop.
