using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

namespace Housing
{
    public class HousingMeshController : MonoBehaviour
    {
        [SerializeField] private HousingMeshChunkController _template;
        [SerializeField] private TileShapeSetLibrary _library;

        private ObjectPool<HousingMeshChunkController> Pool { get; set; }
        private List<HousingMeshChunkController> Controllers { get; } = new();

        private HousingMesh _mesh;

        public TileShapeSetLibrary library
        {
            get => _library;
            set => _library = value;
        }

        public HousingMesh mesh
        {
            get => _mesh;
            set
            {
                if (value == _mesh) return;
                ApplyMesh(value);
            }
        }

        private void ApplyMesh(HousingMesh mesh)
        {
            _mesh = mesh;
            Pool ??= new ObjectPool<HousingMeshChunkController>(_template, transform);
            foreach (var c in Controllers) Pool.ReturnInstance(c);
            Controllers.Clear();
            if (mesh != null)
            {
                foreach (var chunk in mesh.chunks)
                {
                    var controller = Pool.GetInstance();
                    controller.name = $"Chunk ({chunk.position.x}, {chunk.position.y}, {chunk.position.z})";
                    HousingMeshBuilderService.Build(new HousingMeshChunkContext(library, mesh, chunk), controller);
                    Controllers.Add(controller);
                }
            }
        }

        public void UpdateState()
        {
            ApplyMesh(mesh);
        }
    }
}