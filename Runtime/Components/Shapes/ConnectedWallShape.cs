using System.Collections.Generic;
using Essentials.Meshes;
using Housing.Extensions;
using UnityEngine;

namespace Housing
{
    [AddComponentMenu(menuName: "Housing/Connected Wall Shape")]
    public class ConnectedWallShape : ShapeBehaviour
    {
        [SerializeField] private MergableMeshComponent _leftEnd;
        [SerializeField] private MergableMeshComponent _left;
        [SerializeField] private MergableMeshComponent _right;
        [SerializeField] private MergableMeshComponent _rightEnd;

        private Transform _transform;

        public override IEnumerable<MergableMeshComponent> meshes
        {
            get
            {
                yield return _leftEnd;
                yield return _left;
                yield return _right;
                yield return _rightEnd;
            }
        }

        public override void UpdateState(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face)
        {
            var tileState = context.chunk.GetTileState(localTilePosition);
            UpdateStateLeft(context, tileState, face);
            UpdateStateRight(context, tileState, face);
        }

        private void UpdateStateLeft(HousingMeshChunkContext context, TileState tileState, Face face)
        {
            _leftEnd.enabled = false;
            _left.enabled = false;
            var counterClockwiseFace = face.GetCounterClockwiseFace();
            if (tileState.GetFace(counterClockwiseFace) != default)
            {
                // end
                _leftEnd.enabled = true;
                return;
            }

            var neighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(counterClockwiseFace));
            if (neighbour.GetFace(face) == tileState.GetFace(face))
            {
                // connected
                _left.enabled = true;
                return;
            }

            _leftEnd.enabled = true;
        }

        private void UpdateStateRight(HousingMeshChunkContext context, TileState tileState, Face face)
        {
            _right.enabled = false;
            _rightEnd.enabled = false;
            var clockwiseFace = face.GetClockwiseFace();
            if (tileState.GetFace(clockwiseFace) != default)
            {
                // end
                _rightEnd.enabled = true;
                return;
            }

            var neighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(clockwiseFace));
            if (neighbour.GetFace(face) == tileState.GetFace(face))
            {
                // connected
                _right.enabled = true;
                return;
            }

            // end
            _rightEnd.enabled = true;
        }
    }
}