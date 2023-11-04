using Essentials;
using JetBrains.Annotations;
using UnityEngine;

namespace Housing
{
    public class CompiledTileState
    {
        public Vector3Int tilePosition;

        public NamespacedKey space;
        public NamespacedKey floor;
        public NamespacedKey wallNorth;
        public NamespacedKey wallEast;
        public NamespacedKey wallSouth;
        public NamespacedKey wallWest;
        public NamespacedKey ceiling;

        /// <summary>
        /// 1: up, 2: north, 4: east, 8: south, 16: west, 32: down
        /// </summary>
        private byte facesPlaced;

        [CanBeNull] public CompiledTileState up;
        [CanBeNull] public CompiledTileState down;
        [CanBeNull] public CompiledTileState north;
        [CanBeNull] public CompiledTileState east;
        [CanBeNull] public CompiledTileState south;
        [CanBeNull] public CompiledTileState west;

        public bool IsFacePlaced(Face face)
        {
            return (facesPlaced & (byte)face) != 0;
        }

        public void SetFacePlaced(Face face)
        {
            facesPlaced &= (byte)face;
        }

        public enum Face
        {
            Up = 1,
            North = 2,
            East = 4,
            South = 8,
            West = 16,
            Down = 32
        }
    }
}