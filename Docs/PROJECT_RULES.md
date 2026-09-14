# Blood Directive Reboot - Project Rules

## Core Rule
Codex must not create, edit, delete, move, rename, import, or reconfigure project files unless the user explicitly approves the exact task.

Approved wording examples:
- "Approved: create the docs."
- "Approved: build the movement prototype."
- "Approved: edit the camera controller."

If approval is unclear, stop and ask.

## Project Goal
Build Blood Directive as a dark isometric action RPG in Unity 6 URP. The reboot keeps the established story and art direction, but starts from a cleaner implementation.

## Build Philosophy
Design first. Build second. Test before adding more.

The first playable target is not the full game. It is a controlled vertical slice that proves movement, camera, bunker layout, combat readability, and story flow.

## Technical Rules
- Unity 6 URP.
- PC first, Steam target later.
- No ECS/DOTS.
- Use private serialized fields: [SerializeField] private.
- Avoid public fields.
- Prefer simple, dependable systems over broad framework work.
- Avoid procedural generation until the authored vertical slice feels good.
- Preserve future 1-8 player co-op needs, but do not implement multiplayer first.

## Design Rules
- Story drives areas.
- Act 1 is an authored underground bunker escape, not an open-world or random mission structure.
- Movement and camera must work before combat, loot, skill trees, or AI character art.
- The first slice should be compact and playable.
- Do not build systems that are not needed for the current approved slice.

## Art Rules
- Dark, grounded, desaturated, classified underground bunker tone.
- Modern Cold War-era facility, not bright futuristic spaceship.
- Diablo-style readable isometric darkness.
- No real-world military unit names, logos, flags, or readable identifiers.
- Free/imported assets must match the art direction and license requirements.


## Movement And Collision Rule
Gameplay movement uses authored gameplay collision. Visual assets are decorative unless intentionally assigned to gameplay collision.

Required approach:
- player clicks target Walkable gameplay floor
- movement checks against simple Blocker collision
- imported props default to visual-only
- complex visual mesh colliders must not control player movement by default
- roofs and ceiling hints must not block the gameplay camera
- every visual pass requires a movement/camera regression test
## Bug Rule
If a core feature breaks, stop adding features. Fix the broken feature before continuing.

Core blockers:
- movement broken
- camera broken
- compile errors
- player cannot complete test objective
- interaction input broken

## Required End-of-Task Summary
Every recommendation or completed task must include:
- You: what the user does.
- Me: what Codex does.
- Approval needed: what requires explicit approval before proceeding.

