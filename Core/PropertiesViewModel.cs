using Core.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using static Core.Geometry2D;

namespace Core
{
    public class PropertiesViewModel
        : ViewModelBase
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
