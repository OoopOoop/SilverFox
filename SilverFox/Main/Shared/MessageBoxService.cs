using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;

namespace Main.Shared
{
    public abstract  class MessageBoxService
    {
        public void ShowErrorMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInfoMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ShowConfirmMessage(string text, string caption) => MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;

    }
}
