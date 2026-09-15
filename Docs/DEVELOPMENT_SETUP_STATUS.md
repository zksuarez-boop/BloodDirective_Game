# Current verification — 2026-09-15

Functional development setup: PASS. Performance tuning remains outstanding.

- BloodDirectiveEditor Win64 Development compiled and linked successfully in 46.69 seconds.
- Starter map Lvl_TopDown loaded and rendered on the GTX 1060 using DX12.
- User screenshots confirm editor rendering, spawned character, and active Play in Editor.
- User explicitly confirmed left-click movement and obstacle collision, then stopped PIE with Esc. Elevated camera confirmed in screenshots and user checks. These interaction checks were performed by the user, not automated desktop input.
- Saved/Logs/CodexEditorVerification.log confirms PIE world creation, successful play startup and shutdown. Review found no Error: or Fatal entries in this log.
- Screenshot showed approximately 20.6 FPS during PIE; this is a single observation, not a controlled benchmark. Do not mark performance as passed. Profile and tune before expanding content.
- Driver 582.66 initialized successfully. TSR 16-bit optimization warning remains; no driver update performed.
- Z: Zen cache operational. Local Git/LFS setup passed previously; no remote publication.
- Gameplay source and content preserved. Development configuration and instructions are reviewable local changes.

The earlier pending/blocked entries below describe the sequence of setup attempts; they are superseded by this verification summary.
# Development setup status (historical notes below)

- Unreal Engine 5.8.2, changelist 56702186, installed at Z:/Epic Games/UE_5.8.
- MSVC cl.exe 19.50.35727, toolset directory 14.50.35717; Windows SDK 10.0.26100.0 detected by UBT.
- NVIDIA GTX 1060 6GB, driver 582.66 verified with nvidia-smi. Driver was not changed; rendering compatibility remains to be tested.
- UE-LocalDataCachePath = Z:/UnrealDevelopment/Cache. Zen server log explicitly confirms --data-dir Z:/UnrealDevelopment/Cache/Zen and the same DataDir. Cache is on Z:; small Zen executable/logging/tool files may still live on C:.
- Local Git main branch initialized; existing work preserved in commit 0372fbc using explicitly labelled Codex Local Snapshot attribution (no user identity invented or globally configured).
- 482 LFS assets; git lfs fsck passed. No remotes. Binaries, Intermediate, Saved excluded. Source and Content unchanged.
- Project settings disable hardware ray tracing, Lumen GI, mesh distance fields and virtual shadow maps; use screen-space reflections, medium scalability and a 60 FPS cap. Existing DX12/SM6 and Substrate remain intact. Editor background throttling and performance display defaults added. Requires editor restart; visual results not yet verified.
- 2026-09-15: BloodDirectiveEditor Win64 Development build PASSED (46.69 seconds, four compile/link/metadata actions, two parallel actions). MSVC 14.50.35727 and Windows SDK 10.0.26100.0. Log: Saved/Logs/CodexBuildVerification.log.
- UBT warning: Visual Studio SDK not found; Visual Studio editor integration disabled. Build Tools compiler and Windows SDK are present; this is separate from the Live Coding failure.
- Starter map is /Game/TopDown/Lvl_TopDown. Interactive launch/movement/collision verification remains pending. Desktop app-control approval timed out; no visual pass claimed.
- Development changes remain reviewable in git diff. No gameplay features added, no assets deleted, no remote publication.

## 2026-09-15 launch verification
- Closed editor/Live Coding confirmed before successful build.
- Editor launched the project with the existing Top Down map requested. DX12 selected GTX 1060 and driver 582.66. Ray tracing disabled and ZenLocal reports OK, using the cache on Z:.
- First-launch shader compilation is active; user confirmed loading at 43%. Map load and interactive play remain pending.
- Driver warning: TSR 16-bit VALU disabled for NVIDIA drivers older than 610.00; no driver changes performed.
- Desktop capture fails with SetIsBorderRequired: No such interface supported (0x80004002). Text-only accessibility works but supplies no usable viewport controls during loading.


