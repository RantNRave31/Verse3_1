using Core.Elements;
using Newtonsoft.Json;
using System;

namespace Core
{
    public class RenderPipelineInfo
    {
        private IRenderable _renderable;
        [JsonIgnore]
        public IRenderable Renderable => _renderable;
        //private IRenderable _zPrev;
        //public IRenderable ZPrev => _zPrev;
        //private IRenderable _zNext;
        //public IRenderable ZNext => _zNext;
        [JsonIgnore]
        private IRenderable _parent;
        [JsonIgnore]
        public IRenderable Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        [JsonIgnore]
        private ElementsLinkedList<IRenderable> _children = new ElementsLinkedList<IRenderable>();
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> Children => _children;

        public RenderPipelineInfo(IRenderable renderable)
        {
            this._renderable = renderable;
        }

        public void AddChild(IRenderable child)
        {
            if (!this.Children.Contains(child))
            {
                this.Children.Add(child);
                child.RenderPipelineInfo.SetParent(Renderable);
            }
        }
        public void SetParent(IRenderable parent)
        {
            this.Parent = parent;
            if (this.Parent != null)
            {
                if (!this.Parent.Children.Contains(Renderable))
                {
                    this.Parent.Children.Add(Renderable);
                }
            }
        }
        public void SetParent(IElement parent)
        {
            if (parent is IRenderable)
            {
                this.SetParent(parent as IRenderable);
            }
            else throw new Exception("Parent is not a renderable");
        }
    }
}
