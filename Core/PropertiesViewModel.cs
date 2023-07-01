using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using static Core.Geometry2D;

namespace Core
{
    public class MetaData<T>
    {
        static PropertyInfo[] _properties = null;
        public static PropertyInfo[] Properties
        {
            get
            {
                if (null == _properties)
                    _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                return _properties;
            }
        }

    }
    public class BaseViewModel : INotifyPropertyChanged
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

        #region Constructors


        protected BaseViewModel() : base()
        {
        }

        #endregion

        #region Properties



        #endregion


    }
    public class PropertyViewModel
        : BaseViewModel
    {
        public virtual string Category { get; set; }
        protected INotifyPropertyChanged viewModel = null;
        protected PropertyInfo propertyInfo;
        public string Name { get { return propertyInfo.Name; } }
        public Type ValueType { get { return propertyInfo.PropertyType; } }
        public object Value { get { return propertyInfo.GetValue(viewModel); } set { propertyInfo.SetValue(viewModel, value); } }
       public PropertyViewModel(INotifyPropertyChanged viewModel, PropertyInfo propertyInfo)
            : base()
        {
            this.viewModel = viewModel;
            this.propertyInfo = propertyInfo;
            viewModel.PropertyChanged += BaseClass_PropertyChanged;

        }
        private void BaseClass_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
    public class PropertiesViewModel
        : BaseViewModel
    {
        protected INotifyPropertyChanged viewModel = null;
        public ObservableCollection<PropertyViewModel> Properties { get; set; }
        public PropertiesViewModel(INotifyPropertyChanged viewModel)
            : base()
        {
            this.viewModel = viewModel;
            Properties = new ObservableCollection<PropertyViewModel>();
        }
    }
    public class PropertiesViewModel<T>
        : PropertiesViewModel
        where T : INotifyPropertyChanged
    {
        public PropertiesViewModel(T viewModel)
            : base((INotifyPropertyChanged)viewModel)
        {
            foreach (PropertyInfo propertyInfo in MetaData<T>.Properties)
            {
                Properties.Add(new PropertyViewModel(viewModel, propertyInfo));
            }
        }
    }
}
