# Movement Prototype 10 - Objective Display and State Feedback

## Implementation Audit
- **Player behavior:** follow the mission objective through rescue, combat, Aether recovery, and extraction.
- **Regression coverage:** all Prototype 09 gameplay gates remain unchanged.
- **Genre expectation:** an ARPG player can read the current objective without guessing from world-state colors alone.
- **Not included:** quest journal, dialogue, cinematic text, inventory, full UI framework, or final UI art.
- **Changed systems:** one derived scene and a lightweight screen-space objective display.
- **Unity test:** build Prototype 10 in Edit Mode, then run the objective sequence below.

## Unity Test Checklist
1. Stop Play Mode and run **Blood Directive > Reboot > Build Movement Prototype 10**.
2. Confirm the Hierarchy root is `MovementPrototype10_Root`.
3. Press Play. Confirm the top display reads `OBJECTIVE: Rescue the scientist`.
4. Rescue the scientist. Confirm it changes to `OBJECTIVE: Neutralize the Grey`.
5. Defeat the Grey. Confirm it changes to `OBJECTIVE: Recover Aether`.
6. Collect Aether. Confirm it changes to `OBJECTIVE: Reach extraction`.
7. Reach the extraction pad. Confirm it reads `MISSION COMPLETE`.
8. Confirm movement, collision, attack, player death rules, and Console cleanliness still pass.

## Pass Criteria
Prototype 10 passes when the objective display accurately reflects every completed state in the existing Act 1 loop without altering its gameplay behavior.
