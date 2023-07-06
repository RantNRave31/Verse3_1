using Core.Elements;
using Core.Nodes;
using System;
using System.Collections.Generic;

namespace Core
{
    public class IOManager
    {
        private IComputable _computable;
        private ElementsLinkedList<IDataNode> _dataInputNodes = new ElementsLinkedList<IDataNode>();
        public ElementsLinkedList<IDataNode> DataInputNodes => _dataInputNodes;

        private ElementsLinkedList<IDataNode> _dataOutputNodes = new ElementsLinkedList<IDataNode>();
        public ElementsLinkedList<IDataNode> DataOutputNodes => _dataOutputNodes;
        private ElementsLinkedList<IEventNode> _eventInputNodes = new ElementsLinkedList<IEventNode>();
        public ElementsLinkedList<IEventNode> EventInputNodes => _eventInputNodes;
        private ElementsLinkedList<IEventNode> _eventOutputNodes = new ElementsLinkedList<IEventNode>();
        public ElementsLinkedList<IEventNode> EventOutputNodes => _eventOutputNodes;
        private int _primaryDataOutput = -1;
        public int PrimaryDataOutput
        {
            get
            {
                return _primaryDataOutput;
            }
            set
            {
                if (_dataOutputNodes.ItemAtIndex(value) != null)
                {
                    _primaryDataOutput = value;
                }
                else _primaryDataOutput = -1;
            }
        }
        //public int ConnectionCount
        //{
        //    get
        //    {
        //        int count = 0;
        //        try
        //        {
        //            foreach (IDataNode<object> dataInputNode in _dataInputNodes)
        //            {
        //                if (dataInputNode.Connections.Count > 0) count += dataInputNode.Connections.Count;
        //            }
        //            foreach (IDataNode<object> dataOutputNode in _dataOutputNodes)
        //            {
        //                if (dataOutputNode.Connections.Count > 0) count += dataOutputNode.Connections.Count;
        //            }
        //        }
        //        catch
        //        { 
        //            //TODO: LOG to console
        //        }
        //        return count;
        //    }
        //}

        public IOManager(IComputable computable)
        {
            this._computable = computable;
            //this._dataInputNodes = computable.ComputationPipelineInfo.DataUS;
            //this._dataOutputNodes = computable.ComputationPipelineInfo.DataDS;
            //this._eventInputNodes = computable.ComputationPipelineInfo.EventUS;
            //this._eventOutputNodes = computable.ComputationPipelineInfo.EventDS;
        }

        public void EventOccured(int v, EventArgData eventArgData)
        {
            this.EventOutputNodes[v].EventOccured(eventArgData);
        }

        public void RemoveNode(INode node)
        {
            if (this.EventInputNodes.Contains(node as IEventNode))
            {
                this.EventInputNodes.Remove(node as IEventNode);
                return;
            }
            else if (this.EventOutputNodes.Contains(node as IEventNode))
            {
                this.EventOutputNodes.Remove(node as IEventNode);
                return;
            }
            else if (this.DataInputNodes.Contains(node as IDataNode))
            {
                this.DataInputNodes.Remove(node as IDataNode);
                return;
            }
            else if (this.DataOutputNodes.Contains(node as IDataNode))
            {
                this.DataOutputNodes.Remove(node as IDataNode);
                return;
            }
        }

        private void OnDataChanged<T>(IDataNode<T> container, DataChangedEventArgs<T> e)
        {
            if (container.NodeType == NodeType.Input)
            {
                if (container.Parent is IComputable) ComputationCore.Compute(container.Parent as IComputable);
            }
            else if (container.NodeType == NodeType.Output)
            {
                if (container.Parent is IComputable)
                {
                    IComputable c = container.Parent as IComputable;
                    c.ComputationPipelineInfo.IOManager.DeliverData();
                }
            }
        }
        private void OnEvent(IEventNode container, EventArgData e)
        {
            if (container.NodeType == NodeType.Input)
            {
                //Call EventOccured() delegate
                bool gate = container.EventOccured(e);
                if (gate)
                {
                    if (container.Parent is IComputable) ComputationCore.Compute(container.Parent as IComputable);
                }
            }
            else if (container.NodeType == NodeType.Output)
            {
                if (container.Parent is IComputable)
                {
                    IComputable c = container.Parent as IComputable;
                    c.ComputationPipelineInfo.IOManager.DeliverData();
                }
            }
        }

        public int AddDataInputNode<T>(IDataNode<T> dataInputNode)
        {
            if (dataInputNode is null) return default;
            if (!this._dataInputNodes.Contains(dataInputNode))
            {
                if (dataInputNode.NodeType == NodeType.Input)
                {
                    dataInputNode.Parent = _computable;
                    this._dataInputNodes.Add(dataInputNode);
                    int i = this._dataInputNodes.IndexOf(dataInputNode);
                    (this._dataInputNodes[i] as IDataNode<T>).NodeDataChanged += OnDataChanged;
                    return i;
                }
            }
            return default;
        }

