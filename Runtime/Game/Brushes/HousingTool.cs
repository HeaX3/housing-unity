namespace Housing
{
    public abstract class HousingTool : IHousingTool
    {
        public abstract void Paint(HousingMesh mesh);
        public abstract void Clear();
        public abstract void Reset();
    }
}