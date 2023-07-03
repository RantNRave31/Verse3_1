using Core.Elements;

namespace Core
{
    public class RenderPipeline
    {
        private static RenderPipeline instance = new RenderPipeline();
        public static RenderPipeline Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderPipeline();
                }
                return RenderPipeline.instance;
            }
            protected set
            {
                instance = value;
            }
        }
        internal IRenderable _current;
        public IRenderable Current => _current;
        private RenderPipeline()
        {
            this._current = default;
        }
        public static int Render(ElementsLinkedList<IElement> _elementsBuffer)
        {
            int count = 0;
            try
            {
                //Parallel.ForEach(DataModel.Instance.Elements, e => { });
                //TODO: Multithreading
                //https://medium.com/@alex.puiu/parallel-foreach-async-in-c-36756f8ebe62
                foreach (IElement e in _elementsBuffer)
                {
                    if (e != null && e is IRenderable)
                    {
                        IRenderable renderable = e as IRenderable;
                        //if (RenderPipeline.Instance._current != default)
                        //{
                        //    RenderPipeline.Instance._current.ZNext = renderable;
                        //}
                        RenderPipeline.Instance._current = renderable;
                        if (RenderRenderable(renderable))
                        {
                            count++;
                        }
                        //renderable.Render();
                        //count++;
                        //if (renderable.Children.Count > 0)
                        //{
                        //    foreach (IRenderable child in renderable.Children)
                        //    {
                        //        child.Render();
                        //        count++;
                        //    }
                        //}
                    }
                }
            }
            catch /*(Exception e)*/
            {
                //TODO: Log to console
            }
            return count;
        }

        //TODO: Output int
        public static bool RenderRenderable(IRenderable renderable, double xOffset, double yOffset, bool recursive = true)
        {
            //if (renderable.RenderableState == RenderableState.Rendering) return -1;
            bool renderSuccess = true;
            try
            {
                if (renderable != null)
                {
                    renderable.SetX(renderable.X + xOffset);
                    renderable.SetY(renderable.Y + yOffset);
                    renderSuccess = renderSuccess && RenderPipeline.RenderRenderable(renderable);
                    if (recursive)
                    {
                        if (renderable.Children != null && renderable.Children.Count > 0)
                        {
                            foreach (IRenderable child in renderable.Children)
                            {
                                renderSuccess = renderSuccess && RenderPipeline.RenderRenderable(child, xOffset, yOffset);
                            }
                        }
                    }
                }
            }
            catch /*(Exception e)*/
            {
                //TODO: Log to console
                return false;
            }
            return renderSuccess;
        }

        //TODO: Output int
        public static bool RenderRenderable(IRenderable renderable, bool recursive = true)
        {
            bool renderSuccess = true;
            try
            {
                if (renderable != null)
                {
                    renderable.Render();
                    if (recursive)
                    {
                        if (renderable.Children != null && renderable.Children.Count > 0)
                        {
                            foreach (IRenderable child in renderable.Children)
                            {
                                renderSuccess = renderSuccess && RenderRenderable(child);
                            }
                        }
                    }
                }
            }
            catch /*(Exception e)*/
            {
                //TODO: Log to console
                return false;
            }
            return renderSuccess;
        }
    }
}
