# Act 1 Production 06 - Bunker Intake Route

## Purpose
Promote the validated five-space mission route into an Act 1 production scene without changing its approved gameplay.

## Scene Promotion
- Source: `Assets/_BloodDirective/Scenes/Prototypes/ExpandedMissionRoutePrototype01.unity`
- Production scene: `Assets/_BloodDirective/Scenes/Act1/Act1_BunkerIntakeRoute01.unity`
- Build menu: `Blood Directive > Reboot > Build Act 1 Production 06 - Bunker Intake Route`

## Approved Route
1. Intake: player begins in the original Bunker Intake.
2. Service Corridor: first Grey blocks progress.
3. Logistics Bay: rescue the scientist.
4. Relay Chamber: collect the Aether sample after the first Grey is neutralized.
5. Logistics Bay: return to the scientist for calibration.
6. Extraction Passage: defeat the extraction guard and reach the extraction pad.

## Production Data
- Level asset: `Assets/_BloodDirective/ScriptableObjects/Levels/Act1/Act1_BunkerIntakeRoute.asset`
- Level ID: `act1_bunker_intake_route`
- Biome: `underground_bunker`
- Room count: `5`
- Mission: `Act1_BunkerIntake.asset`

## Regression Guardrails
- Expanded Mission Route Prototype 01 remains unchanged as the reference scene.
- Existing mission logic, movement, collision, camera, HUD placement, and visuals are unchanged.
- `VisualOnly` objects do not own gameplay collision.

## Acceptance Checklist
- Build the production scene from the Blood Directive menu while not in Play Mode.
- Confirm the Console is clean after compilation.
- Confirm the five-space route, gates, combat, scientist dependency, Aether collection, and extraction all behave as in the passed prototype.
