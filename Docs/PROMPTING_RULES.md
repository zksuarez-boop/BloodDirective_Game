# Blood Directive - Prompting Rules

## Core Prompt Rule
Every change request must be specific enough to test.

Bad:
Make it look better.

Good:
In the Bunker Breach test scene, replace the flat grey floor with dark modular concrete panels, subtle seams, grime decals, and low roughness variation. Do not change player movement, camera, enemies, UI, or scene objectives. Test that click movement still works.

## Change Request Format
Target:
[Scene / system / file / feature]

Goal:
[What should be different after this change?]

Player action:
[What does the player do?]

Expected result:
[What should happen?]

Do not change:
[Systems or content that should stay untouched.]

Constraints:
[Unity version, input rules, style rules, naming rules, multiplayer concerns, etc.]

Test:
[How to verify it works.]

## Build Prompt Rule
Build prompts should target one vertical slice or one isolated system. Do not ask AI to build the whole game in one prompt.

## Bug Prompt Rule
One bug at a time. Include actual result, expected result, reproduction steps, logs, suspected system, and what must not change.

## Art Prompt Rule
Every art prompt must include:
- role of asset
- pose/composition requirement
- camera/readability requirement
- palette/material direction
- exclusions: no logos, no flags, no readable real-world identifiers

## Approval Rule
Design discussion does not equal approval to edit files. Approval must be explicit.
