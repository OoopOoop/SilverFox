using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Main.Shared
{
    public abstract  class MessageBoxService
    {
        public void ShowMessage(string text, string caption)
        {
            System.Windows.MessageBox.Show(text, caption, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
