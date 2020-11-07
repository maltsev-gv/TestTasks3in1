using System;
using System.Net;
using System.Windows;
using TestTask.Common;
using TestTask.Models;
using TestTask.Services;

namespace TestTask
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static MessageContainer _messageContainer;
        public static event EventHandler MessageContainerChanged;

        public static MessageContainer MessageContainer
        {
            get => _messageContainer;
            internal set
            {
                _messageContainer = value;
                MessageContainerChanged?.Invoke(MessageContainer, EventArgs.Empty);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            ServicePointManager.DefaultConnectionLimit = 10000;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), MessageContainer?[LangKeys.UnhandledException] ?? "Any exception thrown");
            e.Handled = true;
        }

    }
}
