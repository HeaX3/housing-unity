using System;
using System.Collections.Generic;
using Essentials;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Logic
{
    public class CeilingPlacement : SimplePlacement
    {
        [SerializeField] private string _ceilingShape;

        private NamespacedKey ceilingShapeKey;

        private NamespacedKey ceilingShape
        {
            get
            {
                if (ceilingShapeKey == default)
                    ceilingShapeKey = NamespacedKey.TryParse(_ceilingShape, out var k) ? k : default;
                return ceilingShapeKey;
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
                        if (context.ceiling[index] != default || context.chunk.ceiling[index] != typeId) continue;
                        context.ceiling[index] = ceilingShape;
                        foreach (var m in placer(context, localTilePosition, Face.Up)) yield return m;
                    }
                }
            }
        }
    }
}