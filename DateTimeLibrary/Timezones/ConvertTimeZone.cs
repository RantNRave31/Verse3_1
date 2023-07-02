﻿using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class ConvertTimeZone : BaseCompViewModel
    {

        #region Constructors

        public ConvertTimeZone() : base()
        {
        }

        public ConvertTimeZone(int x, int y) : base(x, y)
        {
        }

        #endregion


        public override CompInfo GetCompInfo() => new CompInfo(this, "Convert TimeZone", "Timezones", "DateTime");

        public override void Compute()
        {
            DateTime dateTimeSource = this.ChildElementManager.GetData(nodeBlock, DateTime.Now);
            TimeZoneInfo timezoneSource = this.ChildElementManager.GetData(nodeBlock1, TimeZoneInfo.Local);
            TimeZoneInfo timezoneTarget = this.ChildElementManager.GetData(nodeBlock2, TimeZoneInfo.Utc);

            DateTime dateTimeTarget = TimeZoneInfo.ConvertTime(dateTimeSource, timezoneSource, timezoneTarget);
            this.ChildElementManager.SetData(dateTimeTarget, nodeBlock3);
 

        }

        private DateTimeDataNode nodeBlock;
        private TimeZoneDataNode nodeBlock1;
        private TimeZoneDataNode nodeBlock2;
        private DateTimeDataNode nodeBlock3;



        public override void Initialize()
        {
            nodeBlock = new DateTimeDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "DateTime");

            nodeBlock1 = new TimeZoneDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "Source Timezone");

            nodeBlock2 = new TimeZoneDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock2, "Target Timezone");

            nodeBlock3 = new DateTimeDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock3, "Convert DateTime", true);


        }
    }
}
