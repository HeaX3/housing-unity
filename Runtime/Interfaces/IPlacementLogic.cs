using System;
using System.Collections.Generic;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Interfaces
{
    public interface IPlacementLogic
    {
        /// <summary>
        /// Run the placement logic and execute the placer function for each placement to generate the mesh parts
        /// </summary>
        /// <param name="context"></param>
        /// <param name="placer"></param>
        IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer);
    }
}