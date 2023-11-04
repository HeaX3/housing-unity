using UnityEngine;

namespace Housing
{
    [CreateAssetMenu(fileName = "TileShapeSetLibrary", menuName = "Housing/Tile Shape Set Library")]
    public class TileShapeSetLibrary : ScriptableObject
    {
        [SerializeField] private TileShapeSet[] _sets;

        public TileShapeSet[] sets => _sets;
    }
}