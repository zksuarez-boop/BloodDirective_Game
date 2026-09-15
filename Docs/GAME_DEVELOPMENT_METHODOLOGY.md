# Blood Directive - Game Development Methodology

## Purpose
This document defines how Blood Directive should be built from a best-practice standpoint. The goal is disciplined progress: build the smallest useful thing, verify it, then expand.

## Core Principle
Gameplay systems must be reliable before visual complexity is added.

For this project, the most important example is movement:

Player movement must be controlled by clean authored gameplay collision, not by detailed visual art meshes.

## Why This Matters
The previous project became difficult to test because movement, visuals, generated layout, NavMesh, character models, combat, and mission systems were changed in overlapping passes. When something broke, the cause was ambiguous.

The reboot avoids that by separating:

1. Gameplay collision
2. Visual art
3. Testing/debugging
4. Feature systems

## Layering Rule
### Gameplay Collision Layer
This layer controls what the player can do.

Examples:
- walkable floor areas
- wall blockers
- closed-door blockers
- interaction zones
- enemy body capsules

This layer should be simple, readable, and intentionally authored.

### Visual Art Layer
This layer controls how the game looks.

Examples:
- floor meshes
- wall meshes
- props
- pipes
- lights
- decals
- ceiling hints
- imported asset-pack geometry

Visual art should not control movement by default.

### Navigation/Test Layer
This layer helps verify the game is still playable.

Examples:
- walkable area debug view
- blocker debug view
- movement regression checklist
- camera visibility checklist
- bug report templates

## Movement Methodology
Movement is the first production-quality system because every other playable feature depends on it.

Required movement standard:
- left-click moves to visible floor
- character rotates toward travel direction
- walls and closed doors block movement
- visual-only props do not trap the player
- camera follows without becoming a ceiling view
- movement works before combat, loot, or story scripting is added

## Visual Asset Import Rule
Imported visual assets must not be allowed to define gameplay collision automatically.

Default behavior:
- imported props start as visual-only
- imported floors are reviewed before becoming walkable
- imported walls are reviewed before becoming blockers
- generated/complex mesh colliders are avoided for player movement
- simplified colliders are authored separately when needed

## Recommended Technical Pattern
Player click -> raycast against Walkable gameplay floor -> move controller target -> capsule checks Blocker layer -> visual meshes stay decorative unless intentionally assigned.

## Engine-Agnostic Best Practice
This applies in both Unity and Unreal Engine.

Unity equivalent:
- layers
- raycasts
- simple colliders
- CharacterController or controlled transform movement
- NavMesh only after source geometry is clean

Unreal equivalent:
- collision channels
- simple collision
- blocking volumes
- navmesh bounds
- character movement component

The principle is the same: detailed art is not the source of truth for gameplay collision.

## Project Phases
### Phase 0 - Project Constitution
Create rules, story, art direction, vertical slice scope, prompting rules, bug templates, playtest templates, and asset rules.

### Phase 1 - Core Feel Prototype
Build one scene with one player placeholder, click-to-move, wall blocking, and camera follow. No combat, loot, story triggers, procedural maps, or imported character art.

### Phase 2 - Greybox Vertical Slice
Build one authored bunker route: intake threshold, corridor, checkpoint, scientist area, breach hallway, extraction door.

### Phase 3 - Interaction Prototype
Add left-click interaction, scientist placeholder, door interaction, and objective text.

### Phase 4 - Combat Prototype
Add one basic attack, one Short Grey group, one stronger Grey variant, health, death, and hit feedback.

### Phase 5 - Aether Prototype
Add Aether pickup, Aether counter, scientist explanation, and a simple upgrade placeholder.

### Phase 6 - Visual Pass
Add bunker materials, lighting, pipes, doors, props, post-processing, and mood. After every visual pass, rerun movement and camera tests.

### Phase 7 - Character Asset Pass
Import character art only after movement works. Use script-driven movement with in-place idle/walk/run first. Keep root motion disabled until there is a clear reason to enable it.

### Phase 8 - Enemy Visual Pass
Replace enemy placeholders with readable Short Grey and Tall Grey models. Keep enemy gameplay colliders simple.

### Phase 9 - Act 1 Expansion
Expand only after the vertical slice feels good. Add more bunker sections, enemy types, story events, Aether upgrades, and boss encounters.

### Phase 10 - Multiplayer Planning
Design with future 1-8 player co-op in mind, but do not implement networking until the single-player vertical slice is stable and fun.


## Implementation Audit Rule
Before each new prototype, vertical-slice feature, or asset integration pass, complete a short implementation audit.

Required questions:
- What player behavior are we proving?
- What existing behavior must not regress?
- What genre expectation does this support?
- What are we explicitly not adding yet?
- What files, scenes, or systems are expected to change?
- How will the user test it in Unity?

The audit keeps AI-assisted development from drifting into broad, overlapping edits. If the audit cannot be answered clearly, the next step is not ready to build.
## Approval Gates
Before each implementation phase, define:
- target scene/system
- exact goal
- player action
- expected result
- what must not change
- test procedure

No phase begins until explicitly approved.

## Regression Rule
After every change, verify the previous core behaviors still work.

Current core regression list:
- compile has no errors
- click-to-move works
- walls block movement
- camera keeps player visible
- objective can be completed if present
- no unrelated systems were changed

## Decision Rule
When choosing between fast and stable, choose stable for foundational systems.

Fast iteration is valuable, but unstable foundations produce wasted work.
