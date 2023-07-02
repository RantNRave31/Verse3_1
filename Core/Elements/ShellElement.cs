using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Core.Elements
{
    internal class ShellElement : IElement
    {
        public Guid ID { get; }

        public ElementState ElementState { get; set; }
        public ElementType ElementType { get; set; }
        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<ShellElement>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }
        public ShellElement()
        {
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        ~ShellElement() => Dispose();
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("ID", ID);
                //info.AddValue("X", this.X);
                //info.AddValue("Y", this.Y);
                //info.AddValue("Width", this.Width);
                //info.AddValue("Height", this.Height);
                info.AddValue("ElementType", ElementType);
                //info.AddValue("State", this.State);
                //info.AddValue("IsSelected", this.IsSelected);
                //info.AddValue("BoundingBox", this.BoundingBox);
                //info.AddValue("ElementState", this.ElementState);
                //info.AddValue("Parent", this.Parent);
                //info.AddValue("Children", this.Children);
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }

}
