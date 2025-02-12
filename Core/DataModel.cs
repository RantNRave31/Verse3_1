﻿using Core.Elements;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Xml.Serialization;
using static Core.Geometry2D;

namespace Core
{

    /// <summary>
    /// A simple example of a data-model.  
    /// The purpose of this data-model is to share display data between the main window and overview window.
    /// </summary>
    ////[DataContract]
    [Serializable]
    //[XmlRoot("DataModel")]
    //[XmlType("DataModel")]
    public class DataModel : INotifyPropertyChanged/*, ISerializable*/
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Data Members

        /// <summary>
        /// The list of rectangles that is displayed both in the main window and in the overview window.
        /// </summary>
        [JsonIgnore]
        protected ElementsLinkedList<IElement> elements = new ElementsLinkedList<IElement>();

        ///
        /// The current scale at which the content is being viewed.
        /// 
        [JsonIgnore]
        protected double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        [JsonIgnore]
        protected double contentOffsetX = 0;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        [JsonIgnore]
        protected double contentOffsetY = 0;

        ///
        /// The width of the content (in content coordinates).
        /// 
        [JsonIgnore]
        protected double contentWidth = 0;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        [JsonIgnore]
        protected double contentHeight = 0;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        [JsonIgnore]
        protected double contentViewportWidth = 0;

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        [JsonIgnore]
        protected double contentViewportHeight = 0;

        #endregion Data Members

        #region Constructors

        /// <summary>
        /// Retreive the singleton instance.
        /// </summary>
        //[XmlIgnore]

        [JsonIgnore]
        public static double ContentCanvasMarginOffset = 0.0;

        protected DataModel() : base()
        {
            //
            // Initialize the data model.
            //
            //DataModel.instance = this;
        }

        //protected DataModel(SerializationInfo info, StreamingContext context) : base()
        //{
        //    //
        //    // Initialize the data model.
        //    //
        //    //DataModel.instance = this;
        //    this.elements = (ElementsLinkedList<IElement>)info.GetValue("elements", typeof(ElementsLinkedList<IElement>));
        //    //this.elements = (ElementsLinkedList<IElement>)info.GetValue("elements", typeof(ElementsLinkedList<ShellElement>));
        //    //this.contentScale = (double)info.GetValue("contentScale", typeof(double));
        //    //this.contentOffsetX = (double)info.GetValue("contentOffsetX", typeof(double));
        //    //this.contentOffsetY = (double)info.GetValue("contentOffsetY", typeof(double));
        //    //this.contentWidth = (double)info.GetValue("contentWidth", typeof(double));
        //    //this.contentHeight = (double)info.GetValue("contentHeight", typeof(double));
        //    //this.contentViewportWidth = (double)info.GetValue("contentViewportWidth", typeof(double));
        //    //this.contentViewportHeight = (double)info.GetValue("contentViewportHeight", typeof(double));
        //}

        #endregion

        #region Properties

        /// <summary>
        /// The list of rectangles that is displayed both in the main window and in the overview window.
        /// </summary>
        [JsonIgnore]
        public ElementsLinkedList<IElement> Elements
        {
            get
            {
                return elements;
            }
            protected set
            {
                elements = value;
            }
        }

        public IElement GetElementWithGuid(Guid guid)
        {
            return this.Elements.Find(guid).Value;
        }

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("elements", this.elements);
        //    //info.AddValue("contentScale", this.contentScale);
        //    //info.AddValue("contentOffsetX", this.contentOffsetX);
        //    //info.AddValue("contentOffsetY", this.contentOffsetY);
        //    //info.AddValue("contentWidth", this.contentWidth);
        //    //info.AddValue("contentHeight", this.contentHeight);
        //    //info.AddValue("contentViewportWidth", this.contentViewportWidth);
        //    //info.AddValue("contentViewportHeight", this.contentViewportHeight);
        //}

        ///
        /// The current scale at which the content is being viewed.
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentScale
        {
            get
            {
                return contentScale;
            }
            set
            {
                contentScale = value;

                OnPropertyChanged("ContentScale");
            }
        }

        //[XmlIgnore]
        [JsonIgnore]
        public CanvasPoint ContentOffset
        {
            get
            {
                return new CanvasPoint(ContentOffsetX, ContentOffsetY);
            }
            set
            {
                ContentOffsetX = value.X;
                ContentOffsetY = value.Y;
            }
        }

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentOffsetX
        {
            get
            {
                return contentOffsetX;
            }
            set
            {
                contentOffsetX = value;

                OnPropertyChanged("ContentOffsetX");
            }
        }

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentOffsetY
        {
            get
            {
                return contentOffsetY;
            }
            set
            {
                contentOffsetY = value;

                OnPropertyChanged("ContentOffsetY");
            }
        }

        //[XmlIgnore]
        [JsonIgnore]
        public CanvasSize ContentSize
        {
            get
            {
                return new CanvasSize(ContentWidth, ContentHeight);
            }
            set
            {
                ContentWidth = value.Width;
                ContentHeight = value.Height;
            }
        }

        ///
        /// The width of the content (in content coordinates).
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value + (ContentCanvasMarginOffset * 2);

                OnPropertyChanged("ContentWidth");
            }
        }

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentHeight
        {
            get
            {
                return contentHeight;
            }
            set
            {
                contentHeight = value + (ContentCanvasMarginOffset * 2);

                OnPropertyChanged("ContentHeight");
            }
        }


        //[XmlIgnore]
        [JsonIgnore]
        public CanvasSize ContentViewportSize
        {
            get
            {
                return new CanvasPoint(ContentViewportWidth, ContentViewportHeight);
            }
            set
            {
                ContentViewportWidth = value.Width;
                ContentViewportHeight = value.Height;
            }
        }

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentViewportWidth
        {
            get
            {
                return contentViewportWidth;
            }
            set
            {
                contentViewportWidth = value;

                OnPropertyChanged("ContentViewportWidth");
            }
        }

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        //[XmlIgnore]
        [JsonIgnore]
        public double ContentViewportHeight
        {
            get
            {
                return contentViewportHeight;
            }
            set
            {
                contentViewportHeight = value;

                OnPropertyChanged("ContentViewportHeight");
            }
        }

        #endregion

        
    }

}
