using Core;
using System;
using Verse3.Nodes;
using Verse3.Components;

namespace MathLibrary
{
    public class HarmonicProgression : BaseCompViewModel
    {
        public HarmonicProgression() : base()
        {
        }
        public HarmonicProgression(int x, int y) : base(x, y)
        {
        }
        
        private NumberDataNode Start;
        private NumberDataNode Step;
        private NumberDataNode Count;
        private NumberDataNode Result;
        public override void Initialize()
        {
            //EVENT NODES

            //DATA NODES
            Start = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(Start, "Start");

            Step = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(Step, "Step");

            Count = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(Count, "Count");

            Result = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(Result, "HP", true);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Harmonic Progression", "Series", "Data");
        
        public override void Compute()
        {
            double start = this.ChildElementManager.GetData(Start, 1);
            double stepsize = this.ChildElementManager.GetData(Step, 1.0);
            int count = (int)Math.Abs(this.ChildElementManager.GetData(Count, 10));

           
            DataStructure<double> result = new DataStructure<double>();
            
            for (int i = 0; i < count; i++)
            {
                double supporter = start + (stepsize * i);
                if (supporter == 0)
                {
                    supporter += 1;
                }
                result.Add(1/supporter);
            }

            this.ChildElementManager.SetData(result, Result);
            if (count > 0)
            this.previewTextBlock.DisplayedText = $"Last number = {result.Last.Value.Data.ToString()}";
        }
    }
}
