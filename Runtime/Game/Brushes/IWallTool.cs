using UnityEngine;

namespace Housing
{
    public interface IWallTool : IHousingTool
    {
        void Edge(Vector3Int a, Vector3Int b);
    }
}