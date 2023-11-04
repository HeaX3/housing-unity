using System;
using System.Collections.Generic;
using Essentials;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Logic
{
    public class WallPlacement : SimplePlacement
    {
        [SerializeField] private string _wallShape;

        private NamespacedKey wallShapeKey;

        private NamespacedKey wallShape
        {
            get
            {
                if (wallShapeKey == default) wallShapeKey = NamespacedKey.TryParse(_wallShape, out var k) ? k : default;
                return wallShapeKey;
            }
        }

        protected override IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context, ushort typeId,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer)
        {
            var wallShape = this.wallShape;
            for (var y = 0; y < context.chunk.size.y; y++)
            {
                for (var z = 0; z < context.chunk.size.z; z++)
                {
                    for (var x = 0; x < context.chunk.size.x; x++)
                    {
                        var localTilePosition = new Vector3Int(x, y, z);
                        var index = context.chunk.GetIndex(localTilePosition);
                        if (context.wallsNorth[index] == default && context.chunk.wallsNorth[index] == typeId)
                        {
                            context.wallsNorth[index] = wallShape;
                            foreach (var m in placer(context, localTilePosition, Face.North)) yield return m;
                        }

                        if (context.wallsEast[index] == default && context.chunk.wallsEast[index] == typeId)
                        {
                            context.wallsEast[index] = wallShape;
                            foreach (var m in placer(context, localTilePosition, Face.East)) yield return m;
                        }

                        if (context.wallsSouth[index] == default && context.chunk.wallsSouth[index] == typeId)
                        {
                            context.wallsSouth[index] = wallShape;
                            foreach (var m in placer(context, localTilePosition, Face.South)) yield return m;
                        }

                        if (context.wallsWest[index] == default && context.chunk.wallsWest[index] == typeId)
                        {
                            context.wallsWest[index] = wallShape;
                            foreach (var m in placer(context, localTilePosition, Face.West)) yield return m;
                        }
                    }
                }
            }
        }
    }
}