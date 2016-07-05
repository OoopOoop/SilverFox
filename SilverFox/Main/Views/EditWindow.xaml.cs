using System.Windows;
using System.Windows.Controls;

namespace Main.Views
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Page
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        string originalText;

        private void NewNameTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
           if(string.IsNullOrEmpty(NewNameTxtBox.Text))
            {
                NewNameTxtBox.Text = originalText;
            }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            originalText = OriginalNameTxt.Text;
            txtBox.Clear();
        }
    }
}
