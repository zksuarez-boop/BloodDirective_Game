# Vertical Slice 06 - Playable Demo Validation

## Goal
Package the verified Bunker Breach loop as a single-scene Windows demo target and validate it outside the Unity editor.

## Scene and Build Commands
- Scene: `Assets/_BloodDirective/Scenes/VerticalSlices/BunkerBreachVerticalSlice06.unity`
- Source: `BunkerBreachVerticalSlice05_1.unity`
- Create scene: `Blood Directive > Reboot > Build Vertical Slice 06 - Playable Demo`
- Create Windows build: `Blood Directive > Reboot > Build Windows Playable Demo`
- Build output: `Builds/Windows/BloodDirective_BunkerBreachDemo.exe`

## Build Configuration
- The build settings contain only the Vertical Slice 06 scene.
- Target platform is 64-bit Windows desktop.
- The demo retains the complete gated mission route and replay command from Vertical Slice 05.1.
- No new gameplay, art, camera, or performance-changing feature is introduced in this gate.

## Validation Checklist
- [ ] Unity Console has no warnings or errors after scene generation.
- [ ] The Windows build completes successfully.
- [ ] The executable launches outside the editor.
- [ ] Mouse movement, left-click interactions, space attack, and `R` replay work in the standalone build.
- [ ] The mandatory mission chain cannot be bypassed.
- [ ] UI remains legible at the target desktop resolution.
- [ ] A full rescue-to-extraction run completes in the standalone build.
- [ ] The executable closes cleanly after testing.

## Deferred
- Camera occlusion/cutaway behavior, final audio, final visual assets, settings menu, save system, and Steam packaging.
