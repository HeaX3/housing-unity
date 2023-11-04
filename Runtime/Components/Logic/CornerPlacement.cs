using System;
using System.Collections.Generic;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Logic
{
    public class CornerPlacement : SimplePlacement
    {
        protected override IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context, ushort typeId,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer)
        {
            yield break;
        }
    }
}