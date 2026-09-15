# Act 1 Production 08 - Obsidian Briefing Vault Blockout

## Purpose
Create the first approved Unity blockout from the ten-level Act 1 campaign bible without changing the validated Bunker Intake route or its playable demo.

## Production Scene
- Scene: `Assets/_BloodDirective/Scenes/Act1/Act1_ObsidianBriefingVault01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 08 - Obsidian Briefing Vault Blockout`

## Blockout Route
1. Obsidian Command Chamber: black-operation briefing space and first landmark.
2. Secure Briefing Corridor: long controlled transition that establishes facility scale.
3. Observation Gallery: broad visual breach-cascade beat.
4. Emergency Concourse and Descent Shaft: forced route into Containment Hive.
5. Collapsed Escape Route: substantial optional dead-end that establishes the destroyed original infiltration path.

The critical route is intentionally several hundred metres long. Its final target is a three-minute Level 01 playtime, with combat, briefing, and breach content supplying the remaining pacing beyond traversal.

## Procedural Layout
- The vault is generated at runtime from rectangular floor tiles.
- Each run retains the blueprint's command vault, observation gallery, security transition, collapsed infiltration wing, and emergency descent-shaft landmarks.
- Every run has a guaranteed command-room-to-descent-shaft route.
- The tile connections, loops, and optional branches vary while those landmark spaces stay in their intended progression.
- The unseeded generator uses a new layout each time Play Mode begins. A fixed seed can be supplied later for playtest reproduction.

## Content Records
- Mission asset: `Assets/_BloodDirective/ScriptableObjects/Missions/Act1_ObsidianBriefingVault.asset`
- Level asset: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Act1_ObsidianBriefingVault.asset`
- Room assets: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Rooms/Act1_Obsidian*.asset`

## Ownership Rules
- `Walkable` and `Blocker` objects own collision.
- Visual meshes, lights, and landmark detail use `VisualOnly` and do not own collision.
- This is a layout and data pass. It adds no mission runtime, enemy, dialogue, checkpoint, or save behavior.
- A minimal inherited click-movement player and isometric camera are included solely to validate traversal of the blockout.

## Regression Checklist
- [ ] The Bunker Intake production scenes remain unchanged.
- [ ] The Bunker Breach playable demo remains unchanged.
- [ ] The scene contains all four planned spaces and can be inspected without missing material references.
- [ ] The level record reports four ordered room records and references the Obsidian Briefing Vault mission.
