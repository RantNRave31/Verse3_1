using Core.Elements;
using Newtonsoft.Json;
using System;
using static Core.Geometry2D;

namespace Core
{
    public interface IRenderable : IElement
    {
        #region Render Pipeline Info

        [JsonIgnore]
        public RenderPipelineInfo RenderPipelineInfo { get; }
        //TODO: GUID Lookup in DataModel Instance
        //public IRenderable ZPrev { get; }
        //public IRenderable ZNext { get; }
        [JsonIgnore]
        public IRenderable Parent { get/* => RenderPipelineInfo.Parent*/; }
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> Children { get/* => RenderPipelineInfo.Children*/; }

        #endregion

        #region Properties

#nullable enable
        [JsonIgnore]
        public Type? ViewType { get; }
#nullable restore
        [JsonIgnore]
        object ViewKey { get; set; }
        [JsonIgnore]
        IRenderView RenderView { get; set; }
        [JsonIgnore]
        public bool Visible { get; set; }

        #endregion

        #region BoundingBox

        /// <summary>
        /// Bounding Box of the Element
        /// </summary>
        public abstract BoundingBox BoundingBox { get; }

        /// <summary>
        /// The X coordinate of the location of the element Bounding Box (in content coordinates).
        /// </summary>
        [JsonIgnore]
        double X { get; }
        /// <summary>
        /// Set the X coordinate of the location of the element Bounding Box (in content coordinates).
        /// </summary>
        void SetX(double x);
        /*{
            BoundingBox.Location.X = x;
            OnPropertyChanged("X");
        }*/

        /// <summary>
        /// The Y coordinate of the location of the element Bounding Box (in content coordinates).
        /// </summary>
        [JsonIgnore]
        double Y { get; }
        /// <summary>
        /// Set the Y coordinate of the location of the element Bounding Box (in content coordinates).
        /// </summary>
        void SetY(double x);
        /*{
            BoundingBox.Location.Y = x;
            OnPropertyChanged("Y");
        }*/

        /// <summary>
        /// The width of the element Bounding Box (in content coordinates).
        /// </summary>
        [JsonIgnore]
        double Width { get; set; }
        /// <summary>
        /// Set the width of the element Bounding Box (in content coordinates).
        /// </summary>
        void SetWidth(double x); /*{ BoundingBox.Size.Width = x; OnPropertyChanged("Width"); }*/

        /// <summary>
        /// The height of the element Bounding Box (in content coordinates).
        /// </summary>
        [JsonIgnore]
        double Height { get; set; }
        [JsonIgnore]
        bool IsSelected { get; set; }
        [JsonIgnore]
        bool RenderExpired { get; set; }

        /// <summary>
        /// Set the height of the element Bounding Box (in content coordinates).
        /// </summary>
        void SetHeight(double x); /*{ BoundingBox.Size.Height = x; OnPropertyChanged("Height"); }*/

        #endregion

        void Render();
        /*{
            if (RenderView != null)
                RenderView.Render();
        }*/
    }
}
