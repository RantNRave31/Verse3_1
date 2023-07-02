using Core.Elements;
using Core.Nodes;
using System;
using System.Windows;

namespace Verse3.Elements
{
    [Serializable]
    public class AddRemoveNodeButtonElementViewModel : BaseElement
    {

        #region Properties

        public event EventHandler<RoutedEventArgs> OnRemoveClicked;
        public event EventHandler<RoutedEventArgs> OnAddClicked;
        public event EventHandler<RoutedEventArgs> OnMoveUpClicked;
        public event EventHandler<RoutedEventArgs> OnMoveDownClicked;

        public override Type ViewType => typeof(AddRemoveNodeButtonElementModelView);

        public override ElementType ElementType { get => ElementType.Node; set => base.ElementType = ElementType.Node; }

        public bool AllowRearrangement { get; internal set; }
        public bool IsFirst { get; private set; }

        private INode _owner;

        #endregion

        #region Constructors
        
        //public AddRemoveNodeButtonElement() : base()
        //{
        //    this.AllowRearrangement = false;
        //}
        
        public AddRemoveNodeButtonElementViewModel(IDataNode ownerNode, bool allowRearrangement = false, bool isFirst = true) : base()
        {
            this.AllowRearrangement = allowRearrangement;
            this.IsFirst = isFirst;
            this._owner = ownerNode;
        }

        #endregion
        
        internal void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsFirst) OnRemoveClicked.Invoke(sender, e);
        }
        internal void AddClicked(object sender, RoutedEventArgs e)
        {
            OnAddClicked.Invoke(sender, e);
        }
        internal void MoveUpClicked(object sender, RoutedEventArgs e)
        {
            if (this.AllowRearrangement) OnMoveUpClicked.Invoke(sender, e);
        }
        internal void MoveDownClicked(object sender, RoutedEventArgs e)
        {
            if (this.AllowRearrangement) OnMoveDownClicked.Invoke(sender, e);
        }

    }
}