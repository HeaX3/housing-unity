using UnityEngine;

namespace Housing
{
    public interface IFaceTool : IHousingTool
    {
        void Point(Vector3Int tilePosition, Face face);
    }
}