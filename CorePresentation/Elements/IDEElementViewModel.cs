using Core.Elements;
using System;

namespace Verse3.Elements
{
    //[Serializable]
    public class IDEElementViewModel : BaseElement
    {
        public override Type ViewType => typeof(IDEElementModelView);

        public void SetScript(string v)
        {
            if (this.RenderView != null)
            {
                (this.RenderView as IDEElementModelView).SetScript(v);
            }
        }

        public override ElementType ElementType { get => ElementType.UIElement; set => base.ElementType = ElementType.UIElement; }

        public IDEElementViewModel() : base()
        {
        }

        public event EventHandler<EventArgs> ScriptChanged;
        
        public void TriggerScriptUpdate()
        {
            if (this.RenderView != null)
            {
                (this.RenderView as IDEElementModelView).GetScript();
            }
        }

        public void UpdateScript(string script)
        {
            _script = script;
            if (ScriptChanged != null)
            {
                ScriptChanged.Invoke(this, new EventArgs());
            }
        }

        internal string _script = "";
        public string Script
        {
            get
            {
                return _script;
            }
        }
    }
}
