using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using Essentials.Meshes;
using UnityEngine;

namespace Housing
{
    [CreateAssetMenu(fileName = "TileShapeSet", menuName = "Housing/Tile Shape Set")]
    public class TileShapeSet : ScriptableObject
    {
        [SerializeField] private string _defaultSpace = "generic:air";
        [SerializeField] private string _defaultFloor;
        [SerializeField] private string _defaultWall;
        [SerializeField] private string _defaultCeiling;
        [SerializeField] private TileShapeController[] _templates;

        private NamespacedKey _defaultSpaceKey;
        private NamespacedKey _defaultFloorKey;
        private NamespacedKey _defaultWallKey;
        private NamespacedKey _defaultCeilingKey;

        public NamespacedKey defaultSpace
        {
            get
            {
                if (_defaultSpaceKey == default)
                    _defaultSpaceKey = NamespacedKey.TryParse(_defaultSpace, out var id) ? id : default;
                return _defaultSpaceKey;
            }
        }

        public NamespacedKey defaultFloor
        {
            get
            {
                if (_defaultFloorKey == default)
                    _defaultFloorKey = NamespacedKey.TryParse(_defaultFloor, out var id) ? id : default;
                return _defaultFloorKey;
            }
        }

        public NamespacedKey defaultWall
        {
            get
            {
                if (_defaultWallKey == default)
                    _defaultWallKey = NamespacedKey.TryParse(_defaultWall, out var id) ? id : default;
                return _defaultWallKey;
            }
        }

        public NamespacedKey defaultCeiling
        {
            get
            {
                if (_defaultCeilingKey == default)
                    _defaultCeilingKey = NamespacedKey.TryParse(_defaultCeiling, out var id) ? id : default;
                return _defaultCeilingKey;
            }
        }

        public TileShapeController[] templates => _templates ?? Array.Empty<TileShapeController>();

        public IEnumerable<ICombinableMesh> Build(HousingMeshChunkContext context)
        {
            return templates.OrderByDescending(t => t.priority).SelectMany(t => t.Build(context));
        }

        private void OnValidate()
        {
            var sorted = templates.OrderBy(t => t ? t.gameObject.name : "").ToArray();
            if (sorted.Select((s, index) => templates[index] == s).All(b => b)) return;
            _templates = sorted;
        }
    }
}