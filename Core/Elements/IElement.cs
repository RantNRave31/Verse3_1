using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Elements
{
    public interface IElement : INotifyPropertyChanged, IDisposable, ISerializable
    {
        #region Properties

        /// <summary>
        /// GUID of the element.
        /// </summary>
        //[XmlIgnore]
        public Guid ID { get; }

        [JsonIgnore]
        public ElementState ElementState { get; set; }

        public ElementType ElementType { get; set; }

        public PropertiesViewModel Properties { get; set; }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// Be sure to Define a 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// eg. <code>public new abstract event PropertyChangedEventHandler PropertyChanged;</code>
        /// </summary>
        public abstract void OnPropertyChanged(string name = "");

        #endregion
    }

}
