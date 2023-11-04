using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class HousingMeshChunk
    {
        public readonly Vector3Int position;
        public readonly Vector3Int size;
        public readonly float tileSize;
        public readonly Vector3 worldPosition;
        public readonly Vector3Int tilePosition;

        /// <summary>
        /// Specifies space type, e.g. void, air, water, lava<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] space; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies floor type, e.g. wood, stone, steel (y-negative from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] floor; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies wall type of north facing walls (z-positive from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] wallsNorth; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies wall type of east facing walls (x-positive from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] wallsEast; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies wall type of south facing walls (z-negative from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] wallsSouth; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies wall type of west facing walls (x-negative from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] wallsWest; // length: size.y * size.z * size.x

        /// <summary>
        /// Specifies ceiling type, e.g. wood, stone, steel (y-positive from the tile center)<br />
        /// Contains indices referencing the types array
        /// </summary>
        public readonly ushort[] ceiling; // length: size.y * size.z * size.x

        /// <summary>
        /// Array of types used within this chunk
        /// </summary>
        private readonly List<NamespacedKey> _types;

        public IReadOnlyList<NamespacedKey> types => _types;

        public HousingMeshChunk(Vector3Int position, Vector3Int size, float tileSize)
        {
            this.position = position;
            this.size = size;
            this.tileSize = tileSize;
            worldPosition = new Vector3(
                tileSize * position.x * size.z,
                tileSize * position.y * size.y,
                tileSize * position.z * size.x
            );
            tilePosition = new Vector3Int(position.x * size.x, position.y * size.y, position.z * size.z);
            var arrayLength = size.y * size.z * size.x;
            space = new ushort[arrayLength];
            floor = new ushort[arrayLength];
            wallsNorth = new ushort[arrayLength];
            wallsEast = new ushort[arrayLength];
            wallsSouth = new ushort[arrayLength];
            wallsWest = new ushort[arrayLength];
            ceiling = new ushort[arrayLength];
            _types = new List<NamespacedKey>
            {
                default
            };
        }

        public HousingMeshChunk(
            Vector3Int position,
            Vector3Int size,
            float tileSize,
            IEnumerable<ushort> space,
            IEnumerable<ushort> floor,
            IEnumerable<ushort> wallsNorth, IEnumerable<ushort> wallsEast,
            IEnumerable<ushort> wallsSouth, IEnumerable<ushort> wallsWest,
            IEnumerable<ushort> ceiling,
            IEnumerable<NamespacedKey> types
        )
        {
            this.position = position;
            this.size = size;
            this.tileSize = tileSize;
            worldPosition = new Vector3(
                tileSize * position.x * size.z,
                tileSize * position.y * size.y,
                tileSize * position.z * size.x
            );
            tilePosition = new Vector3Int(position.x * size.x, position.y * size.y, position.z * size.z);
            this.space = space.ToArray();
            this.floor = floor.ToArray();
            this.wallsNorth = wallsNorth.ToArray();
            this.wallsEast = wallsEast.ToArray();
            this.wallsSouth = wallsSouth.ToArray();
            this.wallsWest = wallsWest.ToArray();
            this.ceiling = ceiling.ToArray();
            _types = new List<NamespacedKey>(types);
        }

        /// <summary>
        /// Get the 3d tile position within the chunk where 0, 0, 0 is the origin of the chunk
        /// </summary>
        public Vector3Int GetTilePosition(int index)
        {
            var x = index % size.x;
            var z = (index % (size.x * size.z) - x) / size.x;
            var y = index / (size.x * size.z);
            return new Vector3Int(x, y, z);
        }

        public Vector3 GetWorldPosition(Vector3Int localTilePosition) => GetWorldPosition(GetIndex(localTilePosition));

        public Vector3 GetWorldPosition(int index)
        {
            var x = index % size.x;
            var z = (index % (size.x * size.z) - x) / size.x;
            var y = index / (size.x * size.z);
            return worldPosition + new Vector3(x * tileSize, y * tileSize, z * tileSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Local x tile position inside the chunk</param>
        /// <param name="y">Local y tile position inside the chunk</param>
        /// <param name="z">Local z tile position inside the chunk</param>
        /// <returns></returns>
        public TileState GetTileState(int x, int y, int z)
        {
            return GetTileState(new Vector3Int(x, y, z));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">Local tile position inside the chunk</param>
        /// <returns></returns>
        public TileState GetTileState(Vector3Int position)
        {
            var index = position.y * size.x * size.z +
                        position.z * size.x + position.x;
            return new TileState(
                position + tilePosition,
                types[space[index]],
                types[floor[index]],
                types[wallsNorth[index]],
                types[wallsEast[index]],
                types[wallsSouth[index]],
                types[wallsWest[index]],
                types[ceiling[index]]
            );
        }

        public void SetSpace(Vector3Int position, NamespacedKey type)
        {
            SetSpace(position, GetOrCreateTypeId(type));
        }

        public void SetFloor(Vector3Int position, NamespacedKey type)
        {
            SetFloor(position, GetOrCreateTypeId(type));
        }

        public void SetWalls(Vector3Int position, NamespacedKey type)
        {
            SetWalls(position, GetOrCreateTypeId(type));
        }

        public void SetWallNorth(Vector3Int position, NamespacedKey type)
        {
            SetWallNorth(position, GetOrCreateTypeId(type));
        }

        public void SetWallEast(Vector3Int position, NamespacedKey type)
        {
            SetWallEast(position, GetOrCreateTypeId(type));
        }

        public void SetWallSouth(Vector3Int position, NamespacedKey type)
        {
            SetWallSouth(position, GetOrCreateTypeId(type));
        }

        public void SetWallWest(Vector3Int position, NamespacedKey type)
        {
            SetWallWest(position, GetOrCreateTypeId(type));
        }

        public void SetCeiling(Vector3Int position, NamespacedKey type)
        {
            SetCeiling(position, GetOrCreateTypeId(type));
        }

        public void SetSpace(Vector3Int position, ushort typeId)
        {
            space[GetIndex(position)] = typeId;
        }

        public void SetFloor(Vector3Int position, ushort typeId)
        {
            floor[GetIndex(position)] = typeId;
        }

        public void SetWalls(Vector3Int position, ushort typeId)
        {
            wallsNorth[GetIndex(position)] = typeId;
            wallsEast[GetIndex(position)] = typeId;
            wallsSouth[GetIndex(position)] = typeId;
            wallsWest[GetIndex(position)] = typeId;
        }

        public void SetWallNorth(Vector3Int position, ushort typeId)
        {
            wallsNorth[GetIndex(position)] = typeId;
        }

        public void SetWallEast(Vector3Int position, ushort typeId)
        {
            wallsEast[GetIndex(position)] = typeId;
        }

        public void SetWallSouth(Vector3Int position, ushort typeId)
        {
            wallsSouth[GetIndex(position)] = typeId;
        }

        public void SetWallWest(Vector3Int position, ushort typeId)
        {
            wallsWest[GetIndex(position)] = typeId;
        }

        public void SetCeiling(Vector3Int position, ushort typeId)
        {
            ceiling[GetIndex(position)] = typeId;
        }

        public int GetIndex(Vector3Int position)
        {
            return position.y * size.z * size.x + position.z * size.x + position.x;
        }

        public ushort GetOrCreateTypeId(NamespacedKey type)
        {
            var index = types.IndexOf(type);
            if (index < 0)
            {
                index = types.Count;
                _types.Add(type);
            }

            return (ushort)index;
        }

        public JObject ToJson()
        {
            return new JObject()
                .Set("position", position)
                .Set("size", size)
                .Set("tileSize", tileSize)
                .Set("space", Convert.ToBase64String(space.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("floor", Convert.ToBase64String(floor.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("wallsNorth", Convert.ToBase64String(wallsNorth.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("wallsEast", Convert.ToBase64String(wallsEast.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("wallsSouth", Convert.ToBase64String(wallsSouth.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("wallsWest", Convert.ToBase64String(wallsWest.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("ceiling", Convert.ToBase64String(ceiling.SelectMany(BitConverter.GetBytes).ToArray()))
                .Set("types", new JArray(types.Select(s => s.ToString()).Cast<object>().ToArray()));
        }

        public static bool TryParse(JObject json, out HousingMeshChunk result)
        {
            try
            {
                var position = json.GetVector3Int("position");
                var size = json.GetVector3Int("size");
                var tileSize = json.GetFloat("tileSize");
                var space = ParseIndices(json.GetString("space"));
                var floor = ParseIndices(json.GetString("floor"));
                var wallsNorth = ParseIndices(json.GetString("wallsNorth"));
                var wallsEast = ParseIndices(json.GetString("wallsEast"));
                var wallsSouth = ParseIndices(json.GetString("wallsSouth"));
                var wallsWest = ParseIndices(json.GetString("wallsWest"));
                var ceiling = ParseIndices(json.GetString("ceiling"));
                var types = (json.GetArray("types") ?? new JArray())
                    .Select(value => NamespacedKey.TryParse(value, out var key) ? key : default)
                    .ToArray();

                result = new HousingMeshChunk(
                    position, size, tileSize,
                    space,
                    floor,
                    wallsNorth, wallsEast, wallsSouth, wallsWest,
                    ceiling,
                    types
                );
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                result = default;
                return false;
            }
        }

        public static IEnumerable<HousingMeshChunk> ParseArray(JArray json)
        {
            return json.OfType<JObject>()
                .Select(entry => TryParse(entry, out var item) ? item : default)
                .Where(item => item != null);
        }

        private static ushort[] ParseIndices(string s)
        {
            var indicesArray = Convert.FromBase64String(s);
            var result = new ushort[indicesArray.Length / 2];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToUInt16(indicesArray, i * 2);
            }

            return result;
        }
    }
}