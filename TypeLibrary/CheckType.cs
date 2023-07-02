using Core;
using Core.Nodes;
using System;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class CheckType : BaseCompViewModel
    {
        public CheckType() : base()
        {
        }
        public CheckType(int x, int y) : base(x, y)
        {
        }

        private GenericDataNode ObjectInput;
        private TypeDataNode TypeInput;
        private BooleanDataNode Result;
        public override void Initialize()
        {
            //EVENT NODES

            //DATA NODES
            ObjectInput = new GenericDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(ObjectInput, "Object");

            TypeInput = new TypeDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(TypeInput, "Type");

            Result = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(Result, "Result", true);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Check Type", "Type", "Type");

        public override void Compute()
        {
            DataStructure obj = this.ChildElementManager.GetData(ObjectInput);
            Type type = this.ChildElementManager.GetData(TypeInput, typeof(object));
            if (obj is null || type is null || obj.Data is null) return;
            bool result = obj.Data.GetType().IsAssignableTo(type);
            this.ChildElementManager.SetData(result, Result);
        }
    }
}
