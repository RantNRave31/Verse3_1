using Core;
using Verse3.Components;
using Verse3.Nodes;

namespace IntegerLibrary.BasicOperations
{
    public class IntegerAddition : BaseCompViewModel
    {
        public IntegerAddition() : base()
        {
        }
        public IntegerAddition(int x, int y) : base(x, y)
        {
        }

        private IntegerDataNode A;
        private IntegerDataNode B;
        private IntegerDataNode Result;
        public override void Initialize()
        {
            //EVENT NODES

            //DATA NODES
            A = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(A, "A");

            B = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(B, "B");

            Result = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(Result, "Result", true);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "ADD(int)", "Basic Operations", "Integer");

        public override void Compute()
        {
            int a = ChildElementManager.GetData(A, 0);
            //DataStructure<double> aDS = this.ChildElementManager.GetData(A);
            //if (aDS is null || aDS.Data == default) aDS = new DataStructure<double>(0.0);
            int b = ChildElementManager.GetData(B, 0);
            //DataStructure<double> bDS = this.ChildElementManager.GetData(B);
            //if (bDS is null || bDS.Data == default) bDS = new DataStructure<double>(0.0);
            ChildElementManager.SetData(a + b, Result);
            //if (aDS.Data is null || bDS.Data is null) return;
            //DataStructure<double> result = new DataStructure<double>();
            //if (aDS.Count > 0)
            //{
            //    foreach (IDataGoo<double> goo in aDS)
            //    {
            //        if (bDS.Count > 0)
            //        {
            //            foreach (IDataGoo<double> goo2 in bDS)
            //            {
            //                result.Add(goo.Data + goo2.Data);
            //            }
            //        }
            //        else if (bDS.Data is double b)
            //        {
            //            result.Add(goo.Data + b);
            //        }
            //    }
            //}
            //else if (aDS.Data is double a)
            //{
            //    if (bDS.Count > 0)
            //    {
            //        foreach (IDataGoo<double> goo2 in bDS)
            //        {
            //            result.Add(a + goo2.Data);
            //        }
            //    }
            //    else if (bDS.Data is double b)
            //    {
            //        result.Add(a + b);
            //    }
            //}
        }
    }
}
