using ObjectPooling;
using UnityEngine;

namespace Housing
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HousingMeshChunkController : MonoBehaviour, IPoolable
    {
        [SerializeField] [HideInInspector] private MeshFilter _meshFilter;
        [SerializeField] [HideInInspector] private MeshRenderer _meshRenderer;

        public Mesh mesh
        {
            get
            {
                var m = _meshFilter.sharedMesh;
                if (!m)
                {
                    m = new Mesh
                    {
                        name = gameObject.name
                    };
                    _meshFilter.sharedMesh = m;
                }

                return m;
            }
        }

        public Material[] materials
        {
            get => _meshRenderer.sharedMaterials;
            set => _meshRenderer.sharedMaterials = value;
        }

        private void OnDestroy()
        {
            if (_meshFilter.sharedMesh)
            {
                Destroy(_meshFilter.sharedMesh);
            }
        }

        private void OnValidate()
        {
            BindReferences();
        }

        private void BindReferences()
        {
            if (!_meshFilter || _meshFilter.gameObject != gameObject) _meshFilter = GetComponent<MeshFilter>();
            if (!_meshRenderer || _meshRenderer.gameObject != gameObject) _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Activate()
        {
            BindReferences();
        }

        public void ResetForPool()
        {
        }
    }
}