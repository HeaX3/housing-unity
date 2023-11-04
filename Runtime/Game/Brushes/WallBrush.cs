using System.Collections.Generic;
using Essentials;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class WallBrush : HousingTool, IWallTool
    {
        public NamespacedKey wall;

        private readonly List<Wall> edges = new();

        public override void Paint(HousingMesh mesh)
        {
            foreach (var edge in edges)
            {
                PaintEdge(mesh, edge.a, edge.b);
            }
        }

        private void PaintEdge(HousingMesh mesh, Vector3Int a, Vector3Int b)
        {
            var min = new Vector3Int(
                Mathf.Min(a.x, b.x),
                Mathf.Min(a.y, b.y),
                Mathf.Min(a.z, b.z)
            );
            var max = new Vector3Int(
                Mathf.Max(a.x, b.x),
                Mathf.Max(a.y, b.y),
                Mathf.Max(a.z, b.z)
            );
            var delta = max - min;
            var first = mesh.GetTileState(min);
            var second = mesh.GetTileState(delta.x > 0
                ? min + new Vector3Int(0, 0, -1)
                : min + new Vector3Int(-1, 0, 0)
            );
            var firstFace = delta.x > 0 ? Face.South : Face.West;
            var secondFace = delta.x > 0 ? Face.North : Face.East;
            if (first.space != default) mesh.Set(first.tilePosition, firstFace, wall);
            if (second.space != default) mesh.Set(second.tilePosition, secondFace, wall);
        }

        public override void Clear()
        {
            edges.Clear();
        }

        public override void Reset()
        {
            wall = default;
            Clear();
        }

        public void Edge(Vector3Int a, Vector3Int b)
        {
            edges.Add(new Wall(a, b));
        }

        public void LoadData(JObject json)
        {
            wall = json.GetNamespacedKey("walls");
        }

        public static bool TryParse(JObject json, out WallBrush result)
        {
            result = new WallBrush();
            result.LoadData(json);
            return true;
        }

        private readonly struct Wall
        {
            public readonly Vector3Int a;
            public readonly Vector3Int b;

            public Wall(Vector3Int a, Vector3Int b)
            {
                this.a = a;
                this.b = b;
            }
        }
    }
}