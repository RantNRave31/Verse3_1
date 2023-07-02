using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verse3.Tools
{
    public class ToolSubcategoryViewModel
        : ViewModelBase
    {
        public ObservableCollection<ToolViewModel> Tools { get; set; }
        public ToolViewModel selectedTool;
        public ToolViewModel SelectedTool { get { return selectedTool; } set { if (value == selectedTool) return; selectedTool = value; OnPropertyChanged(); } }
        public ToolSubcategoryViewModel(string name)
            : base(name)
        {
            Tools = new ObservableCollection<ToolViewModel>();
        }
    }
}
