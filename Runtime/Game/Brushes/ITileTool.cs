using UnityEngine;

namespace Housing
{
    public interface ITileTool : IHousingTool
    {
        void Point(Vector3Int tilePosition);
    }
}