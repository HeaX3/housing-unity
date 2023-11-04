using System.Collections.Generic;
using System.Linq;
using Essentials.Meshes;

namespace Housing
{
    public static class HousingMeshBuilderService
    {
        public static void Build(HousingMeshChunkContext context, HousingMeshChunkController controller)
        {
            // var stopwatch = new Stopwatch();
            // stopwatch.Start();
            var result = new List<ICombinableMesh>();
            foreach (var set in context.library.sets)
            {
                result.AddRange(set.Build(context));
            }

            var mesh = controller.mesh;
            mesh.Clear(keepVertexLayout: false);
            mesh.subMeshCount = 0;
            var merged = mesh.Merge(result.Where(p => p != null).ToArray());
            mesh.RecalculateBounds();
            mesh.UploadMeshData(false);
            controller.materials = merged.Materials;
            // var time = stopwatch.ElapsedMilliseconds;
            // Debug.Log("Compiling took " + time.ToString("N0") + "ms");
        }
    }
}