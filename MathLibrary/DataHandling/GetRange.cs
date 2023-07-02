using Core;
using Core.Nodes;
using MS.WindowsAPICodePack.Internal;
using Verse3.Components;
using Verse3.Nodes;


namespace MathLibrary
{
    public class GetRange : BaseCompViewModel
    {
        public GetRange() : base()
        {
        }
        public GetRange(int x, int y) : base(x, y)
        {
        }
        
        private GenericDataNode DataStructureNode;

        private NumberDataNode Result;
        //private IEnumerable<object> sortedData;

        public override void Initialize()
        {
            //EVENT NODES

            //DATA NODES
            DataStructureNode = new GenericDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(DataStructureNode, "Data Structure");

            Result = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(Result, "Range", true);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Get Range", "Computation", "Data");
        
        public override void Compute()
        {
            DataStructure input = this.ChildElementManager.GetData(DataStructureNode);
            if (input == null || input.Count == 0) return;

            if (input[0].Data is double)
            {
                double? range = (double)input.Last.Value.Data - (double)input.First.Value.Data;
                this.ChildElementManager.SetData(range, Result);
                this.previewTextBlock.DisplayedText = $"Range = {range}";
            }

        }
    }
}
