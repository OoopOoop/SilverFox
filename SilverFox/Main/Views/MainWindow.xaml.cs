using Main.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Main.Views
{
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();

            App.Current.MainWindow.Closing += new CancelEventHandler(callVMSaveCommand);
        }


        //call the command to save services when user closes programm
        private void callVMSaveCommand(object sender, CancelEventArgs e)
        {
            var viewModel = (MainViewModel)DataContext;
            if (viewModel.WindowClosingCommand.CanExecute(null))
                viewModel.WindowClosingCommand.Execute(null);
        }


        private void ClearMultiselBtn_Click(object sender, RoutedEventArgs e)
        {
            ServicesGrid.UnselectAll();
        }

        //private void PreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DataGridRow row = GetVisualParentByType((FrameworkElement)e.OriginalSource, typeof(DataGridRow)) as DataGridRow;

        //        row.IsSelected = !row.IsSelected;
        //        e.Handled = true;

        //    }
        //}

        //private void MouseEnterHandler(object sender, MouseEventArgs e)
        //{
        //    if (e.OriginalSource is DataGridRow && e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DataGridRow row = e.OriginalSource as DataGridRow;

        //        row.IsSelected = !row.IsSelected;
        //        e.Handled = true;


        //    }
        //}

        //public static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
        //{
        //    DependencyObject parent = startObject;
        //    while (parent != null)
        //    {
        //        if (type.IsInstanceOfType(parent))
        //            break;
        //        else
        //            parent = VisualTreeHelper.GetParent(parent);
        //    }

        //    return parent;
        //}






    }
}