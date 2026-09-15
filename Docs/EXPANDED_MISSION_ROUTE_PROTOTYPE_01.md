# Expanded Mission Route Prototype 01

## Purpose
Validate whether the larger five-space Bunker Intake layout produces a purposeful mission route using only systems that have already passed.

## Prototype Scene
- Scene: `Assets/_BloodDirective/Scenes/Prototypes/ExpandedMissionRoutePrototype01.unity`
- Source: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeExpansion01.unity`
- Build menu: `Blood Directive > Reboot > Build Expanded Mission Route Prototype 01`

## Route
1. Start in Bunker Intake.
2. Encounter the first Grey in East Service Corridor and rescue the Scientist in Logistics Bay.
3. Defeat the first Grey before attempting to collect Aether.
4. Recover Aether from Relay Chamber.
5. Return to the Scientist in Logistics Bay for calibration.
6. Return through the original bunker route.
7. Defeat the existing extraction Grey and reach the extraction pad.

## Explicitly Unchanged
- All player, Grey, scientist, Aether, calibration, door, extraction, HUD, and camera components.
- Enemy values and AI tuning.
- Collision geometry, room count, lighting, and visual layout.
- UI placement and copy.

## Test Checklist
- [ ] Unity Console is clear after build and a normal run.
- [ ] Act 1 Production 05 source scene remains unchanged.
- [ ] The first Grey is encountered in East Service Corridor and remains within readable combat distance of the route.
- [ ] Scientist rescue occurs in Logistics Bay.
- [ ] Aether is reachable in Relay Chamber only after scientist rescue and first Grey defeat.
- [ ] Returning to the Scientist calibrates the weapon and opens the existing extraction access.
- [ ] The existing extraction Grey and pad complete the mission normally.
- [ ] The expanded travel distance feels purposeful rather than empty.
