using System.Collections.Generic;
using Essentials;
using UnityEngine;

namespace Housing
{
    public class DetailBrush : HousingTool, IFaceTool
    {
        public NamespacedKey type;

        private readonly List<KeyValuePair<Vector3Int, Face>> positions = new();

        public void Point(Vector3Int position, Face face)
        {
            positions.Add(new KeyValuePair<Vector3Int, Face>(position, face));
        }

        public override void Clear()
        {
            positions.Clear();
        }

        public override void Reset()
        {
            Clear();
        }

        public override void Paint(HousingMesh mesh)
        {
            foreach (var (tilePosition, face) in positions)
            {
                mesh.Set(tilePosition, face, type);
            }
        }
    }
}