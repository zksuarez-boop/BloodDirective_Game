# Act 1 Production 07 - Bunker Intake Content Data

## Purpose
Move the approved Bunker Intake route structure into editable Unity content records while preserving the passed mission scene and behavior.

## Scene Promotion
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeRoute01.unity`
- Production scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeContent01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 07 - Bunker Intake Content Data`

## Content Records
The level record `Act1_BunkerIntakeRoute.asset` now owns these ordered room records:
1. Bunker Intake: mission entry.
2. Service Corridor: first Grey, linked to Intake Security.
3. Logistics Bay: scientist and calibration return, linked to Containment Calibration.
4. Relay Chamber: Aether sample.
5. Extraction Passage: extraction guard and pad, linked to Extraction Guard.

## Data Locations
- Room definition type: `Assets/_BloodDirective/Scripts/Data/RoomDefinition.cs`
- Room assets: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Rooms/`
- Level asset: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Act1_BunkerIntakeRoute.asset`

## Regression Guardrails
- Act 1 Production 06 remains unchanged as the route reference.
- This step does not read room data at runtime yet; it cannot change mission behavior.
- Existing movement, collision, camera, HUD, combat, and visuals are unchanged.

## Acceptance Checklist
- Build while not in Play Mode.
- Confirm the Console is clean after compilation.
- Confirm the five room assets exist and appear in the route level record in order.
- Play the full route and confirm it behaves identically to Act 1 Production 06.
