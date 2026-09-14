# Act 1 Bunker Escape Map Concept

## Design Intent
Create a believable underground research facility that plays as a connected isometric action-RPG level. The player begins in its most secret lower depths after a breach, then fights upward through the facility's hidden hierarchy toward a visible evacuation. The map is a three-depth bunker hive, large enough for eight people to map in parallel while remaining one coherent place.

This is a visual macro-map concept. The ten-level campaign sequence, story gates, and mission details live in `ACT1_STORY_AND_TEN_LEVEL_MAP_BIBLE.md`. Neither document is a Unity build specification yet.

## Facility Sectors

### Lower Depths: breach origin
1. Containment Hive: circular cells, breached specimen infrastructure, and the Reptilian incursion point.
2. Service Tunnels and Geothermal Plant: pipe-heavy pressure spaces connecting the containment breach to the operational bunker.

### Mid Depths: hidden operations
3. Cargo Logistics Depot: open industrial floor with cargo cover; the scientist is found here.
4. Aether Relay Cathedral: a tall, memorable research chamber that holds the Aether sample.
5. Research Laboratories: observatories, analysis suites, and story-facing science spaces.

### Upper Depths: escape and exposure
6. Central Operations Nexus: a vertical command atrium, the navigation anchor, and the point where the facility's compromised command becomes clear.
7. Intake and Quarantine: the sanctioned upper-access point through which Grey intervention forces arrived.
8. Evacuation Hangar: a broad final arena with the extraction pad and guard encounter.

## Breach Premise
The Reptilians do not enter through the public-facing bunker entrance. They exploit a buried Aether fault below the containment complex, where the facility has been studying an ancient subterranean signal. The fault opens into the lowest classified level, putting the first breach beyond normal security and beneath the areas most personnel even know exist.

The Greys arrive separately through the facility's sanctioned quarantine and intake systems. Officially, they are an emergency intervention force responding to the Reptilian breach. In practice, their command protocol has been compromised or manipulated, so they treat the player, the scientist, and the Aether research as threats. This creates two readable enemy pressures: Reptilians are the invasive force from below; Greys are the apparently helpful force that becomes hostile inside the bunker.

## Mapper Ownership Model
Each numbered sector is a planning and ownership boundary, not a loading boundary. A mapper owns the room kit, landmark, combat-space plan, side-route plan, and connection stubs for one sector. Shared standards keep every sector coherent:

- Operations Nexus owns the primary vertical connections and global wayfinding.
- Every sector must connect through at least two planned entrances or exits when practical.
- Each sector has one signature landmark and one distinct lighting identity.
- Main-path gates and shortcuts are designed at the borders between sectors, then reviewed together.
- The complete facility is validated as one uninterrupted player route.

## First Playable Route
1. Begin in the breached Containment Hive, with the Reptilian incursion already underway.
2. Escape upward through Service Tunnels and Geothermal Plant, encountering the first hostile Grey response team.
3. Reach Cargo Logistics Depot and rescue the scientist.
4. Reach the Aether Relay Cathedral, recover the sample, and learn that the intervention protocol is compromised.
5. Return to Logistics for calibration through a newly opened shortcut.
6. Pass through Research Laboratories and Central Operations Nexus, where the full command failure becomes visible.
7. Fight through Intake and Quarantine, now controlled by Grey intervention forces.
8. Enter Evacuation Hangar, defeat the extraction guard, and activate the evacuation pad.

## Target Playtime
The first full route targets approximately 45 minutes for a group of eight players who explore at a normal pace. The eight sectors are not equal-duration rooms; they are a shared production structure.

- Containment Hive: 6 minutes.
- Service and Geothermal: 6 minutes.
- Logistics Depot: 5 minutes.
- Aether Relay: 6 minutes.
- Research Laboratories: 5 minutes.
- Operations Nexus: 5 minutes.
- Intake and Quarantine: 5 minutes.
- Evacuation Hangar: 7 minutes.

The remaining time is travel, side-route exploration, interaction, shortcuts, and recovery between encounters.

## Combat Spaces
- Containment Hive: fractured circular cells and breached holding systems; first close-range Reptilian pressure.
- Service Tunnels: close-range patrol pack; corners and pipe cover teach movement.
- Logistics Bay: medium arena with cargo cover and a rescue interaction under pressure.
- Research Laboratories: narrow observation corridors and controlled line-of-sight encounters.
- Operations Hub: central landmark, light combat, and a clear route to the upper-level exit.
- Aether Relay: circular arena with line-of-sight breaks around the relay machinery.
- Intake and Quarantine: a narrow Grey-controlled push toward the final exit door.
- Evacuation Hangar: broad final arena, room to kite enemies, obvious exit landmark.

## Map Rules
- Every major wing needs a unique silhouette, lighting color, and landmark.
- Main path remains readable from the isometric camera without roofs or tall walls hiding enemies.
- At least one shortcut should reduce the return trip after Aether collection.
- Side routes should offer future rewards or optional encounters, never dead-end confusion.
- Doors create pacing and story gates, not arbitrary room-by-room loading screens.
- Combat spaces alternate between tight pressure, medium cover, and broad movement arenas.

## Not Yet Decided
- Exact room count and square-meter scale.
- Total enemy count and pack composition.
- Optional containment-wing reward.
- Final environmental art kit, materials, lighting, and props.
- HUD placement and final camera framing.

## Approval Gate
Review the visual concept and this route before any Unity blockout work. The next build should be one intentional map blockout based on the approved shape, not another prototype-room expansion.
