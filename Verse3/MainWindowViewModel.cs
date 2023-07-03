using Core;
using Verse3.Collections.Generic;
using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Verse3.Components;
using Verse3.Elements;
using Verse3.Tools;
using Core.Assemblies;
using Verse3.Assemblies;
using Core.Elements;
using Verse3.CorePresentation.Workspaces;

namespace Verse3
{
    public class MainWindowViewModel
        : ViewModels.ViewModelBase
    {
        #region MainWindow
        public MainWindowModelView SelectedMainWindowModelView { get; set; }
        public ObservableCollection<MainWindowViewModel> MainWindowViewModels { get; set; }
        MainWindowViewModel _selectedMainWindowViewModel;
        public MainWindowViewModel SelectedMainWindowViewModel { get { return  _selectedMainWindowViewModel; } set { if (value == _selectedMainWindowViewModel) return; _selectedMainWindowViewModel = value; OnPropertyChanged(); } }
        public MainWindowModelView MainWindowModelView { get; set; }
        #endregion
        public WorkspaceViewModel WorkspaceViewModel { set; get;  }
        #region Instrumentation
        public string framesPerSecond;
        public string FramesPerSecond { get { return framesPerSecond; } set { if (value == framesPerSecond) return; framesPerSecond = value; OnPropertyChanged(); } }
        public string status;
        public string Status { get { return status; } set { if (value == status) return; status = value; OnPropertyChanged(); } }
        #endregion
        #region Tools
        #endregion
        public MainWindowViewModel(string displayName)
            : base(displayName)
        {
            MainWindowViewModels = new ObservableCollection<MainWindowViewModel>();
            WorkspaceViewModel  = new WorkspaceViewModel();
        }
    }
}
