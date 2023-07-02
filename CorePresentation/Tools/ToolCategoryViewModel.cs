using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verse3.Tools
{
    public class ToolCategoryViewModel
        : ViewModelBase
    {
        public ObservableCollection<ToolSubcategoryViewModel> ToolSubcategories { get; set; }
        public ToolCategoryViewModel(string name)
            : base(name)
        {
            ToolSubcategories = new ObservableCollection<ToolSubcategoryViewModel>();
        }
    }
}
