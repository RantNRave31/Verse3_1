using Core;
using System;
using System.Runtime.Serialization;
using Verse3.Elements;
using Newtonsoft.Json;
using Verse3.Nodes;
using Core.Elements;
using Core.Nodes;

namespace Verse3.Components
{
    [Serializable] //READ ONLY SERIALIZATION
    public class ChildElementManager : ISerializable
    {
        private BaseCompViewModel _owner;

        public ChildElementManager(BaseCompViewModel owner)
        {
            _owner = owner;
        }

        public ChildElementManager(SerializationInfo info, StreamingContext context)
        {
            if (info is null) throw new NullReferenceException("info is null");
            BaseCompViewModel owner = info.GetValue("Owner", typeof(BaseCompViewModel)) as BaseCompViewModel;
            ElementsLinkedList<ShellNode> inputNodes = (ElementsLinkedList<ShellNode>)info.GetValue("InputNodes", typeof(ElementsLinkedList<ShellNode>));
            ElementsLinkedList<ShellNode> outputNodes = (ElementsLinkedList<ShellNode>)info.GetValue("OutputNodes", typeof(ElementsLinkedList<ShellNode>));

            if (inputNodes != null && inputNodes.Count > 0)
            {
                int index = 0;
                foreach (ShellNode shell in inputNodes)
                {
                    if (shell.BezierElements != null && shell.BezierElements.Count > 0)
                    {
                        //foreach (BezierElement bezierElement in shell.BezierElements)
                        //{
                        //    bezierElement.
                        //}
                        //owner.ChildElementManager.InputNodes[index]
                    }
                    index++;
                }
            }

            //this._owner = (BaseComp)info.GetValue("Owner", typeof(BaseComp));
            //this.InputNodes = (ElementsLinkedList<INode>)info.GetValue("InputNodes", typeof(ElementsLinkedList<INode>));
            //this.OutputNodes = (ElementsLinkedList<INode>)info.GetValue("OutputNodes", typeof(ElementsLinkedList<INode>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Owner", _owner);
            info.AddValue("InputNodes", InputNodes);
            info.AddValue("OutputNodes", OutputNodes);
        }

        public void AdjustBounds(bool forceExpand = false)
        {
            if (_owner.RenderView is BaseCompModelView)
            {
                BaseCompModelView view = _owner.RenderView as BaseCompModelView;

                if (_owner.Width < 50 || _owner.Height < 50 || _owner.Width == double.NaN || _owner.Height == double.NaN || forceExpand)
                {
                    _owner.Width = 10000;
                    _owner.Height = 10000;
                    view.UpdateLayout();
                    //DataViewModel.WPFControl.ExpandContent();
                }

                //if (view.MainStackPanel.ActualWidth < 50 || view.MainStackPanel.ActualHeight < 50) return;
                //if (view.MainStackPanel.ActualWidth > 500 || view.MainStackPanel.ActualHeight > 500) return;
                if (_owner.Width != view.MainStackPanel.ActualWidth) _owner.Width = view.MainStackPanel.ActualWidth;
                if (_owner.Height != view.MainStackPanel.ActualHeight) _owner.Height = view.MainStackPanel.ActualHeight;
                //_owner.OnPropertyChanged("BoundingBox");
                //RenderPipeline.RenderRenderable(_owner);
                if (view.CenterBar.ActualWidth < view.ActualWidth - (view.InputsList.ActualWidth + view.OutputsList.ActualWidth))
                {
                    if (view.ActualWidth > view.InputsList.ActualWidth + view.OutputsList.ActualWidth)
                    {
                        double targetWidth = _owner.Width;
                        if (view.InputsList.ActualWidth + view.OutputsList.ActualWidth + view.CenterBar.ActualWidth >
                            view.BottomUI.ActualWidth)
                            targetWidth = view.InputsList.ActualWidth + view.OutputsList.ActualWidth + view.CenterBar.ActualWidth;
                        else targetWidth = view.BottomUI.ActualWidth;
                        if (_owner.Width != targetWidth)
                        {
                            _owner.Width = targetWidth;
                        }
                        view.CenterBar.Width = _owner.Width - (view.InputsList.ActualWidth + view.OutputsList.ActualWidth);
                    }
                }
                view.UpdateLayout();
            }
        }

