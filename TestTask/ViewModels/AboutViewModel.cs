using System.IO;
using System.Reflection;
using System.Windows.Input;
using TestTask.Common;
using TestTask.Services;

namespace TestTask.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        public ICommand OpenWinWordCommand => new RelayCommand(_ =>
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(assemblyLocation), "Files", "Test tasks.doc");
            ProcessHelper.StartProcess(path);
        });
    }
}
