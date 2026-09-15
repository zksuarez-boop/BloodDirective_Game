# Blood Directive - Unreal Handoff Project Truth

This file is the handoff anchor for continuing Blood Directive on another computer. Read it before proposing designs, generating artwork, changing files, or expanding scope.

## Canon Authority

- `Docs/GAME_DESIGN_BIBLE.md` establishes the premise, core loop, camera, Aether, playable operative, scientist, alien species, and five-act plan.
- `Docs/ACT1_STORY_AND_TEN_LEVEL_MAP_BIBLE.md` is authoritative for Act 1 level order, story reveals, gates, and faction pressure.
- `Docs/ART_DIRECTION_BRIEF.md` and `Docs/ARTWORK_PRODUCTION_PLAN.md` define visual boundaries.
- Older eight-sector bunker docs remain macro-layout references only. They do not override the approved Act 1 opening: Level 01 is Obsidian Briefing Vault, then Level 02 is Containment Hive.

If documents conflict, preserve the later approved ten-level Act 1 bible and ask Zack which version is authoritative.

## Project Truth Summary

Blood Directive is a dark fixed-isometric action RPG about an off-record black-file operative sent into a buried Cold War-era bunker for a manipulated assassination mission. The mission begins in the Obsidian Briefing Vault, where target identities are withheld. A catastrophic breach interrupts the operation and turns it into an escape, rescue, Aether recovery, and evidence mission.

The term `Origin` is not defined as a formal capitalized concept in the current project docs. Do not invent its meaning without approval.

## Established Player And NPCs

- Playable character: the initial black-file operative. Direction: lean, wiry, grounded, survivalist, tactical, dark olive/black/gunmetal, compact armor, practical weapons, no real unit identifiers.
- Key NPC: the scientist. They understand Aether and become the weapon/research upgrade anchor.
- Roadmap note: future player class expansion starts with `Berserkr`, but no origin, kit, or approved visual blueprint exists yet. Treat Berserkr concepts as new ideas unless Zack provides canon.

## Established Alien Factions

- Greys: first active enemy force. Short Greys are scouts, technicians, containment units, and swarm security. Tall Greys are coordinators, researchers, and intervention-protocol controllers. Greys enter through sanctioned Intake and Quarantine systems; their protocol has been altered, making the player, scientist, and Aether work containment threats.
- Reptilians: breach from beneath through a buried Aether fault below the containment complex. Their verified population is 833. Ranks, hierarchy, and named leaders are deferred.
- Nordics: ambiguous later observers, not routine Act 1 allies or enemies.
- Insectoids: later terrifying nonhuman force, not to be substituted for Greys or Reptilians.
- The incoming hostile extraterrestrial power is intentionally unnamed. Do not design its species, logo, emblem, or visual identity.

## Five-Act Plan

1. Act 1: buried Cold War bunker escape.
2. Act 2: remote U.S. desert and first base camp.
3. Act 3: South American jungle investigation.
4. Act 4: water/Atlantis arc.
5. Act 5: Antarctica forbidden-zone descent.

Only Act 1 has detailed level-by-level design in this repo.

## Act 1 Ten-Level Canon

1. Obsidian Briefing Vault: interrupted black-operation briefing, command room, observation gallery, emergency descent shaft, collapsed escape route.
2. Containment Hive: breached holding ring, Reptilian origin through the Aether fault.
3. Geothermal Undercroft: service tunnels, three-reactor arena, fear amplification through systems.
4. Cargo Logistics Depot: scientist rescue.
5. Aether Relay Cathedral: first live Aether sample, Grey protocol turn.
6. Bio-Research Laboratories: records show protocol change before breach.
7. Central Operations Nexus: command abandonment and routing reset.
8. Intake and Quarantine: Grey-controlled sanctioned entry system.
9. Strategic Command Vault: evidence of manipulated operation.
10. Evacuation Hangar: extraction guard, evacuation with scientist and Aether sample.

## Current Unity Prototype Truth

Unity 6 URP prototype contains:

- 12 focused movement/combat/Aether prototypes.
- Vertical Slice 01-06, ending in a Windows playable demo target.
- Act 1 Bunker Intake production scenes and content data.
- Obsidian Briefing Vault blockout.
- Original modular Act 1 bunker prefabs: floor, wall, containment console, pipe, light.
- ScriptableObjects for Act 1 missions, levels, rooms, and encounters.

Validated loop:

