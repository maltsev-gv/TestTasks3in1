using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using TestTask.Common;
using TestTask.Models;
using TestTask.Services;
using Clipboard = System.Windows.Clipboard;

namespace TestTask.ViewModels
{
    public class SqlScriptsViewModel : ObservableObject
    {
        public ICommand ChangeDisplayingScriptCommand => new RelayCommand(dispScriptObj =>
        {
            DisplayingScript = (DisplayingScripts) dispScriptObj;
            switch (DisplayingScript)
            {
                case DisplayingScripts.EntireScript:
                    var sb = new StringBuilder(ResourceHelper.GetSqlScript(DisplayingScripts.CreateDatabase));
                    sb.AppendLine(ResourceHelper.GetSqlScript(DisplayingScripts.CreateProcedure));
                    sb.AppendLine(ResourceHelper.GetSqlScript(DisplayingScripts.InsertData));
                    Script = sb.ToString();
                    break;
                case DisplayingScripts.CreateDatabase:
                case DisplayingScripts.CreateProcedure:
                case DisplayingScripts.InsertData:
                    Script = ResourceHelper.GetSqlScript(DisplayingScript);
                    break;
            }
        });

        public DisplayingScripts DisplayingScript
        {
            get => GetVal<DisplayingScripts>();
            set => SetVal(value);
        }

        public ICommand CopyToClipboardCommand => new RelayCommand(_ => Clipboard.SetDataObject(Script));
        
        public ICommand SaveZipCommand => new RelayCommand(_ => 
        { 
            var sfd = new SaveFileDialog() { FileName = "Turnstile.zip" };
            if (sfd.ShowDialog() == DialogResult.OK)
            { 
                File.WriteAllBytes(sfd.FileName, ResourceHelper.GetBinaryFile("Files.Turnstile.zip"));
                ProcessHelper.StartProcess(sfd.FileName);
            }
        });

        public string Script
        {
            get => GetVal<string>();
            set => SetVal(value);
        }
    }
}
