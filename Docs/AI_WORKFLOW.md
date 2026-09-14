# Blood Directive - AI-Assisted Workflow

## Goal
Use AI heavily while keeping Unity as the source of truth and avoiding uncontrolled project sprawl.

## Workflow
1. Define the requested change clearly.
2. Confirm scope and files/systems affected.
3. Get explicit approval before edits.
4. Implement the smallest useful change.
5. Test or provide exact test steps.
6. Summarize changed files, result, and next recommended step.

## Implementation Audit
Before implementation, answer:
- What player behavior are we proving?
- What existing behavior must not regress?
- What genre expectation does this support?
- What are we explicitly not adding yet?
- What files, scenes, or systems are expected to change?
- How will the user test it in Unity?
## Tool Roles
Unity:
- scenes
- GameObjects
- physics
- rendering
- animation
- UI
- builds

Codex:
- architecture
- scripts
- docs
- debugging
- implementation plans
- test checklists

AI image/model tools:
- concept art
- character direction
- texture ideas
- prototype assets

External assets:
- only with clear license tracking
- only if they match art direction

## Current Reboot Priority
1. Project documentation.
2. Movement prototype.
3. Camera prototype.
4. One authored bunker hallway.
5. Interaction prototype.
6. One enemy group.
7. One Aether pickup.

## Automation
Unity CLI or other automation tools may be added later, but only after the reboot project has a stable first prototype.
