using System.Collections.Generic;
using Essentials.Meshes;
using Housing.Extensions;
using UnityEngine;

namespace Housing
{
    [AddComponentMenu(menuName: "Housing/Pillar Shape")]
    public class SimplePillarShape : ShapeBehaviour
    {
        [SerializeField] private MergableMeshComponent _innerLeft;
        [SerializeField] private MergableMeshComponent _straightLeft;
        [SerializeField] private MergableMeshComponent _outerLeft;
        [SerializeField] private MergableMeshComponent _innerRight;
        [SerializeField] private MergableMeshComponent _straightRight;
        [SerializeField] private MergableMeshComponent _outerRight;

        public override IEnumerable<MergableMeshComponent> meshes
        {
            get
            {
                yield return _innerLeft;
                yield return _straightLeft;
                yield return _outerLeft;
                yield return _innerRight;
                yield return _straightRight;
                yield return _outerRight;
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
            _innerLeft.enabled = false;
            _straightLeft.enabled = false;
            _outerLeft.enabled = false;
            var counterClockwiseFace = face.GetCounterClockwiseFace();
            if (tileState.GetFace(counterClockwiseFace) != default)
            {
                // inner
                _innerLeft.enabled = true;
                return;
            }

            var neighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(counterClockwiseFace));
            if (neighbour.GetFace(face) != default)
            {
                // straight
                // TODO add logic
                return;
            }

            var aroundTheCorner = tileState.tilePosition.GetRelative(face).GetRelative(face.GetCounterClockwiseFace());
            if (context.housingMesh.GetTileState(aroundTheCorner).GetFace(face.GetClockwiseFace()) != default)
            {
                // outer
                _outerLeft.enabled = true;
            }
        }

        private void UpdateStateRight(HousingMeshChunkContext context, TileState tileState, Face face)
        {
            _innerRight.enabled = false;
            _straightRight.enabled = false;
            _outerRight.enabled = false;
            var clockwiseFace = face.GetClockwiseFace();
            if (tileState.GetFace(clockwiseFace) != default)
            {
                // inner
                _innerRight.enabled = true;
                return;
            }

            var neighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(clockwiseFace));
            if (neighbour.GetFace(face) != default)
            {
                // straight
                // TODO add logic
                return;
            }

            var aroundTheCorner =
                tileState.tilePosition.GetRelative(face).GetRelative(face.GetClockwiseFace());
            if (context.housingMesh.GetTileState(aroundTheCorner).GetFace(face.GetCounterClockwiseFace()) != default)
            {
                // outer
                _outerRight.enabled = true;
            }
        }
    }
}