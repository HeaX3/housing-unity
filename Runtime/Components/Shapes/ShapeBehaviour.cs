using System.Collections.Generic;
using Essentials.Meshes;
using Housing.Interfaces;
using UnityEngine;

namespace Housing
{
    public abstract class ShapeBehaviour : MonoBehaviour, IShapeComponent
    {
        public abstract IEnumerable<MergableMeshComponent> meshes { get; }

        public abstract void UpdateState(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face);
    }
}