1. Left-click movement through authored collision.
2. Rescue scientist.
3. Defeat breach Grey.
4. Collect one Aether sample.
5. Return to scientist and spend Aether for weapon calibration.
6. Defeat activated extraction guard.
7. Pass blast door and step onto extraction pad.
8. Mission complete panel and replay command.

## Unreal Transition Rule

Treat the Unity project as prototype reference and design truth, not as a line-by-line conversion target.

Reuse directly:

- Documents, lore, Act structure, route intent, level measurements as references.
- Concept art and visual direction.
- Original modular bunker-kit idea.
- Collision philosophy and playtest checklists.

Recreate in Unreal:

- Gameplay logic, Blueprints, input, AI, UI, materials, lighting, levels, packaging.
- Click movement, camera, interaction, combat, health, Aether, calibration, extraction.

Replace or source externally:

- Final character models, Grey models, bunker kit meshes, VFX, audio, UI art, animations.
- Use free/marketplace assets only with clear license tracking and approval.

Defer until after MVP:

- Full ten-level Act 1, procedural bunker, multiplayer, crafting, large skill tree, permanent progression, full inventory, save/checkpoints, final dialogue, final bosses, additional playable classes.

Remove for MVP:

- Any scope that does not support the small complete run.

## Approved Unreal Visual Direction

Blood Directive in Unreal should look like a grounded, modern Diablo II-style bunker ARPG:

- Fixed elevated three-quarter camera.
- Realistic 3D materials and dark atmospheric environments.
- Clear player, enemy, door, hazard, loot, and objective readability.
- Cold War bunker construction: concrete, aged steel, pipes, cable trays, strip lights, blast doors, observation glass.
- Alarm red for danger, Aether cyan for rare energy/calibration, warning amber for caution/interactions.
- No bright spaceship corridors, cyberpunk neon wash, fantasy dungeon stone, real flags, real unit identifiers, or unreadable clutter.

## Preview Concept Images

Concept images are stored in `Docs/ConceptArt/`:

- `black_file_operative_sheet.png`
- `grey_enemy_sheet.png`
- `act1_mvp_bunker_gameplay_concept.png`
- `hud_concept.png`

Important caveat: the large extraction creature in the gameplay/HUD concepts is not approved canon. Interpret it as a boss-lite visual mood placeholder or stronger Grey placeholder until Zack approves a boss design.

## MVP Target

MVP target remains:

Start menu -> playable character -> two normal rooms -> enemies -> combat -> loot/equipment placeholder -> boss-lite extraction arena -> visible win state -> restart/quit.

Minimum content:

- Unreal Top Down template foundation.
- Fixed elevated camera.
- One placeholder operative.
- Two dark bunker rooms plus extraction arena.
- Short Grey enemy, plus one stronger Grey/guard variant.
- Health and Aether.
- Basic attack and one tactical ability placeholder.
- Scientist interaction.
- Aether pickup.
- Weapon calibration.
- One loot/equipment placeholder.
- Win state with restart/quit.

Do not expand the bunker before the small complete run works.

## Hardware Constraint

Current development computer:

- Intel i7-3770K
- 32 GB RAM
- NVIDIA GTX 1060 6 GB
- Approximately 500 GB SSD

First Unreal prototype must support scalable settings:

- Optional Lumen during development.
- No reliance on hardware ray tracing.
- Controlled texture sizes, shader complexity, shadows, particles, enemy counts.
- Modular assets and material instances over unique one-off art.

## One-Hour Build Rules

- Each weekday gets exactly one 60-minute objective and one binary Pass test.
- Stop after 60 minutes.
- If test fails, mark Needs Work and record smallest next fix.
- Complete that fix before new scope.
- Never double workload after a missed day.
- Use placeholders until gameplay works.
- Maintain a playable packaged build every Friday.
- Prioritize finishing over feature accumulation.

## Next One-Hour Objective

Create/open the Unreal project foundation from the Top Down template.

Pass test:

- Unreal opens to a Top Down template level.
- Placeholder player can click-move inside one dark rectangular bunker room.
- Walls block movement.
- Camera is fixed elevated three-quarter.
- No compile or Blueprint errors in Output Log.

## Startup Prompt For Desktop Codex

Use this when opening Codex on the desktop machine:

```text
Read Docs/UNREAL_HANDOFF_PROJECT_TRUTH.md and continue Blood Directive from the approved Unreal transition plan. Treat Unity as prototype reference only. Do not redesign canon, invent undocumented characters, invent alien origins, or expand scope beyond the MVP without approval. First objective: create or inspect the Unreal Top Down template foundation and build the one-room click-movement bunker prototype with a binary Pass test.
```
