using UnityEngine;

namespace Housing.Extensions
{
    public static class Vector3IntExtensions
    {
        public static Vector3Int GetRelative(this Vector3Int position, Face face)
        {
            return position + face switch
            {
                Face.Up => Vector3Int.up,
                Face.Down => Vector3Int.down,
                Face.North => Vector3Int.forward,
                Face.East => Vector3Int.right,
                Face.South => Vector3Int.back,
                Face.West => Vector3Int.left,
                _ => default
            };
        }
    }
}