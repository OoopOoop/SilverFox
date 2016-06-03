using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace Main.Shared
{
    public abstract  class MessageBoxService
    {
        public void ShowErrorMessage(string text, string caption)
        {
            System.Windows.MessageBox.Show(text, caption, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

    }
}
