using System;
using System.Collections.Generic;
using UnityEngine;

namespace BloodDirective.Act1
{
    /// <summary>
    /// Builds a playable rectangular-tile vault at runtime. Each run has a guaranteed main route,
    /// loop routes, and optional branches while preserving the command-room to descent-shaft flow.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ProceduralVaultLayoutGenerator : MonoBehaviour
    {
        [SerializeField] private Material _floorMaterial;
        [SerializeField] private Material _insetMaterial;
        [SerializeField] private Material _wallMaterial;
        [SerializeField] private Material _trimMaterial;
        [SerializeField] private Material _cyanMaterial;
        [SerializeField] private Material _warningMaterial;
        [SerializeField] private Transform _player;
        [SerializeField] private int _gridWidth = 32;
        [SerializeField] private int _gridHeight = 46;
        [SerializeField] private Vector2 _tileSize = new Vector2(8f, 6f);
        [SerializeField] private int _seed;

        private const string WalkableLayer = "Walkable";
        private const string BlockerLayer = "Blocker";
        private const string VisualOnlyLayer = "VisualOnly";

        private bool[,] _tiles;
        private System.Random _random;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            _random = _seed == 0
                ? new System.Random(Environment.TickCount ^ Guid.NewGuid().GetHashCode())
                : new System.Random(_seed);
            _tiles = new bool[_gridWidth, _gridHeight];

            Vector2Int start = new Vector2Int(_gridWidth / 2, 3);
            Vector2Int gallery = new Vector2Int(_random.Next(7, _gridWidth - 7), _gridHeight / 3);
            Vector2Int security = new Vector2Int(_random.Next(6, _gridWidth - 6), _gridHeight * 2 / 3);
            Vector2Int exit = new Vector2Int(_random.Next(6, _gridWidth - 6), _gridHeight - 5);
            int collapseSide = _random.Next(0, 2) == 0 ? -1 : 1;
            Vector2Int collapsedWing = Clamp(new Vector2Int(start.x + collapseSide * _random.Next(8, 13), _random.Next(7, 15)));

            CarveRoom(start, 7, 5);
            CarveRoom(gallery, 6, 5);
            CarveRoom(security, 4, 4);
            CarveRoom(exit, 7, 7);
            CarveRoom(collapsedWing, 5, 4);

            List<Vector2Int> mainPath = new List<Vector2Int>();
            AppendPath(mainPath, CarvePath(start, gallery, 1));
            AppendPath(mainPath, CarvePath(gallery, security, 1));
            AppendPath(mainPath, CarvePath(security, exit, 1));
            CarvePath(start, collapsedWing, 0);
            CarveLoops(mainPath);
            CarveOptionalBranches(mainPath);
            BuildTileMap(start, gallery, security, collapsedWing, exit);

            if (_player != null)
                _player.position = ToWorld(start) + Vector3.up;
        }

        private static void AppendPath(List<Vector2Int> destination, List<Vector2Int> segment)
        {
            if (segment.Count == 0)
                return;

            int startIndex = destination.Count == 0 ? 0 : 1;
            for (int index = startIndex; index < segment.Count; index++)
                destination.Add(segment[index]);
        }

        private List<Vector2Int> CarvePath(Vector2Int start, Vector2Int destination, int halfWidth)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int current = Clamp(start);
            int safety = _gridWidth * _gridHeight * 4;

            while (current != destination && safety-- > 0)
            {
                CarveWide(current, halfWidth);
                path.Add(current);

                Vector2Int toDestination = destination - current;
                Vector2Int step;
                if (_random.NextDouble() < 0.72d)
                {
                    if (Mathf.Abs(toDestination.y) >= Mathf.Abs(toDestination.x))
                        step = new Vector2Int(0, Math.Sign(toDestination.y));
                    else
                        step = new Vector2Int(Math.Sign(toDestination.x), 0);
                }
                else
                {
                    step = _random.Next(0, 4) switch
                    {
                        0 => Vector2Int.up,
                        1 => Vector2Int.right,
                        2 => Vector2Int.left,
                        _ => Vector2Int.down
                    };
                }

                Vector2Int next = Clamp(current + step);
                current = next == current ? Clamp(current + Vector2Int.up) : next;
            }

