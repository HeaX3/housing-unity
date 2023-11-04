using System.Collections.Generic;
using Essentials.Meshes;
using UnityEngine;

namespace Housing
{
    [AddComponentMenu(menuName: "Housing/Floor Shape")]
    public class SimpleFloorShape : ShapeBehaviour
    {
        [SerializeField] private MergableMeshComponent _mesh;

        private Transform _transform;

        public override IEnumerable<MergableMeshComponent> meshes
        {
            get { yield return _mesh; }
        }

        public override void UpdateState(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face)
        {
            _mesh.enabled = true;
        }
    }
}