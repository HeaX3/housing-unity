using System.Collections.Generic;
using System.Linq;
using Essentials.Meshes;
using UnityEngine;

namespace Housing.Interfaces
{
    public interface IShapeComponent
    {
        IEnumerable<MergableMeshComponent> meshes { get; }
        
        public IEnumerable<ICombinableMesh> Build(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face)
        {
            UpdateState(context, localTilePosition, face);
            return meshes.Where(m => m.enabled).Select(m => m.CalculateCurrentMesh());
        }

        void UpdateState(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face);
    }
}