using System;
using System.Collections.Generic;
using System.Linq;
using Essentials;
using JetBrains.Annotations;
using MultiplayerProtocol;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class HousingMesh : ISerializableValue
    {
        public Vector3Int min { get; private set; }
        public Vector3Int max { get; private set; }
        public bool hasBounds { get; private set; }

        public Vector3Int chunkSize { get; private set; }
        public float tileSize { get; private set; }
        private readonly List<HousingMeshChunk> _chunks = new();
        public IReadOnlyCollection<HousingMeshChunk> chunks => _chunks;

        public HousingMesh(Vector3Int chunkSize, float tileSize)
        {
            this.chunkSize = chunkSize;
            this.tileSize = tileSize;
        }

        public HousingMesh()
        {
        }

        public void SerializeInto(SerializedData message)
        {
            message.Write(min);
            message.Write(max);
            message.Write(hasBounds);
            message.Write(chunkSize);
            message.Write(Mathf.RoundToInt(tileSize * 100));
            message.Write(_chunks);
        }

        public void DeserializeFrom(SerializedData message)
        {
            min = message.ReadVector3Int();
            max = message.ReadVector3Int();
            hasBounds = message.ReadBool();
            chunkSize = message.ReadVector3Int();
            tileSize = message.ReadInt() / 100f;
            _chunks.Clear();
            _chunks.AddRange(message.ReadArray<HousingMeshChunk>());
        }

        public void SetBounds(Vector3Int min, Vector3Int max)
        {
            hasBounds = true;
            this.min = min;
            this.max = max;
        }

        public void ClearBounds()
        {
            hasBounds = false;
            min = default;
            max = default;
        }

        public TileState GetTileState(Vector3Int tilePosition)
        {
            if (!Contains(tilePosition)) return new TileState(tilePosition);
            var chunk = GetChunkAt(tilePosition);
            return chunk != null ? chunk.GetTileState(tilePosition - chunk.tilePosition) : new TileState(tilePosition);
        }

        [NotNull]
        public HousingMeshChunk GetOrCreateChunk(Vector3Int chunkPosition)
        {
            if (!Contains(new Vector3Int(chunkPosition.x * chunkSize.x, chunkPosition.y * chunkSize.y,
                    chunkPosition.z * chunkSize.z)))
                throw new InvalidOperationException("Tile position is outside of the bounds");
            var existing = _chunks.FirstOrDefault(c => c.position == chunkPosition);
            if (existing != null) return existing;
            var result = new HousingMeshChunk(chunkPosition, chunkSize, tileSize);
            _chunks.Add(result);
            return result;
        }

        [NotNull]
        public HousingMeshChunk GetOrCreateChunkAt(Vector3Int tilePosition)
        {
            if (!Contains(tilePosition)) throw new InvalidOperationException("Tile position is outside of the bounds");
            var chunkPosition = new Vector3Int(
                Mathf.FloorToInt(tilePosition.x / (float)chunkSize.x),
                Mathf.FloorToInt(tilePosition.y / (float)chunkSize.y),
                Mathf.FloorToInt(tilePosition.z / (float)chunkSize.z)
            );
            return GetOrCreateChunk(chunkPosition);
        }


        [CanBeNull]
        public HousingMeshChunk GetChunkAt(Vector3Int tilePosition)
        {
            if (!Contains(tilePosition)) return default;
            var chunkPosition = new Vector3Int(
                Mathf.FloorToInt(tilePosition.x / (float)chunkSize.x),
                Mathf.FloorToInt(tilePosition.y / (float)chunkSize.y),
                Mathf.FloorToInt(tilePosition.z / (float)chunkSize.z)
            );
            return _chunks.FirstOrDefault(c => c.position == chunkPosition);
        }

        public void Set(Vector3Int tilePosition, NamespacedKey type)
        {
            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                Set(tilePosition, face, type);
            }
        }

        public void Set(Vector3Int tilePosition, Face face, NamespacedKey type)
        {
            switch (face)
            {
                case Face.Space:
                    SetSpace(tilePosition, type);
                    break;
                case Face.Up:
                    SetCeiling(tilePosition, type);
                    break;
                case Face.North:
                    SetWallNorth(tilePosition, type);
                    break;
                case Face.East:
                    SetWallEast(tilePosition, type);
                    break;
                case Face.South:
                    SetWallSouth(tilePosition, type);
                    break;
                case Face.West:
                    SetWallWest(tilePosition, type);
                    break;
                case Face.Down:
                    SetFloor(tilePosition, type);
                    break;
            }
        }

        public void SetSpace(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetSpace(tilePosition - chunk.tilePosition, type);
        }

        public void SetFloor(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetFloor(tilePosition - chunk.tilePosition, type);
        }

        public void SetWalls(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetWalls(tilePosition - chunk.tilePosition, type);
        }

        public void SetWallNorth(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetWallNorth(tilePosition - chunk.tilePosition, type);
        }

        public void SetWallEast(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetWallEast(tilePosition - chunk.tilePosition, type);
        }

        public void SetWallSouth(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetWallSouth(tilePosition - chunk.tilePosition, type);
        }

        public void SetWallWest(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetWallWest(tilePosition - chunk.tilePosition, type);
        }

        public void SetCeiling(Vector3Int tilePosition, NamespacedKey type)
        {
            if (!Contains(tilePosition)) return;
            var chunk = GetOrCreateChunkAt(tilePosition);
            chunk.SetCeiling(tilePosition - chunk.tilePosition, type);
        }

        public bool Contains(Vector3Int position)
        {
            return !hasBounds || Clamp(position) == position;
        }

        private Vector3Int Clamp(Vector3Int tilePosition)
        {
            return hasBounds
                ? new Vector3Int(
                    Mathf.Clamp(tilePosition.x, min.x, max.x),
                    Mathf.Clamp(tilePosition.y, min.y, max.y),
                    Mathf.Clamp(tilePosition.z, min.z, max.z)
                )
                : tilePosition;
        }

        public JObject ToJson()
        {
            return new JObject()
                .Set("hasBounds", hasBounds)
                .Set("min", min)
                .Set("max", max)
                .Set("tileSize", tileSize)
                .Set("chunkSize", chunkSize)
                .Set("chunks", new JArray(chunks.Select(c => c.ToJson()).Cast<object>().ToArray()));
        }

        public void LoadData(JObject json)
        {
            hasBounds = json.GetBool("hasBounds");
            min = json.GetVector3Int("min");
            max = json.GetVector3Int("max");
            if (json.ContainsKey("chunks"))
            {
                _chunks.AddRange(HousingMeshChunk.ParseArray(json.GetArray("chunks") ?? new JArray()));
            }
        }

        public static bool TryParse(JObject json, out HousingMesh result)
        {
            if (json == null || !json.ContainsKey("tileSize"))
            {
                result = default;
                return false;
            }

            var chunkSize = json.GetVector3Int("chunkSize");
            var tileSize = json.GetFloat("tileSize");
            if (Mathf.Abs(tileSize) <= float.Epsilon)
            {
                Debug.LogWarning("Encountered tileSize at zero: " + tileSize);
                result = default;
                return false;
            }

            result = new HousingMesh(chunkSize, tileSize);
            result.LoadData(json);
            return true;
        }
    }
}