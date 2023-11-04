using Essentials;
using UnityEngine;

namespace Housing
{
    public class HousingMeshChunkContext
    {
        public readonly TileShapeSetLibrary library;
        public readonly HousingMesh housingMesh;
        public readonly HousingMeshChunk chunk;

        /// <summary>
        /// True for each shape that has been applied to the space
        /// </summary>
        public readonly NamespacedKey[] space;

        /// <summary>
        /// True for each shape that has been applied to the floor
        /// </summary>
        public readonly NamespacedKey[] floor;

        /// <summary>
        /// True for each shape that has been applied to the north wall
        /// </summary>
        public readonly NamespacedKey[] wallsNorth;

        /// <summary>
        /// True for each shape that has been applied to the east wall
        /// </summary>
        public readonly NamespacedKey[] wallsEast;

        /// <summary>
        /// True for each shape that has been applied to the south wall
        /// </summary>
        public readonly NamespacedKey[] wallsSouth;

        /// <summary>
        /// True for each shape that has been applied to the west wall
        /// </summary>
        public readonly NamespacedKey[] wallsWest;

        /// <summary>
        /// True for each shape that has been applied to the ceiling
        /// </summary>
        public readonly NamespacedKey[] ceiling;

        public HousingMeshChunkContext(
            TileShapeSetLibrary library,
            HousingMesh housingMesh,
            HousingMeshChunk chunk
        )
        {
            this.library = library;
            this.housingMesh = housingMesh;
            this.chunk = chunk;

            var faceArrayLength = chunk.size.y * chunk.size.z * chunk.size.x;
            space = new NamespacedKey[faceArrayLength];
            floor = new NamespacedKey[faceArrayLength];
            wallsNorth = new NamespacedKey[faceArrayLength];
            wallsEast = new NamespacedKey[faceArrayLength];
            wallsSouth = new NamespacedKey[faceArrayLength];
            wallsWest = new NamespacedKey[faceArrayLength];
            ceiling = new NamespacedKey[faceArrayLength];
        }

        public NamespacedKey GetPlacedFace(Face face, Vector3Int localTilePosition)
        {
            return GetPlacedFace(face, chunk.GetIndex(localTilePosition));
        }

        public NamespacedKey GetPlacedFace(Face face, int index)
        {
            return face switch
            {
                Face.Up => ceiling[index],
                Face.North => wallsNorth[index],
                Face.East => wallsEast[index],
                Face.South => wallsSouth[index],
                Face.West => wallsWest[index],
                Face.Down => floor[index],
                _ => space[index]
            };
        }
    }
}