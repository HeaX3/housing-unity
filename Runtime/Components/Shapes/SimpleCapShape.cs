using System.Collections.Generic;
using Essentials.Meshes;
using Housing.Extensions;
using UnityEngine;

namespace Housing
{
    [AddComponentMenu(menuName: "Housing/Cap Shape")]
    public class SimpleCapShape : ShapeBehaviour
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
            var aboveTileState = context.chunk.GetTileState(localTilePosition.GetRelative(Face.Up));
            if (aboveTileState.floor != default || aboveTileState.GetFace(face) != default)
            {
                foreach (var m in meshes) m.enabled = false;
                return;
            }

            var tileState = context.chunk.GetTileState(localTilePosition);
            UpdateStateLeft(context, localTilePosition, face, tileState);
            UpdateStateRight(context, localTilePosition, face, tileState);
        }

        private void UpdateStateLeft(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face,
            TileState tileState)
        {
            _innerLeft.enabled = false;
            _straightLeft.enabled = false;
            _outerLeft.enabled = false;
            var counterClockwiseFace = face.GetCounterClockwiseFace();
            var neighbour =
                context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(face.GetCounterClockwiseFace()));
            var aboveNeighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(Face.Up));
            if (aboveNeighbour.GetFace(counterClockwiseFace) == default && tileState.GetFace(counterClockwiseFace) != default)
            {
                // inner
                _innerLeft.enabled = true;
                return;
            }

            if (aboveNeighbour.GetFace(counterClockwiseFace) != default || neighbour.GetFace(face) != default)
            {
                // straight
                _straightLeft.enabled = true;
                return;
            }

            var aroundTheCorner = tileState.tilePosition.GetRelative(face).GetRelative(face.GetCounterClockwiseFace());
            if (context.housingMesh.GetTileState(aroundTheCorner).GetFace(face.GetClockwiseFace()) != default)
            {
                // outer
                _outerLeft.enabled = true;
                return;
            }

            // straight
            _straightLeft.enabled = true;
        }

        private void UpdateStateRight(HousingMeshChunkContext context, Vector3Int localTilePosition, Face face,
            TileState tileState)
        {
            _innerRight.enabled = false;
            _straightRight.enabled = false;
            _outerRight.enabled = false;
            var clockwiseFace = face.GetClockwiseFace();
            var neighbour =
                context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(face.GetClockwiseFace()));
            var aboveNeighbour = context.housingMesh.GetTileState(tileState.tilePosition.GetRelative(Face.Up));
            if (aboveNeighbour.GetFace(clockwiseFace) == default && tileState.GetFace(clockwiseFace) != default)
            {
                // inner
                _innerRight.enabled = true;
                return;
            }

            if (aboveNeighbour.GetFace(clockwiseFace) != default || neighbour.GetFace(face) != default)
            {
                // straight
                _straightRight.enabled = true;
                return;
            }

            var aroundTheCorner =
                tileState.tilePosition.GetRelative(face).GetRelative(face.GetClockwiseFace());
            if (context.housingMesh.GetTileState(aroundTheCorner).GetFace(face.GetCounterClockwiseFace()) != default)
            {
                // outer
                _outerRight.enabled = true;
                return;
            }

            // straight
            _straightRight.enabled = true;
        }
    }
}