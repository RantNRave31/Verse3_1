using Core.ViewModels;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Core
{
    public class PropertyViewModel
        : ViewModelBase
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
}
