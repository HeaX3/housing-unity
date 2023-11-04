using System.Collections.Generic;
using System.Linq;
using Essentials.Meshes;
using Housing.Interfaces;
using UnityEngine;

namespace Housing
{
    public class TileShapeController : MonoBehaviour
    {
        [SerializeField] [Tooltip("The higher the priority, the sooner the shape is applied")]
        private int _priority;

        public int priority => _priority;

        private Transform _transform;
        private IPlacementLogic[] _placement;
        private IShapeComponent[] _components;

        private new Transform transform
        {
            get
            {
                if (!_transform) _transform = base.transform;
                return _transform;
            }
        }

        private IPlacementLogic[] placement => _placement ??= GetComponentsInChildren<IPlacementLogic>();
        private IShapeComponent[] components => _components ??= GetComponentsInChildren<IShapeComponent>();

        public IEnumerable<ICombinableMesh> Build(HousingMeshChunkContext context)
        {
            return placement.SelectMany(p => p.Place(context, Place));
        }

        private IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context, Vector3Int localTilePosition,
            Face face)
        {
            transform.position = context.chunk.GetWorldPosition(localTilePosition) + face switch
            {
                Face.Up => new Vector3(0, 0, 0),
                Face.North => new Vector3(0, 0, 1),
                Face.East => new Vector3(1, 0, 1),
                Face.South => new Vector3(1, 0, 0),
                Face.West => new Vector3(0, 0, 0),
                Face.Down => new Vector3(0, 0, 0),
                _ => new Vector3(0, 0, 0)
            } * context.chunk.tileSize;
            transform.eulerAngles = face switch
            {
                Face.Up => new Vector3(0, 0, 0),
                Face.North => new Vector3(0, 180, 0),
                Face.East => new Vector3(0, 270, 0),
                Face.South => new Vector3(0, 0, 0),
                Face.West => new Vector3(0, 90, 0),
                Face.Down => new Vector3(0, 0, 0),
                _ => new Vector3(0, 0, 0)
            };
            return components.SelectMany(c => c.Build(context, localTilePosition, face));
        }
    }
}