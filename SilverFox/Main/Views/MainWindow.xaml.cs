using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void ClearMultiselBtn_Click(object sender, RoutedEventArgs e)
        {
            ServicesGrid.UnselectAll();
        }
    }
}
