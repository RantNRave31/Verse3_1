using System;
using System.Runtime.Serialization;

namespace Verse3.Components
{
    [Serializable]
    internal class ShellComp : BaseCompViewModel, ISerializable
    {
        public SerializationInfo _info;
        public StreamingContext _context;
        public string _metadataCompInfo;
        public ShellComp() : base()
        {
        }

        public ShellComp(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _info = info;
            _context = context;
            _metadataCompInfo = info.GetString("MetadataCompInfo");
        }

        internal void ApplyObjectData(ref BaseCompViewModel comp)
        {
            //comp._cEManager = _info.GetValue("ChildElementManager", typeof(ChildElementManager)) as ChildElementManager;

            //comp._cEManager = new ChildElementManager(comp);

            //renderPipelineInfo = new RenderPipelineInfo(this);
            //computationPipelineInfo = new ComputationPipelineInfo(this);
            //comp.computationPipelineInfo = new ComputationPipelineInfo(comp);
            //computationPipelineInfo = info.GetValue("ComputationPipelineInfo", typeof(ComputationPipelineInfo)) as ComputationPipelineInfo;

            //this.boundingBox = new BoundingBox();


            comp.ID = (Guid)_info.GetValue("ID", typeof(Guid));
            comp.Name = _info.GetString("Name");
            //comp._metaDataCompInfo = _info.GetString("MetadataCompInfo");
            //CompInfo ci = this.GetCompInfo();

            //this.Accent = new SolidColorBrush(ci.Accent);
            //this.Background = new SolidColorBrush(Colors.Gray);
            //this.ElementType = (ElementType)info.GetValue("ElementType", typeof(ElementType));
            //this.State = (ElementState)info.GetValue("State", typeof(ElementState));
            //comp.BoundingBox = (BoundingBox)_info.GetValue("BoundingBox", typeof(BoundingBox));
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        //public ShellComp(BaseComp comp)
        //{
        //    this.BoundingBox = comp.BoundingBox;
        //    this.computationPipelineInfo = comp.ComputationPipelineInfo;
        //    this._cEManager = comp.ChildElementManager;
        //    this.ID = comp.ID;
        //    this._metadataCompInfo = comp.MetadataCompInfo;
        //    this.MetadataCompInfo = comp.MetadataCompInfo;
        //    this.Name = comp.Name;
        //}

        public override void Compute()
        {
        }

        public override CompInfo GetCompInfo() => CompInfo.FromString(this, _metadataCompInfo);

        public override void Initialize()
        {
        }

    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
