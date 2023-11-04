using System.Collections.Generic;
using Essentials;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class RoomBrush : HousingTool, ITileTool
    {
        public NamespacedKey space;
        public NamespacedKey floor;
        public NamespacedKey wall;
        public NamespacedKey ceiling;

        public bool expandSpace;

        private readonly List<Vector3Int> positions = new();

        public void Point(Vector3Int position)
        {
            positions.Add(position);
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
            foreach (var position in positions)
            {
                if (!mesh.Contains(position)) continue;
                Paint(mesh, position);
            }
        }

        private void Paint(HousingMesh mesh, Vector3Int position)
        {
            var previousState = mesh.GetTileState(position);
            if (space != default) PaintSpace(mesh, previousState);
            if (ceiling != default) PaintUp(mesh, previousState);
            if (floor != default) PaintDown(mesh, previousState);
            if (wall != default)
            {
                PaintNorth(mesh, previousState);
                PaintEast(mesh, previousState);
                PaintSouth(mesh, previousState);
                PaintWest(mesh, previousState);
            }
        }

        private void PaintSpace(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            mesh.SetSpace(position, space);
        }

        private void PaintUp(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.up;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition))
            {
                if (neighbour.floor != default) mesh.SetFloor(neighbourPosition, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetFloor(neighbourPosition, floor);
            }

            mesh.SetCeiling(position, ceiling);
        }

        private void PaintDown(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.down;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition))
            {
                if (neighbour.ceiling != default) mesh.SetCeiling(neighbourPosition, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetCeiling(neighbourPosition, ceiling);
            }

            mesh.SetFloor(position, floor);
        }

        private void PaintNorth(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.forward;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition) || (expandSpace && neighbour.space == space))
            {
                if (tileState.floor == floor) return;
                if (neighbour.wallSouth != default) mesh.SetWallSouth(neighbourPosition, default);
                mesh.SetWallNorth(position, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetWallSouth(neighbourPosition, wall);
            }

            mesh.SetWallNorth(position, wall);
        }

        private void PaintEast(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.right;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition) || (expandSpace && neighbour.space == space))
            {
                if (tileState.floor == floor) return;
                if (neighbour.wallWest != default) mesh.SetWallWest(neighbourPosition, default);
                mesh.SetWallEast(position, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetWallWest(neighbourPosition, wall);
            }

            mesh.SetWallEast(position, wall);
        }

        private void PaintSouth(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.back;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition) || (expandSpace && neighbour.space == space))
            {
                if (tileState.floor == floor) return;
                if (neighbour.wallNorth != default) mesh.SetWallNorth(neighbourPosition, default);
                mesh.SetWallSouth(position, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetWallNorth(neighbourPosition, wall);
            }

            mesh.SetWallSouth(position, wall);
        }

        private void PaintWest(HousingMesh mesh, TileState tileState)
        {
            var position = tileState.tilePosition;
            var neighbourPosition = position + Vector3Int.left;
            var neighbour = mesh.GetTileState(neighbourPosition);
            if (positions.Contains(neighbourPosition) || (expandSpace && neighbour.space == space))
            {
                if (tileState.floor == floor) return;
                if (neighbour.wallEast != default) mesh.SetWallEast(neighbourPosition, default);
                mesh.SetWallWest(position, default);
                return;
            }

            if (neighbour.space != default)
            {
                mesh.SetWallEast(neighbourPosition, wall);
            }

            mesh.SetWallWest(position, wall);
        }

        public void LoadData(JObject json)
        {
            space = json.GetNamespacedKey("space");
            floor = json.GetNamespacedKey("floor");
            wall = json.GetNamespacedKey("walls");
            // outsideWalls = json.GetNamespacedKey("outsideWalls");
            ceiling = json.GetNamespacedKey("ceiling");
            // roof = json.GetNamespacedKey("roof");
        }

        public static bool TryParse(JObject json, out RoomBrush result)
        {
            result = new RoomBrush();
            result.LoadData(json);
            return true;
        }
    }
}