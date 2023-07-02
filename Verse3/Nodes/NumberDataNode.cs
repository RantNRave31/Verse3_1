using Core;
using Core.Nodes;
using System;

namespace Verse3.Nodes
{
    //[Serializable]
    public class NumberDataNode : DataNodeElement<double>
    {
        //TODO: Properties like Accept Integers Only, Accept Decimals Only, etc.
        public NumberDataNode(IRenderable parent, NodeType type = NodeType.Unset, int decimalPlaces = 2) : base(parent, type)
        {
            DecimalPlaces = decimalPlaces;
        }

        private int _decimalPlaces = 2;
        public int DecimalPlaces { get => _decimalPlaces; private set => _decimalPlaces = value; }
        private DataStructure<double> _dataGoo = new DataStructure<double>();
        public new DataStructure<double> DataGoo
        {
            get => _dataGoo;
            set
            {
                try
                {
                    if (value != null && value is DataStructure<double>)
                    {
                        _dataGoo = value as DataStructure<double>;
                        //TODO: Round to decimal places
                        //double data = _dataGoo.Data;
                        //data = Math.Round(data, _decimalPlaces);
                    }
                    else
                    {
                        if (value != null)
                        {
                            if (value.Data.GetType().IsAssignableTo(typeof(double)))
                            {
                                _dataGoo = value.DuplicateAsType<double>();
                            }
                        }
                        _dataGoo = new DataStructure<double>();
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
