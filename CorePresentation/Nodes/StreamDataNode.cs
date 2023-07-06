using Core;
using Core.Nodes;
using System;
using System.IO;

namespace Verse3.Nodes
{
    //[Serializable]
    public class StreamDataNode : DataNodeElement<Stream>
    {
        public StreamDataNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
            
        }

        private DataStructure<Stream> _dataGoo = new DataStructure<Stream>();
        public new DataStructure<Stream> DataGoo
        {
            get => _dataGoo;
            set
            {
                try
                {
                    if (value != null && value is DataStructure<Stream>)
                    {
                        _dataGoo = value as DataStructure<Stream>;
                        //TODO: Round to decimal places
                        //double data = _dataGoo.Data;
                        //data = Math.Round(data, _decimalPlaces);
                    }
                    else
                    {
                        if (value != null)
                        {
                            if (value.Data.GetType().IsAssignableTo(typeof(Stream)))
                            {
                                _dataGoo = value.DuplicateAsType<Stream>();
                            }
                        }
                        _dataGoo = new DataStructure<Stream>();
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
