# Movement Prototype 09 - Scientist Rescue Gate

## Implementation Audit
- **Player behavior:** rescue the scientist, defeat the Grey, collect Aether, and extract.
- **Regression coverage:** movement, collision, enemy combat, death state, Aether pickup, door opening, and extraction completion.
- **Genre expectation:** a compact story-driven ARPG objective sequence.
- **Not included:** dialogue UI, final scientist art, escort behavior, upgrades, inventory, or mission framework.
- **Changed systems:** one derived scene, scientist interaction state, and the optional Aether prerequisite.
- **Unity test:** build Prototype 09 in Edit Mode, then complete the test below.

## Unity Test Checklist
1. Stop Play Mode and run **Blood Directive > Reboot > Build Movement Prototype 09**.
2. Confirm the Hierarchy root is `MovementPrototype09_Root`.
3. Press Play. Confirm Aether stays dark before the scientist is rescued.
4. Left-click the blue-grey scientist. Confirm the scientist turns green.
5. Defeat the Grey. Confirm Aether turns cyan.
6. Collect Aether, pass through the opened blast door, and activate the extraction pad.
7. Restart and verify that defeating the Grey without rescuing the scientist does not unlock Aether.
8. Confirm the Console has no warnings or errors.

## Pass Criteria
Prototype 09 passes when the scientist-rescue requirement gates the already-passed Aether-to-extraction loop without breaking movement or combat.
