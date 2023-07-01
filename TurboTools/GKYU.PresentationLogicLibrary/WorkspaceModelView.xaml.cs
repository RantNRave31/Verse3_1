using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GKYU.PresentationLogicLibrary;

using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.ViewModels;
using static GKYU.PresentationLogicLibrary.WorkspaceViewModel;
using System.Diagnostics;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.About;

namespace GKYU.PresentationLogicLibrary.ModelViews
{
    /// <summary>
    /// Interaction logic for WorkspaceModelView.xaml
    /// </summary>
    public partial class WorkspaceModelView : UserControl
    {
        public WorkspaceModelView()
        {
            InitializeComponent();
        }
        private void AddNewFileViewModel(EquipmentViewModel fileViewModel)
        {
            ((WorkspaceViewModel)this.DataContext).FileViewModels.Add(fileViewModel);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            ((WorkspaceViewModel)this.DataContext).CurrentDateTime = DateTime.Now;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutModelView dialog = new AboutModelView();
            //dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((EquipmentViewModel)((WorkspaceViewModel)this.DataContext).SelectedFileViewModel).PullPinPadParameters();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                AddNewFileViewModel(new EquipmentViewModel("Debug", new FileModel()) { Name="Debug"});
            }
            ((WorkspaceViewModel)this.DataContext).Refresh(null);
            CommandViewModel commandVM = ((WorkspaceViewModel)this.DataContext).Commands.First(cvm => cvm.Name == "Open File");
            commandVM.Command.Execute("C:\\Work\\Text.txt");
            commandVM.Command.Execute("C:\\Work\\Test.bmp");
        }
    }
}
