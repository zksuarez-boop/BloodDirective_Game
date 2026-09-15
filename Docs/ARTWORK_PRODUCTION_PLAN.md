# Blood Directive Artwork Production Plan

## Purpose
This plan corrects the art-production target to the established game canon. It turns the design bible, Act 1 campaign map, vertical-slice HUD, and character direction into an implementable artwork brief without changing gameplay, collision, or story scope.

## Authority and Continuity
- `GAME_DESIGN_BIBLE.md` establishes the game premise, playable operative, scientist, alien roster, Aether role, and five-act structure.
- `ACT1_STORY_AND_TEN_LEVEL_MAP_BIBLE.md` is authoritative for Act 1's ten levels, level order, story reveals, gates, and alien pressure.
- `ACT1_BUNKER_ESCAPE_MAP_CONCEPT.md` and `ACT1_BUNKER_ESCAPE_FLOOR_PLAN_01.md` are earlier eight-sector macro-layout references. They inform the physical bunker, but do not override the ten-level opening: Level 01 is Obsidian Briefing Vault, then Level 02 is Containment Hive.
- `VERTICAL_SLICE_02_FEEDBACK_AND_HUD.md`, `VERTICAL_SLICE_04_CHARACTER_AND_COMBAT_PRESENTATION.md`, and `VERTICAL_SLICE_05_MISSION_POLISH.md` define the proved HUD and presentation baseline.
- When documents conflict, preserve the later approved ten-level story Bible and record a clarification instead of silently redesigning canon.

## Shared Art Grammar
Blood Directive is a dark, readable isometric action RPG: classified Cold War infrastructure, worn concrete, aged steel, military utility systems, and controlled occult-science intrusion. The visual language must support click movement and combat readability before spectacle.

- Use a dark desaturated foundation: bunker black, deep steel blue, cold concrete, gunmetal, utility olive, and aged off-white.
- Reserve alarm red for danger, active failure, damage, and hostile emergency states.
- Reserve Aether cyan for contained energy, calibration, anomaly evidence, and upgraded weapon state.
- Use warning amber for caution, locks, industrial service states, and non-hostile interactable risk.
- Preserve clean playable-floor values and readable actor silhouettes under the fixed isometric camera. Avoid roof mass, busy decals, or prop density that hides targets or click surfaces.
- Modern Cold War infrastructure is the baseline. Do not drift into clean spaceship corridors, cyberpunk neon, fantasy ruins, real-world flags, military patches, logos, or readable unit identifiers.

## Story-Origin Visual Rules
The facility is a classified human research site built around a buried Aether fault. Art must make the two distinct arrivals legible without exposing the hidden human actor prematurely.

- Reptilian breach evidence rises from below: fractured containment infrastructure, displaced heavy hardware, fault glow, damaged seals, and pressure-driven fear effects. It is strongest in the lower bunker.
- Grey intervention enters through sanctioned Intake and Quarantine systems: controlled decontamination devices, orderly medical/technical hardware, intervention beacons, and a sterile administrative language that becomes hostile when its protocol is corrupted.
- The hidden human command layer is shown through redacted operations hardware, selective evacuation, altered records, and withheld access. Do not reveal its identity, logo, or final conspiracy imagery in Act 1.
- Aether connects the breach, scientific research, weapon calibration, and future travel. It is rare and consequential, not ambient neon decoration.

## Character and Alien Blueprint

### Player: Black-File Operative
- Lean, wiry, veteran survivalist silhouette; dark olive, black, and gunmetal clothing; compact armor; pouches, gloves, boots, and a small Aether-device accent.
- The character reads as human and practical from the isometric camera. No superhero armor, giant pauldrons, or real military identifiers.
- Build visual models as visual-only children of the proven gameplay body. Use in-place animation first; keep root motion disabled until movement has an explicit approved change.

### Scientist
- A human research specialist, not a generic civilian or armored soldier. Their silhouette should combine practical emergency research layers, a compact field kit, and a small calibrated Aether instrument.
- Their colors should remain quieter than the player and enemies, with controlled cyan only when interacting with Aether or weapon calibration.
- The scientist is the Act 1 rescue and upgrade anchor; make them instantly distinguishable from both Grey technicians and bunker staff.

### Short Grey
- Thin body, large head, pale grey skin, black eyes, and a readable compact silhouette from above.
- Visual roles: scout, technician, containment worker, and swarm-like security.
- Their sanctioned equipment can use sterile intervention materials and limited technical cyan, but hostile behavior is communicated through pose, eye treatment, and alert states rather than covering the model in red lights.

### Tall Grey
- Taller, more deliberate coordinator with elongated limbs and a restrained Aether glow.
- Visual role: command personnel, research leadership, and intervention-protocol control. Its silhouette must remain clearly distinct from a Reptilian at gameplay distance.

### Reptilian
- Dense, powerful, predatory non-human silhouette with dark scales or plated organic surfaces, oxidized green-grey accents, and restrained Aether contamination.
- Primary Act 1 visual role: lower-level breach force, elite infiltrator, and commander presence.
- Never share Grey intervention gear, Grey head proportions, or sterile Intake markings. The two forces must be identifiable before a player reads HUD text.

### Later Species
- Nordics are ambiguous, distant observers. Do not portray them as routine allies or an Act 1 combat faction.
- Insectoids are a later overwhelming non-human threat. Keep their body language, materials, and silhouettes separate from both Greys and Reptilians.
- The coming hostile extraterrestrial power remains unnamed. No final species design, insignia, or hero asset should be commissioned until narrative approval establishes it.

## Act Environment Blueprint

### Act 1: Buried Cold War Bunker Escape
The full act climbs from deepest classified research to a visible evacuation. Reptilian pressure is strongest below; Grey control is strongest above.

