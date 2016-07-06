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

        /// <summary>
        /// Remove 'DeferRefresh' error when editing services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServicesGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);
        }
    }
}