        public int AddDataOutputNode<T>(IDataNode<T> dataOutputNode)
        {
            if (dataOutputNode is null) return default;
            if (!this._dataOutputNodes.Contains(dataOutputNode))
            {
                if (dataOutputNode.NodeType == NodeType.Output)
                {
                    dataOutputNode.Parent = _computable;
                    this._dataOutputNodes.Add(dataOutputNode);
                    int i = this._dataOutputNodes.IndexOf(dataOutputNode);
                    (this._dataOutputNodes[i] as IDataNode<T>).NodeDataChanged += OnDataChanged;
                    return i;
                }
            }
            return default;
        }
        public int AddEventInputNode(IEventNode eventInputNode, EventDelegate eventDelegate = null)
        {
            if (eventInputNode is null) return default;
            if (!this._eventInputNodes.Contains(eventInputNode))
            {
                if (eventInputNode.NodeType == NodeType.Input)
                {
                    eventInputNode.Parent = _computable;
                    this._eventInputNodes.Add(eventInputNode);
                    int i = this._eventInputNodes.IndexOf(eventInputNode);
                    (this._eventInputNodes[i] as IEventNode).NodeEvent += OnEvent;
                    if (eventDelegate != null)
                    {
                        EventDelegates.Add(eventDelegate);
                    }
                }
            }
            return default;
        }

        public int AddEventOutputNode(IEventNode eventOutputNode, EventDelegate eventDelegate = null)
        {
            if (eventOutputNode is null) return default;
            if (!this._eventOutputNodes.Contains(eventOutputNode))
            {
                if (eventOutputNode.NodeType == NodeType.Output)
                {
                    eventOutputNode.Parent = _computable;
                    this._eventOutputNodes.Add(eventOutputNode);
                    int i = this._eventOutputNodes.IndexOf(eventOutputNode);
                    (this._eventOutputNodes[i] as IEventNode).NodeEvent += OnEvent;
                    if (eventDelegate != null)
                    {
                        EventDelegates.Add(eventDelegate);
                    }
                }
            }
            return default;
        }

