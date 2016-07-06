using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Shared
{
    public class NotifyService: MessageBoxService,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
