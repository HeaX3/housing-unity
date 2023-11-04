using System;
using System.Collections.Generic;
using Essentials;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Logic
{
    public class FloorPlacement : SimplePlacement
    {
        [SerializeField] private string _floorShape;

        private NamespacedKey floorShapeKey;

        private NamespacedKey floorShape
        {
            get
            {
                if (floorShapeKey == default) floorShapeKey = NamespacedKey.TryParse(_floorShape, out var k) ? k : default;
                return floorShapeKey;
            }
        }
        
        protected override IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context, ushort typeId,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer)
        {
            for (var y = 0; y < context.chunk.size.y; y++)
            {
                for (var z = 0; z < context.chunk.size.z; z++)
                {
                    for (var x = 0; x < context.chunk.size.x; x++)
                    {
                        var localTilePosition = new Vector3Int(x, y, z);
                        var index = context.chunk.GetIndex(localTilePosition);
                        if (context.floor[index] != default || context.chunk.floor[index] != typeId) continue;
                        context.floor[index] = floorShape;
                        foreach (var m in placer(context, localTilePosition, Face.Down)) yield return m;
                    }
                }
            }
        }
    }
}