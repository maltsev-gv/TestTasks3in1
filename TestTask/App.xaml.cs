using System;
using System.Net;
using System.Windows;
using TestTask.Models;

namespace TestTask
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static MessageContainer _messageContainer;
        public static event EventHandler MessageContainerChanged;

        /// <summary>
        /// Основной компонент, необходимый для переводов интерфейса и внутренних строк. Устанавливается методом
        /// LanguageSelectorViewModel.SetNewLanguage. Выбирается в зависимости от текущей локали Windows либо от желания пользователя
        /// </summary>
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

        /// <summary>
        /// Обработчик неперехваченных ранее исключений
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), MessageContainer?[LangKeys.UnhandledException] ?? "Any exception thrown");
            e.Handled = true;
        }

    }
}
