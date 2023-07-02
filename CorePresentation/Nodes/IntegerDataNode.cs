using Core;
using Core.Nodes;
using System;

namespace Verse3.Nodes
{
    //[Serializable]
    public class IntegerDataNode : DataNodeElement<int>
    {
        //TODO: Properties like Accept Integers Only, Accept Decimals Only, etc.
        public IntegerDataNode(IRenderable parent, NodeType type = NodeType.Unset, int decimalPlaces = 0) : base(parent, type)
        {
            DecimalPlaces = decimalPlaces;
        }

        private int _decimalPlaces = 0;
        public int DecimalPlaces { get => _decimalPlaces; private set => _decimalPlaces = value; }
        private DataStructure<int> _dataGoo = new DataStructure<int>();
        public new DataStructure<int> DataGoo
        {
            get => _dataGoo;
            set
            {
                try
                {
                    if (value != null && value is DataStructure<int>)
                    {
                        _dataGoo = value as DataStructure<int>;
                        //TODO: Round to decimal places
                        //double data = _dataGoo.Data;
                        //data = Math.Round(data, _decimalPlaces);
                    }
                    else
                    {
                        if (value != null)
                        {
                            if (value.Data.GetType().IsAssignableTo(typeof(int)))
                            {
                                _dataGoo = value.DuplicateAsType<int>();
                            }
                        }
                        _dataGoo = new DataStructure<int>();
                    }
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
        }
        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
