using System.Collections;
using System.Linq;
using Essentials;
using UnityEngine;

namespace Housing.Tests
{
    public class HousingMeshTest : MonoBehaviour
    {
        [SerializeField] private HousingMeshController _controller;
        [SerializeField] private float _tileSize = 2.5f;
        [SerializeField] private Vector3Int _chunkSize = new(4, 4, 4);
        [SerializeField] private string space;
        [SerializeField] private string ceiling;
        [SerializeField] private string wall;
        [SerializeField] private string floor;

        private void Start()
        {
            StartCoroutine(TestRoutine());
        }

        private IEnumerator TestRoutine()
        {
            yield return null;
            while (enabled)
            {
                var mesh = new HousingMesh(_chunkSize, _tileSize);
                DrawRandomRoom(mesh, Vector3Int.zero);
                DrawRandomRoom(mesh, new Vector3Int(4, 0, 4));
                _controller.mesh = mesh;
                yield return new WaitForSeconds(1);
            }
        }

        private void DrawRandomRoom(HousingMesh mesh, Vector3Int position)
        {
            var library = _controller.library;
            var defaultSet = library.sets.FirstOrDefault();
            if (!defaultSet) return;
            var size = new Vector3Int(
                Random.Range(1, 16),
                Random.Range(1, 3),
                Random.Range(1, 8)
            );
            var brush = new RoomBrush();
            for (var y = 0; y < size.y; y++)
            {
                for (var z = 0; z < size.z; z++)
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        brush.Point(position + new Vector3Int(x, y, z));
                    }
                }
            }

            brush.space = NamespacedKey.TryParse(space, out var s) ? s : default;
            brush.ceiling = NamespacedKey.TryParse(ceiling, out var c) ? c : default;
            brush.wall = NamespacedKey.TryParse(wall, out var w) ? w : default;
            brush.floor = NamespacedKey.TryParse(floor, out var f) ? f : default;
            brush.Paint(mesh);
        }
    }
}