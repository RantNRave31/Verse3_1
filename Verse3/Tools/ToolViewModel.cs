using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse3.Components;

namespace Verse3.Tools
{
    public class ToolViewModel
        : ViewModelBase
    {
        private string _categoryName;
        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                if (value != _categoryName)
                {
                    _categoryName = value;
                    OnPropertyChanged();
                }
            }
        }
        private CompInfo _compInfo;
        public CompInfo CompInfo
        {
            get
            {
                return _compInfo;
            }
            set
            {
                _compInfo = value;
                OnPropertyChanged();
            }
        }

        public ToolViewModel(string group, string name)
            : base(name)
        {
            CategoryName = group;
        }
    }
}
