using Main.Models;
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
using System.Windows.Shapes;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Page
    {
        public AddWindow()
        {
            InitializeComponent();
        }



        //Activate rows MultiSelect in dataGrid
        private void PreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
        
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataGridRow row = GetVisualParentByType((FrameworkElement)e.OriginalSource, typeof(DataGridRow)) as DataGridRow;

                row.IsSelected = !row.IsSelected;
                e.Handled = true;
            }
        }

        private void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is DataGridRow && e.LeftButton == MouseButtonState.Pressed)
            {
                DataGridRow row = e.OriginalSource as DataGridRow;

                row.IsSelected = !row.IsSelected;
                e.Handled = true;
                
            }
        }


        public static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
        {
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        //private void ServicesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var grid = sender as DataGrid;
        //    var selected = grid.SelectedItems;

        //    List<string> names = new List<string>();
        //    foreach (var item in selected)
        //    {
        //        var service = item as ServiceItem;
        //        names.Add(service.DisplayName);
        //    }
        //}
    }
}