        public object GetData(int index)
        {
            if (index >= this.DataInputNodes.Count || index < 0) return default;
            return this._dataInputNodes[index].DataGoo.Data;
        }
        public Type GetData(out object output, int index)
        {
            if (index >= this.DataInputNodes.Count || index < 0)
            {
                output = default;
                return default;
            }
            output = this._dataInputNodes[index].DataGoo.Data;
            if (output != null)
            {
                return this._dataInputNodes[index].DataGoo.Data.GetType();
            }
            else return null;
        }
        public T GetData<T>(int index)
        {
            if (index >= this.DataInputNodes.Count || index < 0) return default;
            if (this._dataInputNodes[index].DataValueType == typeof(T))
            {
                if (this._dataInputNodes[index].DataGoo.IsValid && this._dataInputNodes[index].DataGoo.Data != null)
                {
                    object ret = this._dataInputNodes[index].DataGoo.Data;
                    if (ret is T castData)
                    {
                        return castData;
                    }
                    else if (ret is IDataGoo<T> goo)
                    {
                        return goo.Data;
                    }
                    else if (ret is object[] array)
                    {
                        if (array.Length == 1)
                        {
                            if (array[0] is T)
                            {
                                return (T)array[0];
                            }
                        }
                    }
                    else
                    {
                        Exception ex = new Exception("Data type mismatch");
                        CoreConsole.Log(ex);
                    }
                }
            }
            return default;
        }
        public T GetData<T>(int index, T defaultValue = default)
        {
            if (index >= this.DataInputNodes.Count || index < 0) return defaultValue;
            if (this._dataInputNodes[index].DataValueType == typeof(T))
            {
                if (this._dataInputNodes[index].DataGoo.IsValid && this._dataInputNodes[index].DataGoo.Data != null)
                {
                    if (this._dataInputNodes[index].DataGoo.Data is T)
                    {
                        object ret = this._dataInputNodes[index].DataGoo.Data;
                        if (ret is T castData)
                        {
                            return castData;
                        }
                        else if (ret is IDataGoo<T> goo)
                        {
                            return goo.Data;
                        }
                        else if (ret is object[] array)
                        {
                            if (array.Length == 1)
                            {
                                if (array[0] is T)
                                {
                                    return (T)array[0];
                                }
                            }
                        }
                        else
                        {
                            Exception ex = new Exception("Data type mismatch");
                            CoreConsole.Log(ex);
                        }
                    }
                    else
                    {
                        Exception ex = new Exception("Data type mismatch");
                        CoreConsole.Log(ex);
                    }
                }
            }
            return defaultValue;
        }
        public bool GetData<T>(out object output, int index)
        {
            try
            {
                if (index >= this.DataInputNodes.Count || index < 0)
                {
                    output = default;
                    return default;
                }
                if (this._dataInputNodes[index].DataValueType == typeof(T))
                {
                    output = (this._dataInputNodes[index].DataGoo as DataStructure<T>).Data;
                    object ret = this._dataInputNodes[index].DataGoo.Data;
                    if (ret is T castData)
                    {
                        output = castData;
                        return true;
                    }
                    else if (ret is IDataGoo<T> goo)
                    {
                        output = goo.Data;
                        return true;
                    }
                    else if (ret is object[] array)
                    {
                        if (array.Length == 1)
                        {
                            if (array[0] is T)
                            {
                                output = (T)array[0];
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Exception ex = new Exception("Data type mismatch");
                        CoreConsole.Log(ex);
                    }
                    return false;
                }
                else
                {
                    output = default;
                    return false;
                }
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                output = default;
                return false;
            }
        }

        //---------------------------
        
        public bool SetData(object data, int index)
        {
            if (index >= this.DataOutputNodes.Count || index < 0) return false;
            if (data is null) return false;
            if (this._dataOutputNodes[index].DataValueType.IsAssignableFrom(data.GetType()))
            {
                this._dataOutputNodes[index].DataGoo.Data = data;
                return true;
            }
            else return false;
        }
        public bool SetData(DataStructure data, int index)
        {
            if (index >= this.DataOutputNodes.Count || index < 0) return false;
            if (data is null || !data.IsValid) return false;
            if (this._dataOutputNodes[index].DataValueType.IsAssignableFrom(data.DataType))
            {
                this._dataOutputNodes[index].DataGoo = data;
                return true;
            }
            else return false;
        }
        public bool SetData<T>(T data, int index)
        {
            if (index >= this.DataOutputNodes.Count || index < 0) return false;
            if (data is null) return false;
            if (typeof(DataStructure<T>).IsAssignableFrom(data.GetType())) return SetData<T>(data as DataStructure<T>, index);
            if (this._dataOutputNodes[index].DataValueType.IsAssignableFrom(data.GetType()))
            {
                (this._dataOutputNodes[index].DataGoo as DataStructure<T>).Data = data;
                return true;
            }
            else return false;
        }
        public bool SetData<T>(DataStructure<T> data, int index)
        {
            if (index >= this.DataOutputNodes.Count || index < 0) return false;
            if (data is null || !data.IsValid) return false;
            if (this._dataOutputNodes[index].DataValueType == data.DataType)
            {
                this._dataOutputNodes[index].DataGoo = data;
                return true;
            }
            else return false;
        }

        public bool CollectData()
        {
            bool result = false;
            foreach (IDataNode dataInputNode in this._dataInputNodes)
            {
                //TODO: ONLY WHEN/IF CONNECTIONS CHANGED
                if (dataInputNode.Connections != null && dataInputNode.Connections.Count > 0)
                {
                    foreach (IConnection conn in dataInputNode.Connections)
                    {
                        if (conn.Destination == dataInputNode && conn.Origin.Parent is IComputable)
                        {
                            if (!this._computable.ComputationPipelineInfo.DataUS.Contains(conn.Origin.Parent as IComputable))
                            {
                                this._computable.ComputationPipelineInfo.DataUS.Add(conn.Origin.Parent as IComputable);
                            }
                        }
                    }
                }
                dataInputNode.CollectData();
            }
            return result;
        }
        public void DeliverData()
        {
            foreach (IDataNode dataOutputNode in this._dataOutputNodes)
            {
                //TODO: ONLY WHEN?IF CONNECTIONS CHANGED
                if (dataOutputNode.Connections != null && dataOutputNode.Connections.Count > 0)
                {
                    foreach (IConnection conn in dataOutputNode.Connections)
                    {
                        if (conn.Origin == dataOutputNode && conn.Destination.Parent is IComputable)
                        {
                            if (!this._computable.ComputationPipelineInfo.DataDS.Contains(conn.Destination.Parent as IComputable))
                            {
                                this._computable.ComputationPipelineInfo.DataDS.Add(conn.Destination.Parent as IComputable);
                            }
                        }
                    }
                }
                dataOutputNode.DeliverData();
            }
        }

        //private Foo(IComputable computable)
        //{
        //    EventDelegate eventDelegate = Bar;
        //}

        public delegate void EventDelegate(IEventNode container, EventArgData e);
        public List<EventDelegate> EventDelegates = new List<EventDelegate>();
    }
}
