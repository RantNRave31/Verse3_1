namespace Core
{
    public interface IRenderView
    {
        abstract IRenderable Element { get; }
        void Render();
    }
}
