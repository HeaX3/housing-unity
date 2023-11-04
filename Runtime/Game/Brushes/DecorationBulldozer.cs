using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Housing
{
    public class DecorationBulldozer : HousingTool
    {
        public override void Paint(HousingMesh mesh)
        {
            throw new System.NotImplementedException();
        }

        public override void Clear()
        {
            throw new System.NotImplementedException();
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }

        public static bool TryParse(JObject json, out DecorationBulldozer result)
        {
            result = new DecorationBulldozer();
            return true;
        }
    }
}