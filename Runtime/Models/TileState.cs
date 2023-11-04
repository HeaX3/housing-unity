using Essentials;
using UnityEngine;

namespace Housing
{
    public readonly struct TileState
    {
        public readonly Vector3Int tilePosition;
        public readonly NamespacedKey space;
        public readonly NamespacedKey floor;
        public readonly NamespacedKey wallNorth;
        public readonly NamespacedKey wallEast;
        public readonly NamespacedKey wallSouth;
        public readonly NamespacedKey wallWest;
        public readonly NamespacedKey ceiling;

        public TileState(
            Vector3Int tilePosition
        )
        {
            this.tilePosition = tilePosition;
            space = default;
            floor = default;
            wallNorth = default;
            wallEast = default;
            wallSouth = default;
            wallWest = default;
            ceiling = default;
        }

        public TileState(
            Vector3Int tilePosition,
            NamespacedKey space,
            NamespacedKey floor,
            NamespacedKey wallNorth,
            NamespacedKey wallEast,
            NamespacedKey wallSouth,
            NamespacedKey wallWest,
            NamespacedKey ceiling
        )
        {
            this.tilePosition = tilePosition;
            this.space = space;
            this.floor = floor;
            this.wallNorth = wallNorth;
            this.wallEast = wallEast;
            this.wallSouth = wallSouth;
            this.wallWest = wallWest;
            this.ceiling = ceiling;
        }

        public NamespacedKey GetFace(Face face)
        {
            return face switch
            {
                Face.Up => ceiling,
                Face.North => wallNorth,
                Face.East => wallEast,
                Face.South => wallSouth,
                Face.West => wallWest,
                Face.Down => floor,
                _ => space
            };
        }
    }
}