        public void AddElement(IRenderable element)
        {
            DataTemplateManager.RegisterDataTemplate(element);
            _owner.RenderPipelineInfo.AddChild(element);
            switch (element.ElementType)
            {
                case ElementType.UIElement:
                    {
                        _bottomUI.Add(element);
                        break;
                    }
                case ElementType.DisplayUIElement:
                    {
                        _center.Add(element);
                        break;
                    }
                case ElementType.Node:
                    {
                        if (element is INode)
                        {
                            INode node = element as INode;
                            if (node.NodeType == NodeType.Input)
                            {
                                _input.Add(element);
                            }
                            else if (node.NodeType == NodeType.Output)
                            {
                                _output.Add(element);
                            }
                        }
                        else if (element is AddRemoveNodeButtonElementViewModel arnbe)
                        {
                            _input.Add(arnbe);
                        }
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
            //AdjustBounds();
            RenderingCore.Render(_owner);
            MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ExpandContent();
        }
        public void RemoveElement(IRenderable element)
        {
            switch (element.ElementType)
            {
                case ElementType.UIElement:
                    {
                        _bottomUI.Remove(element);
                        break;
                    }
                case ElementType.DisplayUIElement:
                    {
                        _center.Remove(element);
                        break;
                    }
                case ElementType.Node:
                    {
                        if (element is INode)
                        {
                            INode node = element as INode;
                            if (node.NodeType == NodeType.Input)
                            {
                                _input.Remove(element);
                            }
                            else if (node.NodeType == NodeType.Output)
                            {
                                _output.Remove(element);
                            }
                        }
                        break;
                    }
                default:
                    {

                        break;
                    }
            }
        }

        #region Add/Remove Individual Nodes

        public int AddDataOutputNode<T>(IDataNode<T> node, string name = "", bool isPrimaryOutput = false)
        {
            node.Name = name;
            if (node is IRenderable) AddElement(node as IRenderable);
            int outInt = _owner.ComputationPipelineInfo.IOManager.AddDataOutputNode(node);
            if (isPrimaryOutput)
            {
                _owner.ComputationPipelineInfo.IOManager.PrimaryDataOutput = outInt;
            }
            return outInt;
        }

        public int AddDataInputNode<T>(IDataNode<T> node, string name = "")
        {
            node.Name = name;
            if (node is IRenderable) AddElement(node as IRenderable);
            return _owner.ComputationPipelineInfo.IOManager.AddDataInputNode(node);
        }

        public int AddExpandableDataInputNode<T>(IDataNode<T> node, string name = "", bool rearrangable = false)
        {
            node.Name = name;
            if (node is IRenderable) AddElement(node as IRenderable);
            AddRemoveNodeButtonElementViewModel addRemoveNode = new AddRemoveNodeButtonElementViewModel(node, rearrangable, true);
            addRemoveNode.OnAddClicked += (s, e) =>
            {
                if (rearrangable)
                {
                    //TODO: Add rearrangable node
                }
                else
                {
                    //TODO: Add non-rearrangable node
                }
            };
            AddElement(addRemoveNode);
            return _owner.ComputationPipelineInfo.IOManager.AddDataInputNode(node);
        }

        public int AddEventOutputNode(IEventNode node, string name = "")
        {
            node.Name = name;
            if (node is IRenderable) AddElement(node as IRenderable);
            return _owner.ComputationPipelineInfo.IOManager.AddEventOutputNode(node);
        }

        public int AddEventInputNode(IEventNode node, string name = "")
        {
            node.Name = name;
            if (node is IRenderable) AddElement(node as IRenderable);
            return _owner.ComputationPipelineInfo.IOManager.AddEventInputNode(node);
        }

        public void RemoveNode(INode node)
        {
            if (node is IRenderable) RemoveElement(node as IRenderable);
            _owner.ComputationPipelineInfo.IOManager.RemoveNode(node);
        }

        #endregion

        public T GetData<T>(DataNode<T> node, T defaultValue = default)
        {
            try
            {
                DataStructure<T> ds = GetData(node);
                if (ds is null) return defaultValue;
                if (ds.Data is T castData)
                {
                    return castData;
                }
                else if (ds.Data is T[] castArray && castArray.Length == 1)
                {
                    return castArray[0];
                }
                else if (ds.Data == default)
                {
                    return defaultValue;
                }
                else
                {
                    Exception ex = new Exception("Data is not a single item / type mismatch");
                    CoreConsole.Log(ex);
                }
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
            return defaultValue;
        }

        public DataStructure<T> GetData<T>(DataNode<T> node)
        {
            if (node == null) return null;
            //if (node.DataValueType == typeof(T))
            //{
            if (node.DataGoo != null)
            {
                if (node.DataGoo.DataType.IsAssignableTo(typeof(T)))
                {
                    //return node.DataGoo.Duplicate<T>();
                    return node.DataGoo;
                }
                else if (node.DataGoo.DataType.IsAssignableTo(typeof(object)))
                {
                    return (node.DataGoo as DataStructure).DuplicateAsType<T>();
                }
                else
                {
                    Exception ex = new Exception("Data type mismatch");
                    CoreConsole.Log(ex);
                }
            }
            //}
            return null;
        }

        public bool SetData<T>(object data, DataNode<T> node)
        {
            if (data is null) return false;
            if (node is null) return false;
            if (data is DataStructure ds)
            {
                if (data is DataStructure<T> dsT)
                    return SetData(dsT, node);
                else
                    return SetData(ds.DuplicateAsType<T>(), node);
            }
            else if (data is T || data.GetType().IsAssignableTo(typeof(T)))
            {
                try
                {
                    return SetData(new DataStructure<T>((T)data), node);
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
            else
            {
                try
                {
                    T tryCastData = (T)data;
                    return SetData(new DataStructure<T>(tryCastData), node);
                }
                catch (Exception ex1)
                {
                    CoreConsole.Log(ex1);
                }
            }
            return false;
        }

        private bool SetData<T>(DataStructure<T> data, DataNode<T> node)
        {
            try
            {
                if (data is null) return false;
                if (node is null) return false;
                //if (data is EventArgData eData)
                //{
                //    if (eData.Data is DataStructure eds)
                //    {
                //        node.DataGoo = eds.Duplicate<T>();
                //    }
                //}
                else
                {
                    node.DataGoo = data;
                    return true;
                }
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                return false;
            }
        }

        public ElementsLinkedList<IRenderable> FilterChildElementsByType(ElementType elementType)
        {
            ElementsLinkedList<IRenderable> renderables = new ElementsLinkedList<IRenderable>();
            foreach (IRenderable renderable in _owner.Children)
            {
                if (renderable.ElementType == elementType)
                {
                    //TODO: Log to console
                    //if (renderable is INode) continue;
                    /*else */
                    renderables.Add(renderable);
                }
            }
            return renderables;
        }

        //public void EventOccured(int v, EventArgData eventArgData)
        //{
        //    this._owner.ComputationPipelineInfo.IOManager.EventOccured(v, eventArgData);
        //}

        public void EventOccured(EventNode node, EventArgData eventArgData)
        {
            node.EventOccured(eventArgData);
        }


        private ElementsLinkedList<IRenderable> _input = new ElementsLinkedList<IRenderable>();
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> InputSide
        {
            get
            {
                //foreach (IRenderable renderable in _owner.Children)
                //{
                //    if (renderable.ElementType == ElementType.Node)
                //    {
                //        if (renderable is INode)
                //        {
                //            INode node = renderable as INode;
                //            if (node.NodeType == NodeType.Input)
                //            {
                //                _input.Add(renderable);
                //            }
                //        }
                //    }
                //}
                return _input;
            }
        }
        internal ElementsLinkedList<INode> InputNodes
        {
            get
            {
                ElementsLinkedList<INode> nodes = new ElementsLinkedList<INode>();
                if (_input != null && _input.Count > 0)
                    foreach (IRenderable renderable in _input)
                    {
                        if (renderable is INode node)
                        {
                            nodes.Add(node);
                        }
                    }
                return nodes;
            }
            set
            {
                //DO NOT SET ANY VALUES
                //if (value != null && value.Count > 0)
                //{
                //    foreach (ShellNode shellNode in value)
                //    {
                //        if (shellNode != null)
                //        {

                //        }
                //    }
                //}
            }
        }

        private ElementsLinkedList<IRenderable> _output = new ElementsLinkedList<IRenderable>();
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> OutputSide
        {
            get
            {
                //foreach (IRenderable renderable in _owner.Children)
                //{
                //    if (renderable.ElementType == ElementType.Node)
                //    {
                //        if (renderable is INode)
                //        {
                //            INode node = renderable as INode;
                //            if (node.NodeType == NodeType.Output)
                //            {
                //                _output.Add(renderable);
                //            }
                //        }
                //    }
                //}
                return _output;
            }
        }
        internal ElementsLinkedList<INode> OutputNodes
        {
            get
            {
                ElementsLinkedList<INode> nodes = new ElementsLinkedList<INode>();
                if (_output != null && _output.Count > 0)
                    foreach (IRenderable renderable in _output)
                    {
                        if (renderable is INode node)
                        {
                            nodes.Add(node);
                        }
                    }
                return nodes;
            }
            set
            {
                //DO NOT SET ANY VALUES
                //if (value != null && value.Count > 0)
                //{
                //    foreach (ShellNode shellNode in value)
                //    {
                //        if (shellNode != null)
                //        {

                //        }
                //    }
                //}
            }
        }

        private ElementsLinkedList<IRenderable> _bottomUI = new ElementsLinkedList<IRenderable>();
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> BottomUIItems
        {
            get
            {
                //foreach (IRenderable renderable in _owner.Children)
                //{
                //    if (renderable.ElementType == ElementType.UIElement)
                //    {
                //        //TODO: Log to console
                //        if (renderable is INode) continue;
                //        else _bottomUI.Add(renderable);
                //    }
                //}
                return _bottomUI;
            }
        }

        private ElementsLinkedList<IRenderable> _center = new ElementsLinkedList<IRenderable>();
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> CenterBarItems
        {
            get
            {
                //foreach (IRenderable renderable in _owner.Children)
                //{
                //    if (renderable.ElementType == ElementType.DisplayUIElement)
                //    {
                //        //TODO: Log to console
                //        if (renderable is INode) continue;
                //        else _center.Add(renderable);
                //    }
                //}
                return _center;
            }
        }
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
