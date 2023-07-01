using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Verse3.Tools
{
    public class ToolPanelViewModel
        : ViewModelBase
    {
        public ObservableCollection<ToolCategoryViewModel> ToolCategories { get; set; }
        public ToolPanelViewModel(string name)
            : base(name)
        {
            ToolCategories = new ObservableCollection<ToolCategoryViewModel>();
        }
    }
}
