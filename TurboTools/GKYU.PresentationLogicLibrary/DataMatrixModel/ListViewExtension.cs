using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

using GKYU.BusinessLogicLibrary.DataMatrixModel;
using System.Windows.Data;

namespace GKYU.PresentationLogicLibrary.DataMatrixModel
{
    public class ListViewExtension
    {
        public static readonly DependencyProperty MatrixSourceProperty =
            DependencyProperty.RegisterAttached("MatrixSource",
            typeof(DataMatrix), typeof(ListViewExtension),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnMatrixSourceChanged)));

        public static DataMatrix GetMatrixSource(DependencyObject d)
        {
            return (DataMatrix)d.GetValue(MatrixSourceProperty);
        }

        public static void SetMatrixSource(DependencyObject d, DataMatrix value)
        {
            d.SetValue(MatrixSourceProperty, value);
        }

        private static void OnMatrixSourceChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ListView listView = d as ListView;
            DataMatrix dataMatrix = e.NewValue as DataMatrix;

            listView.ItemsSource = dataMatrix;
            if (null != dataMatrix)
            {
                GridView gridView = listView.View as GridView;
                int count = 0;
                gridView.Columns.Clear();
                foreach (var col in dataMatrix.Columns)
                {
                    gridView.Columns.Add(
                        new GridViewColumn
                        {
                            Header = col.Name,
                            DisplayMemberBinding = new Binding(string.Format("[{0}]", count))
                        });
                    count++;
                }
            }
        }
    }
}
