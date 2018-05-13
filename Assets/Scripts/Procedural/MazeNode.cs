using UnityEngine;
using System.Collections.Generic;

namespace Simulation.Procedural
{
    class MazeNode
    {
        public const int WALL_TOP = 1 << 0;
        public const int WALL_RIGHT = 1 << 1;
        public const int WALL_BOTTOM = 1 << 2;
        public const int WALL_LEFT = 1 << 3;
        public const int WALL_AROUND = WALL_TOP | WALL_RIGHT | WALL_BOTTOM | WALL_LEFT;

        public Vector2Int GridPos { get; private set; }
        public Vector3 WorldPos { get; private set; }
        public Vector3 Size { get; private set; }
        public int Options { get; set; }
        public List<MazeWall> Walls { get; private set; } = new List<MazeWall>();
        public int SetNumber { get; set; } = 0;

        public bool HasWallTop => HasMask(WALL_TOP);
        public bool HasWallRight => HasMask(WALL_RIGHT);
        public bool HasWallBottom => HasMask(WALL_BOTTOM);
        public bool HasWallLeft => HasMask(WALL_LEFT);

        MazeWall wallPrefab;

        public MazeNode(MazeWall wallPrefab, Vector2Int gridPos, Vector3 worldPos, Vector3 size, int options = 0)
        {
            this.wallPrefab = wallPrefab;
            GridPos = gridPos;
            WorldPos = worldPos;
            Size = size;
            Options = options;
        }

        public bool HasMask(int mask) => (Options & mask) == mask;
        public void AddMask(int mask) => Options |= mask;
        public void RemoveMask(int mask) => Options &= ~mask;

        public void CreateWalls()
        {
            if (HasWallTop) {
                var top = MonoBehaviour.Instantiate(wallPrefab);

                top.transform.position = WorldPos + Vector3.forward * Size.z * 0.5f;
                top.transform.localScale = new Vector3(Size.x, Size.y, 1f);

                Walls.Add(top);

                top.Rise();
            }

            if (HasWallBottom) {
                var bottom = MonoBehaviour.Instantiate(wallPrefab);

                bottom.transform.position = WorldPos + Vector3.back * Size.z * 0.5f;
                bottom.transform.localScale = new Vector3(Size.x, Size.y, 1f);

                Walls.Add(bottom);

                bottom.Rise();
            }

            if (HasWallRight) {
                var right = MonoBehaviour.Instantiate(wallPrefab);

                right.transform.position = WorldPos + Vector3.right * Size.x * 0.5f;
                right.transform.localScale = new Vector3(1f, Size.y, Size.z);

                Walls.Add(right);

                right.Rise();
            }

            if (HasWallLeft) {
                var left = MonoBehaviour.Instantiate(wallPrefab);

                left.transform.position = WorldPos + Vector3.left * Size.x * 0.5f;
                left.transform.localScale = new Vector3(1f, Size.y, Size.z);

                Walls.Add(left);

                left.Rise();
            }
        }
    }
}