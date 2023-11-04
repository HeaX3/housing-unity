using System;
using System.Collections.Generic;
using Essentials;
using Essentials.Meshes;
using Housing.Interfaces;
using UnityEngine;

namespace Housing.Logic
{
    public abstract class SimplePlacement : MonoBehaviour, IPlacementLogic
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _debug;

        private NamespacedKey _idKey;

        public NamespacedKey id => _idKey;
        protected bool debug => _debug;

        public IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer)
        {
            if (_idKey == default) _idKey = NamespacedKey.TryParse(_id, out var id) ? id : default;
            var typeIndex = context.chunk.types.IndexOf(id);
            if (typeIndex < 0)
            {
                if (debug) Debug.Log("Type index " + _id + " is not present");
                yield break;
            }

            var typeId = (ushort)typeIndex;
            if (debug) Debug.Log("Place " + _id + " (" + typeId + ")");
            foreach (var m in Place(context, typeId, placer))
            {
                yield return m;
            }
        }

        protected abstract IEnumerable<ICombinableMesh> Place(HousingMeshChunkContext context, ushort typeId,
            Func<HousingMeshChunkContext, Vector3Int, Face, IEnumerable<ICombinableMesh>> placer);
    }
}