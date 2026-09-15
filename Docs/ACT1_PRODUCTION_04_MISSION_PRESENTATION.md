# Act 1 Production 04 - Mission Briefing and Objective Presentation

## Purpose
Promote the passed encounter-production scene into the opening Act 1 mission presentation. The new briefing panel reads from the existing `Act1_BunkerIntake` mission asset; it does not create a separate mission-state system.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeMission01.unity`
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeEncounter01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 04 - Bunker Intake Mission Presentation`

## Presentation Behavior
- At run start, a six-second upper-right panel displays the Act 1 mission title and briefing.
- The passed objective, instruction, status, timer, and completion panels remain the only live mission-state presentation.
- Briefing copy comes from `Assets/_BloodDirective/ScriptableObjects/Missions/Act1_BunkerIntake.asset`.

## Regression Contract
- Act 1 Production 03 remains unchanged.
- No route, combat, AI, collision, door, extraction, objective-state, or camera behavior changes.
- No new input, dialogue, loot, checkpoint, or save flow is introduced.
- The briefing uses unscaled time and a single lightweight UI update until it hides.

## Test Checklist
- [ ] Unity Console is clear after build and a normal run.
- [ ] Act 1 Production 03 source scene remains unchanged.
- [ ] The Bunker Intake briefing appears at run start and hides after roughly six seconds.
- [ ] Existing objective/instruction text still updates through every mission beat.
- [ ] The full rescue-to-extraction route completes normally.
