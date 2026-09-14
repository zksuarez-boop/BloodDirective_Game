# Movement Prototype 11 - Aether Counter

## Implementation Audit
- **Player behavior:** collect Aether and see the resource total change during the established mission loop.
- **Regression coverage:** scientist gate, Grey combat, Aether collection, blast door, extraction, objective display, and player death rules.
- **Genre expectation:** a collected resource has clear player-facing value and feedback.
- **Not included:** inventory, saving, spending, upgrades, crafting, or skill trees.
- **Changed systems:** optional Aether wallet, counter display, and existing pickup reward wiring.
- **Unity test:** build Prototype 11 in Edit Mode and verify the counter through one normal run.

## Unity Test Checklist
1. Stop Play Mode and run **Blood Directive > Reboot > Build Movement Prototype 11**.
2. Confirm the Hierarchy root is `MovementPrototype11_Root`.
3. Press Play and confirm the top-right HUD reads `AETHER: 0`.
4. Complete scientist rescue and defeat the Grey. Confirm the counter stays at zero.
5. Collect Aether. Confirm the counter changes once to `AETHER: 1`.
6. Confirm the blast door still opens, the objective becomes extraction, and the extraction pad completes the mission.
7. Restart Play Mode and confirm the counter resets to `AETHER: 0`.
8. Confirm the Console has no warnings or errors.

## Pass Criteria
Prototype 11 passes when collecting the existing Aether sample increments one clear runtime counter without changing the passed mission loop.