            CarveWide(destination, halfWidth);
            path.Add(destination);
            return path;
        }

        private void CarveLoops(List<Vector2Int> mainPath)
        {
            if (mainPath.Count < 30)
                return;

            for (int loop = 0; loop < 3; loop++)
            {
                int firstIndex = Mathf.Clamp((loop + 1) * mainPath.Count / 4, 5, mainPath.Count - 20);
                int secondIndex = Mathf.Clamp(firstIndex + _random.Next(12, 28), firstIndex + 6, mainPath.Count - 1);
                Vector2Int first = mainPath[firstIndex];
                Vector2Int second = mainPath[secondIndex];
                int side = _random.Next(0, 2) == 0 ? -1 : 1;
                Vector2Int waypoint = Clamp(new Vector2Int(
                    Mathf.Clamp((first.x + second.x) / 2 + side * _random.Next(5, 10), 3, _gridWidth - 4),
                    Mathf.Clamp((first.y + second.y) / 2, 3, _gridHeight - 4)));

                CarvePath(first, waypoint, 0);
                CarvePath(waypoint, second, 0);
            }
        }

        private void CarveOptionalBranches(List<Vector2Int> mainPath)
        {
            if (mainPath.Count < 20)
                return;

            for (int branch = 0; branch < 5; branch++)
            {
                Vector2Int origin = mainPath[_random.Next(6, mainPath.Count - 6)];
                int side = _random.Next(0, 2) == 0 ? -1 : 1;
                Vector2Int destination = Clamp(new Vector2Int(
                    origin.x + side * _random.Next(4, 10),
                    origin.y + _random.Next(-4, 9)));
                List<Vector2Int> branchPath = CarvePath(origin, destination, 0);
                if (branchPath.Count > 0)
                    CarveRoom(branchPath[branchPath.Count - 1], 3, 3);
            }
        }

        private void BuildTileMap(Vector2Int start, Vector2Int gallery, Vector2Int security, Vector2Int collapsedWing, Vector2Int exit)
        {
            GameObject generatedRoot = new GameObject("GeneratedVaultLayout");
            generatedRoot.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            generatedRoot.transform.SetParent(transform, false);

            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    if (!_tiles[x, y])
                        continue;

                    Vector2Int cell = new Vector2Int(x, y);
                    CreateTile(generatedRoot.transform, cell);
                    CreateOpenEdgeWalls(generatedRoot.transform, cell);
                }
            }

            CreateLandmark(generatedRoot.transform, start, "CommandVaultDais", _cyanMaterial, new Vector3(6f, 0.75f, 3f));
            CreateLandmark(generatedRoot.transform, gallery, "ObservationGalleryWindow", _cyanMaterial, new Vector3(16f, 1.25f, 0.18f));
            CreateLandmark(generatedRoot.transform, security, "SecurityCheckpoint", _trimMaterial, new Vector3(5f, 1.5f, 2f));
            CreateLandmark(generatedRoot.transform, collapsedWing, "CollapsedInfiltrationBarrier", _warningMaterial, new Vector3(7f, 1.4f, 2f));
            CreateLandmark(generatedRoot.transform, exit, "EmergencyDescentShaft", _warningMaterial, new Vector3(8f, 0.08f, 8f));
        }

        private void CreateTile(Transform parent, Vector2Int cell)
        {
            Vector3 position = ToWorld(cell);
            GameObject walkable = new GameObject($"WalkableTile_{cell.x}_{cell.y}");
            walkable.layer = LayerMask.NameToLayer(WalkableLayer);
            walkable.transform.SetParent(parent, false);
            walkable.transform.position = position;
            walkable.AddComponent<BoxCollider>().size = new Vector3(_tileSize.x, 0.2f, _tileSize.y);

            CreateVisualCube(parent, $"FloorPlate_{cell.x}_{cell.y}", position + Vector3.up * 0.115f, new Vector3(_tileSize.x, 0.025f, _tileSize.y), _floorMaterial);
            CreateVisualCube(parent, $"FloorInset_{cell.x}_{cell.y}", position + Vector3.up * 0.13f, new Vector3(_tileSize.x - 0.28f, 0.018f, _tileSize.y - 0.28f), _insetMaterial);
        }

        private void CreateOpenEdgeWalls(Transform parent, Vector2Int cell)
        {
            Vector3 center = ToWorld(cell);
            float halfX = _tileSize.x * 0.5f;
            float halfZ = _tileSize.y * 0.5f;

            if (!IsCarved(cell + Vector2Int.left))
                CreateWall(parent, $"WallWest_{cell.x}_{cell.y}", center + new Vector3(-halfX, 1.4f, 0f), new Vector3(0.3f, 2.8f, _tileSize.y));
            if (!IsCarved(cell + Vector2Int.right))
                CreateWall(parent, $"WallEast_{cell.x}_{cell.y}", center + new Vector3(halfX, 1.4f, 0f), new Vector3(0.3f, 2.8f, _tileSize.y));
            if (!IsCarved(cell + Vector2Int.down))
                CreateWall(parent, $"WallSouth_{cell.x}_{cell.y}", center + new Vector3(0f, 1.4f, -halfZ), new Vector3(_tileSize.x, 2.8f, 0.3f));
            if (!IsCarved(cell + Vector2Int.up))
                CreateWall(parent, $"WallNorth_{cell.x}_{cell.y}", center + new Vector3(0f, 1.4f, halfZ), new Vector3(_tileSize.x, 2.8f, 0.3f));
        }

        private void CreateWall(Transform parent, string name, Vector3 position, Vector3 size)
        {
            GameObject blocker = new GameObject(name + "_Blocker");
            blocker.layer = LayerMask.NameToLayer(BlockerLayer);
            blocker.transform.SetParent(parent, false);
            blocker.transform.position = position;
            blocker.AddComponent<BoxCollider>().size = size;
            CreateVisualCube(parent, name + "_VisualOnly", position, size, _wallMaterial);
        }

        private void CreateLandmark(Transform parent, Vector2Int cell, string name, Material material, Vector3 size)
        {
            CreateVisualCube(parent, name + "_VisualOnly", ToWorld(cell) + Vector3.up * (size.y * 0.5f), size, material);
        }

        private void CreateVisualCube(Transform parent, string name, Vector3 position, Vector3 size, Material material)
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = name;
            visual.layer = LayerMask.NameToLayer(VisualOnlyLayer);
            visual.transform.SetParent(parent, false);
            visual.transform.position = position;
            visual.transform.localScale = size;
            visual.GetComponent<Renderer>().sharedMaterial = material;
            Destroy(visual.GetComponent<Collider>());
        }

        private void CarveRoom(Vector2Int center, int halfWidth, int halfHeight)
        {
            for (int x = center.x - halfWidth; x <= center.x + halfWidth; x++)
            {
                for (int y = center.y - halfHeight; y <= center.y + halfHeight; y++)
                    CarveCell(new Vector2Int(x, y));
            }
        }

        private void CarveWide(Vector2Int center, int halfWidth)
        {
            for (int x = center.x - halfWidth; x <= center.x + halfWidth; x++)
                CarveCell(new Vector2Int(x, center.y));
        }

        private void CarveCell(Vector2Int cell)
        {
            if (cell.x >= 0 && cell.x < _gridWidth && cell.y >= 0 && cell.y < _gridHeight)
                _tiles[cell.x, cell.y] = true;
        }

        private bool IsCarved(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < _gridWidth && cell.y >= 0 && cell.y < _gridHeight && _tiles[cell.x, cell.y];
        }

        private Vector2Int Clamp(Vector2Int cell)
        {
            return new Vector2Int(Mathf.Clamp(cell.x, 1, _gridWidth - 2), Mathf.Clamp(cell.y, 1, _gridHeight - 2));
        }

        private Vector3 ToWorld(Vector2Int cell)
        {
            return new Vector3((cell.x - (_gridWidth - 1) * 0.5f) * _tileSize.x, 0f, cell.y * _tileSize.y);
        }
    }
}
