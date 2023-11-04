using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using Housing.Extensions;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class RoomBulldozer : HousingTool, ITileTool
    {
        private static readonly Face[] neighbourFaces =
            { Face.Up, Face.Down, Face.North, Face.East, Face.South, Face.West };

        private NamespacedKey floorType;
        private NamespacedKey wallType;
        private NamespacedKey ceilingType;

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
                var tileState = mesh.GetTileState(position);
                if (floorType == default && tileState.floor != default) floorType = tileState.floor;
                if (wallType == default)
                {
                    if (tileState.wallNorth != default) wallType = tileState.wallNorth;
                    if (tileState.wallEast != default) wallType = tileState.wallEast;
                    if (tileState.wallSouth != default) wallType = tileState.wallSouth;
                    if (tileState.wallWest != default) wallType = tileState.wallWest;
                }

                if (ceilingType == default && tileState.ceiling != default) ceilingType = tileState.ceiling;
                mesh.Set(position, default);
            }

            if (floorType == default)
            {
                floorType = GuessFloorType(mesh);
            }

            if (wallType == default)
            {
                wallType = GuessWallType(mesh);
            }

            if (ceilingType == default)
            {
                ceilingType = GuessCeilingType(mesh);
            }

            foreach (var position in positions)
            {
                if (!mesh.Contains(position)) continue;
                foreach (var face in neighbourFaces)
                {
                    var relative = position.GetRelative(face);
                    if (positions.Contains(relative)) continue;
                    Reconstruct(mesh, relative, face.GetOppositeFace());
                }
            }
        }

        private void Reconstruct(HousingMesh mesh, Vector3Int position, Face face)
        {
            var tileState = mesh.GetTileState(position);
            if (tileState.space == default || tileState.GetFace(face) != default) return;
            mesh.Set(position, face, face switch
            {
                Face.Up => ceilingType,
                Face.North or Face.East or Face.South or Face.West => wallType,
                Face.Down => floorType,
                _ => throw new InvalidOperationException()
            });
        }

        private static NamespacedKey GuessFloorType(HousingMesh mesh)
        {
            // welcome to LINQ hell :))
            //
            // In human terms:
            // From all chunks, select all their surface types except the empty type,
            // and the amount of times the type occurs within the chunk,
            // then add it all up and select the type that occurs the most
            var counts = mesh.chunks.SelectMany(c => c.types.Skip(1).Select((t, index) =>
                    new KeyValuePair<NamespacedKey, int>(
                        t,
                        c.floor.Count(f => f == index + 1)
                    )))
                .GroupBy(e => e.Key)
                .Select(g => new KeyValuePair<NamespacedKey, int>(
                    g.Key,
                    g.Select(e => e.Value).DefaultIfEmpty(0).Sum(v => v)
                ))
                .Where(e => e.Value > 0)
                .ToDictionary(e => e.Key, e => e.Value);
            return counts.Count != 0
                ? counts.OrderByDescending(e => e.Value).First().Key
                : default;
        }

        private static NamespacedKey GuessWallType(HousingMesh mesh)
        {
            // welcome to LINQ hell :))
            //
            // In human terms:
            // From all chunks, select all their surface types except the empty type,
            // and the amount of times the type occurs within the chunk,
            // then add it all up and select the type that occurs the most
            var counts = mesh.chunks.SelectMany(c => c.types.Skip(1).Select((t, index) =>
                    new KeyValuePair<NamespacedKey, int>(
                        t,
                        c.wallsNorth.Count(f => f == index + 1) + c.wallsEast.Count(f => f == index + 1) +
                        c.wallsSouth.Count(f => f == index + 1) + c.wallsWest.Count(f => f == index + 1)
                    )))
                .GroupBy(e => e.Key)
                .Select(g => new KeyValuePair<NamespacedKey, int>(
                    g.Key,
                    g.Select(e => e.Value).DefaultIfEmpty(0).Sum(v => v)
                ))
                .Where(e => e.Value > 0)
                .ToDictionary(e => e.Key, e => e.Value);
            return counts.Count != 0
                ? counts.OrderByDescending(e => e.Value).First().Key
                : default;
        }

        private static NamespacedKey GuessCeilingType(HousingMesh mesh)
        {
            // welcome to LINQ hell :))
            //
            // In human terms:
            // From all chunks, select all their surface types except the empty type,
            // and the amount of times the type occurs within the chunk,
            // then add it all up and select the type that occurs the most
            var counts = mesh.chunks.SelectMany(c => c.types.Skip(1).Select((t, index) =>
                    new KeyValuePair<NamespacedKey, int>(
                        t,
                        c.ceiling.Count(f => f == index + 1)
                    )))
                .GroupBy(e => e.Key)
                .Select(g => new KeyValuePair<NamespacedKey, int>(
                    g.Key,
                    g.Select(e => e.Value).DefaultIfEmpty(0).Sum(v => v)
                ))
                .Where(e => e.Value > 0)
                .ToDictionary(e => e.Key, e => e.Value);
            return counts.Count != 0
                ? counts.OrderByDescending(e => e.Value).First().Key
                : default;
        }

        public static bool TryParse(JObject json, out RoomBulldozer result)
        {
            result = new RoomBulldozer();
            return true;
        }
    }
}