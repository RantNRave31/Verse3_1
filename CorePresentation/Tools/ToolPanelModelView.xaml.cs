﻿using System;
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
using Verse3.CorePresentation.Workspaces;

namespace Verse3.Tools
{
    /// <summary>
    /// Interaction logic for ToolsetViewModel.xaml
    /// </summary>
    public partial class ToolPanelModelView : UserControl
    {
        public ToolPanelModelView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceViewModel.StaticWorkspaceViewModel.AddToCanvas_OnCall(sender, e);
        }
    }
}
