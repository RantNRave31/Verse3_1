using Core.Elements;
using Newtonsoft.Json;
using System;

namespace Core
{
    [Serializable]
    public class ComputationPipelineInfo
    {
        [JsonIgnore]
        private IComputable _computable;
        [JsonIgnore]
        private IOManager _ioManager;
        [JsonIgnore]
        public IOManager IOManager => _ioManager;
        //[JsonIgnore]
        private ElementsLinkedList<IComputable> _dataDS = new ElementsLinkedList<IComputable>();
        public ElementsLinkedList<IComputable> DataDS => _dataDS;

        //[JsonIgnore]
        private ElementsLinkedList<IComputable> _dataUS = new ElementsLinkedList<IComputable>();
        public ElementsLinkedList<IComputable> DataUS => _dataUS;

        //[JsonIgnore]
        private ElementsLinkedList<IComputable> _eventDS = new ElementsLinkedList<IComputable>();
        public ElementsLinkedList<IComputable> EventDS => _eventDS;

        //[JsonIgnore]
        private ElementsLinkedList<IComputable> _eventUS = new ElementsLinkedList<IComputable>();
        public ElementsLinkedList<IComputable> EventUS => _eventUS;

        [JsonIgnore]
        private ComputableElementState computableElementState = ComputableElementState.Default;
        public ComputableElementState ComputableElementState { get => computableElementState; internal set => computableElementState = value; }

        public ComputationPipelineInfo(IComputable computable)
        {
            this._computable = computable;
            this._ioManager = new IOManager(computable);
        }
        public void AddDataUpStream(IComputable dataUS)
        {
            if (dataUS is null) return;
            if (!this._dataUS.Contains(dataUS))
            {
                this._dataUS.Add(dataUS);
                dataUS.ComputationPipelineInfo.AddDataDownStream(_computable);
            }
        }
        public void AddDataDownStream(IComputable dataDS)
        {
            if (dataDS is null) return;
            if (!this._dataDS.Contains(dataDS))
            {
                this._dataDS.Add(dataDS);
                dataDS.ComputationPipelineInfo.AddDataUpStream(_computable);
            }
        }
        public void AddEventUpStream(IComputable eventUS)
        {
            if (eventUS is null) return;
            if (!this._eventUS.Contains(eventUS))
            {
                this._eventUS.Add(eventUS);
                eventUS.ComputationPipelineInfo.AddEventDownStream(_computable);
            }
        }
        public void AddEventDownStream(IComputable eventDS)
        {
            if (eventDS is null) return;
            if (!this._eventDS.Contains(eventDS))
            {
                this._eventDS.Add(eventDS);
                eventDS.ComputationPipelineInfo.AddEventUpStream(_computable);
            }
        }
        public bool CollectData()
        {
            return IOManager.CollectData();
            //if (this._dataUS.Count > 0)
            //{
            //    foreach (IComputable dataUS in this._dataUS)
            //    {
            //        if (dataUS.ComputableElementState == ComputableElementState.Computed)
            //        {
            //        }
            //        //else ComputationPipeline.ComputeComputable(dataUS);
            //    }
            //}
        }
        public void DeliverData()
        {
            IOManager.DeliverData();
            //if (this._computable.ComputableElementState == ComputableElementState.Computed)
            //{
            //    foreach (IComputable dataDS in this._dataDS)
            //    {
            //        if (dataDS.ComputableElementState == ComputableElementState.Computed)
            //        {
            //        }
            //    }
            //}
            //else ComputationPipeline.ComputeComputable(this._computable);
        }
    }
}