| Level | Landmark and art purpose | Dominant pressure |
|---|---|---|
| 01 Obsidian Briefing Vault | Secure command room, observation gallery, collapsed infiltration route, emergency descent shaft. Establish secrecy before the breach. | Human command failure, first breach |
| 02 Containment Hive | Circular holding ring, central rupture, cell pods, survivor corridor. | Reptilian origin |
| 03 Geothermal Undercroft | Pipe tunnels, three-reactor arena, maintenance loop, freight lift. | Industrial fear amplification |
| 04 Cargo Logistics Depot | Cargo floor, stacked cover, loading offices, scientist holding area. | Human operations and rescue |
| 05 Aether Relay Cathedral | Octagonal relay arena, tall machinery, outer maintenance ring. | Aether discovery and Grey protocol turn |
| 06 Bio-Research Laboratories | Observation corridors, experiment rooms, specimen archive, diagnostics bay. | Human research evidence |
| 07 Central Operations Nexus | Circular atrium, lifts, spoke corridors, distant views toward escape. | Command abandonment |
| 08 Intake and Quarantine | Decontamination lanes, checkpoints, administration, quarantine bypass. | Grey sanctioned intervention turned hostile |
| 09 Strategic Command Vault | Secure data vault, planning rooms, server chamber, escape corridor. | Proof of manipulated operation |
| 10 Evacuation Hangar | Aircraft bay, cover lanes, catwalks, visible craft, final extraction pad. | Contested evacuation |

Do not make all ten levels a recolored corridor. Each level needs its own silhouette, lighting behavior, landmark, and story-facing prop set while sharing the same bunker construction kit.

### Act 2: Remote U.S. Desert and First Base Camp
Shift from buried secrecy to exposed field operations: wind-worn utility structures, temporary research camp, perimeter lights, portable Aether instruments, and distant terrain for navigation. Keep the setting grounded, austere, and free of real-world military branding.

### Act 3: South American Jungle Investigation
Use dense humid vegetation, degraded expedition infrastructure, archaeological/field-research traces, low-visibility traversal, and ancient Aether evidence. The environment must feel researched and dangerous, not a generic fantasy temple.

### Act 4: Water and Atlantis Arc
Establish pressure, flooded systems, submerged structures, reflective water, and ancient non-human scale. Maintain practical traversal contrast and strong combat-floor readability; water effects must not obscure threat silhouettes.

### Act 5: Antarctica Forbidden-Zone Descent
Use extreme cold, wind-carved exterior installations, buried access shafts, ice-bound research architecture, and the final descent into forbidden infrastructure. It should feel austere and hostile, not bright snow tourism.

## HUD and Interaction Artwork Blueprint
The HUD must retain the proved vertical-slice information contract while receiving final art treatment.

- Top-left: vitality, Aether count, and weapon state. Vitality is red for injury/critical condition; Aether is cyan; the weapon label communicates Standard versus Aether Calibrated with text and icon/material change, not color alone.
- Top-center: current objective, concise and state-driven. It must not reveal target identities or the human conspiracy before the story does.
- Bottom-center: contextual instruction for the current verified interaction or combat state. It disappears when no prompt is relevant.
- World space: health bars only over active enemies, with clear defeat removal. Interactable and ally identifiers must never visually compete with enemy health.
- Mission status: operation title, non-blocking deployment/status callouts, elapsed run timer, and a restrained mission-complete panel with replay command.
- Typography: compact classified-operations hierarchy, high contrast, no tiny decorative labels, no faux terminal paragraphs, and no essential information communicated solely by red/cyan/amber color.
- HUD art must be resolution-safe, readable at the isometric camera scale, and tested against dark bunker scenes, bright exterior scenes, Aether cyan, and alarm-red lighting.

## Asset and Technical Requirements
- Every imported, bought, or AI-generated asset is recorded in `ASSET_LEDGER.md` before integration with source/tool, creator, license, date, usage, and notes.
- Art meshes default to `VisualOnly`. Authored Walkable and Blocker collision remains the movement authority.
- Imported floors, walls, and props need a collision and camera-occlusion review before they are allowed to affect gameplay.
- Use modular kits per act: floor, wall, transition, door, cover, landmark, light, pipe/cable, signage surface without readable real-world identifiers, and prop families.
- Create silhouette turnarounds, material references, and isometric readability captures before requesting high-detail production models.
- Do not commission final models for unapproved names, ranks, bosses, enemy counts, reward tables, or the unnamed arriving species.

## Delivery Order
1. Lock the Act 1 reference board, color script, modular bunker kit, and landmark set for Levels 01 through 10.
2. Finalize player, scientist, Short Grey, Tall Grey, and Reptilian silhouette sheets before high-detail production.
3. Produce the HUD component library using the proven vitality, Aether, weapon, objective, prompt, health-bar, timer, and completion states.
4. Create Act 2 through Act 5 visual bibles and modular kits only after the game design adds their level-specific story, enemies, and interaction requirements.
5. Integrate assets in isolated passes with movement, camera, interaction, combat, and HUD regression checks after each pass.

## Acceptance Checklist
- [ ] Act 1 begins in Obsidian Briefing Vault and visually progresses through all ten approved levels.
- [ ] Reptilian breach origin and Grey sanctioned entry are visually distinct and story-correct.
- [ ] Player, scientist, Short Grey, Tall Grey, and Reptilian silhouettes are distinguishable from the gameplay camera.
- [ ] Art supports, rather than spoils, the hidden human actor and unnamed arriving species.
- [ ] HUD preserves the validated vertical-slice information and uses accessible redundant cues.
- [ ] Imported and generated art is licensed, ledgered, visual-only by default, and passes movement/camera regression.
