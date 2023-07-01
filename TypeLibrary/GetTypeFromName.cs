using Core;
using System;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class GetTypeFromName : BaseCompViewModel
    {
        public GetTypeFromName() : base()
        {
        }
        public GetTypeFromName(int x, int y) : base(x, y)
        {
        }

        private TextDataNode TypeNameInput;
        private TypeDataNode TypeOutput;
        public override void Initialize()
        {
            //EVENT NODES

            //DATA NODES
            TypeNameInput = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(TypeNameInput, "Type Name");

            TypeOutput = new TypeDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(TypeOutput, "Type", true);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Get Type from Name", "Type", "Type");

        public override void Compute()
        {
            string typeName = this.ChildElementManager.GetData(TypeNameInput, "");
            Type? type = Type.GetType(typeName);
            if (type is null) return;
            this.ChildElementManager.SetData(type, TypeOutput);
        }
    }
}
