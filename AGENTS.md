# BloodDirective development instructions

## Scope and canon
This is the Unreal 5.8 C++ Top Down starter project. Preserve existing template variants, maps, assets, and gameplay. Do not add gameplay features unless requested. Never publish, push, or add a remote without explicit authorization.

Read C:/Users/anony/Documents/BloodDirective_Game/Docs/UNREAL_HANDOFF_PROJECT_TRUTH.md first for design work. Its referenced GAME_DESIGN_BIBLE.md, ACT1_STORY_AND_TEN_LEVEL_MAP_BIBLE.md, ART_DIRECTION_BRIEF.md and ARTWORK_PRODUCTION_PLAN.md govern canon. The ten-level Act 1 bible overrides old eight-sector layouts. Unity is reference only. Do not invent missing lore or promote concept-image bosses to canon.

## Structure
- BloodDirective.uproject: engine association, runtime module, plugins.
- Source/BloodDirective/: main C++ module, Top Down classes, preserved Strategy/TwinStick variants.
- Source/BloodDirectiveEditor.Target.cs: editor target.
- Config/: project defaults and hardware-conscious development settings.
- Content/TopDown/Lvl_TopDown.umap: existing starter map; default startup/game map.
- Content/: Unreal binary assets, tracked with Git LFS.
- Saved/, Intermediate/, Binaries/: generated and ignored. Build/ is not blanket-ignored because it can contain packaging resources.

## Local machine
Engine: Z:/Epic Games/UE_5.8 (5.8.2).
Project: Z:/UnrealDevelopment/Projects/BloodDirective.
Hardware target: i7-3770K, 32 GB RAM, GTX 1060 6GB.
UE-LocalDataCachePath should be Z:/UnrealDevelopment/Cache. Confirm actual Zen cache location in runtime logs; an environment variable alone is not proof.
Keep ray tracing, Lumen and virtual shadow maps disabled for this workstation; medium scalability and 60 FPS cap are development defaults. Keep DX12/SM6 and existing material support unless testing establishes a need to change them. Do not globally change driver, engine, or user settings without task authorization.

## PowerShell commands
Close the project editor before a normal C++ build; save work first. Do not terminate an editor with unsaved work.

```powershell
& 'Z:/Epic Games/UE_5.8/Engine/Build/BatchFiles/Build.bat' BloodDirectiveEditor Win64 Development '-Project=Z:/UnrealDevelopment/Projects/BloodDirective/BloodDirective.uproject' -WaitMutex -MaxParallelActions=2
& 'Z:/Epic Games/UE_5.8/Engine/Binaries/Win64/UnrealEditor.exe' 'Z:/UnrealDevelopment/Projects/BloodDirective/BloodDirective.uproject'
git status --short
git lfs fsck
git lfs ls-files
```

## Verification
1. Build BloodDirectiveEditor Win64 Development and record compiler/SDK versions and result.
2. Open /Game/TopDown/Lvl_TopDown; check missing assets, Blueprint errors, driver warnings, and the actual DDC/Zen path in Saved/Logs.
3. Play in Editor: verify player spawns, click movement works, obstacles block movement, and camera remains elevated. Stop PIE and check Output Log for errors.
4. Distinguish headless map-load checks from real GPU rendering and interactive playtests. Never claim interaction verification from compilation alone.
5. Inspect git diff and ensure no unintended Source/Content changes. Keep generated files out of Git. Verify LFS pointers and objects before commits. Do not invent the user's Git identity.
6. Work in one-hour objectives, record pass/fail and smallest next fix; do not expand scope after a failure.
