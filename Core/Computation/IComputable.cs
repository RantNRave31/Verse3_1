using Core.Elements;
using Core.Nodes;
using Newtonsoft.Json;

namespace Core
{
    public interface IComputable : IElement
    {
        public void OnLog_Internal(EventArgData e);
        //[JsonIgnore]
        public ComputationPipelineInfo ComputationPipelineInfo { get; }

        //public ElementsLinkedList<INode> Nodes { get; }

        bool CollectData();

        [JsonIgnore]
        public ComputableElementState ComputableElementState { get; set; }
        //public ElementConsole Console { get; }
        //public bool Enabled { get; set; }
        //void ClearData();
        //{
        //TODO: Populate DataDS and DataUS and Collect data from nodes
        //}
        void Compute();
        void DeliverData();
    }
}
