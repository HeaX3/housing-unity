using System.Collections.Generic;

namespace Housing
{
    public class CompiledMesh
    {
        private readonly List<CompiledMeshChunk> _chunks = new();
        public IReadOnlyList<CompiledMeshChunk> chunks => _chunks;

        internal void AddChunk(CompiledMeshChunk chunk)
        {
            _chunks.Add(chunk);
        }
    }
}