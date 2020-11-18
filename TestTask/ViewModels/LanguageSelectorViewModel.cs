using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using TestTask.Common;
using TestTask.ExtensionMethods;
using TestTask.Models;
using TestTask.Properties;
using TestTask.Services;

namespace TestTask.ViewModels
{
    public class LanguageSelectorViewModel : ObservableObject
    {
        public LanguageSelectorViewModel()
        {
            // ждём события обновления json-файлов в каталоге JsonPath
            MessageContainerService.FilesChanged += MessageContainerService_FilesChanged;
            // запускаем таймер для перечитывания каталога с json-файлами
            UpdateLangFiles();
        }

        private void MessageContainerService_FilesChanged(object sender, EventArgs e)
        {
            // в newContainers приходят все найденные локали, в т.ч. ранее прочитанные. Разбираем их на новые, обновленные и удалённые
            var newContainers = MessageContainerService.MessageContainers;
            var newLocales = newContainers
                .Where(nc => LangContainers.All(lc => lc.FileInfo.FullName != nc.FileInfo.FullName)).ToArray();
            var updatedLocales = newContainers.Where(nc =>
                LangContainers.Any(lc => lc.FileInfo.FullName == nc.FileInfo.FullName && lc != nc)).ToArray();
            var removedLocales = LangContainers.Where(lc => newContainers.All(nc => lc.FileInfo != nc.FileInfo)
                && !updatedLocales.Any(ul => ul.LocaleName == lc.LocaleName)).ToArray();

            // при запуске программы или после появления первого json (если ранее их не было), выбираем текущую локаль Windows
            if (SelectedMessageContainer == null)
            {
                var localeName = CultureInfo.CurrentUICulture.Name.ToLower();
                SelectedMessageContainer = newContainers.FirstOrDefault(nc => nc.LocaleName.ToLower() == localeName)
                                           ?? newContainers.FirstOrDefault(nc => localeName.StartsWith(nc.LocaleName.ToLower()));
            }
            // или восстанавливаем ранее выбранную локаль
            else
            {
                SelectedMessageContainer = newContainers.FirstOrDefault(nc => nc.LocaleName == SelectedMessageContainer.LocaleName)
                                           ?? newContainers.FirstOrDefault();
            }

            var status = new StringBuilder();
            if (SelectedMessageContainer != null)
            {
                // собираем строку статуса, чтобы сообщить, какие локали нашли/потеряли
                AppendStatus(status, newLocales, LangKeys.LocalesRead);
                AppendStatus(status, updatedLocales, LangKeys.LocalesUpdated);
                AppendStatus(status, removedLocales, LangKeys.LocalesRemoved);
            }

            // обновляем интерфейс в главном потоке (событие было вызвано из фонового)
            ThreadHelper.RunInMainThread(() =>
            {
                var selectedContainer = SelectedMessageContainer;
                LangContainers.Clear();
                newContainers.ForEach(nf => LangContainers.Add(nf));
                SelectedMessageContainer = selectedContainer;
                Status = status.ToString().TrimEnd('\n').TrimEnd('\r');
            });

            void AppendStatus(StringBuilder statusStringBuilder, MessageContainer[] locales, LangKeys descriptorKey)
            {
                if (locales.Any())
                {
                    var statusLine = SelectedMessageContainer[descriptorKey];
                    if (statusLine != null)
                    {
                        statusStringBuilder.AppendLine(
                            statusLine.Formatted(locales.Select(nl => nl.LocaleName).JoinedString()));
                    }
                }
            }
        }

        /// <summary>
        /// Путь к каталогу с json-файлами локалей.
        /// Его изхменение приводит к перезапуску таймера обновления католога и считыванию новых файлов
        /// </summary>
        public string JsonPath
        {
            get
            {
                SetInitialVal(new DirectoryInfo(Settings.Default.DefaultJsonPath).FullName);
                return GetVal<string>();
            }
            set => SetVal(value, UpdateLangFiles);
        }

        /// <summary>
        /// Список найденных локалей (контейнеров)
        /// </summary>
        public ObservableCollection<MessageContainer> LangContainers { get; } = new ObservableCollection<MessageContainer>();

        /// <summary>
        /// Строка состояния
        /// </summary>
        public string Status
        {
            get => GetVal<string>();
            set => SetVal(value);
        }

        /// <summary>
        /// Выбранный контейнер. Его изменение приводит к вызову метода SetNewLanguage(), который обновляет интерфейс
        /// </summary>
        public MessageContainer SelectedMessageContainer
        {
            get => GetVal<MessageContainer>();
            set => SetVal(value, SetNewLanguage);
        }

        /// <summary>
        /// Команда "Сменить каталог". Вызывается с кнопки. После смены катлога вызывается метод UpdateLangFiles()
        /// </summary>
        public ICommand ChangeFolderCommand => new RelayCommand(_ =>
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = JsonPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    JsonPath = dialog.SelectedPath;
                }
            }
        });

        /// <summary>
        /// Команда "Открыть текущий каталог". Вызывается с кнопки. 
        /// </summary>
        public ICommand OpenFolderCommand => new RelayCommand(_ =>
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(new DirectoryInfo(JsonPath).FullName);
                process.Start();
            }
        });

        /// <summary>
        /// Команда "Создать в текущем каталоге дефолтные контейнеры с локалями". Вызывается с кнопки. 
        /// </summary>
        public ICommand CreateTestJsonsCommand => new RelayCommand(_ =>
        {
            TestContainersCreator.SaveAllContainers();
            JsonPath = new DirectoryInfo(Settings.Default.DefaultJsonPath).FullName;
        });

        /// <summary>
        /// Установка нового языка интерфейса. Вызывается после смены выбранного контейнера. Обновляет строку статуса и зовёт метод
        /// Translator.UpdateElems() из главного потока
        /// </summary>
        private void SetNewLanguage()
        {
            if (SelectedMessageContainer?[LangKeys.SelectedLocale] != null)
            {
                Status = SelectedMessageContainer[LangKeys.SelectedLocale].Formatted(SelectedMessageContainer.LocaleName);
            }

            App.MessageContainer = SelectedMessageContainer;
            ThreadHelper.RunInMainThread(() => Translator.UpdateElems(SelectedMessageContainer));
        }

        /// <summary>
        /// Перезапуск таймера и перечитывание json-файлов после смены JsonPath
        /// </summary>
        private void UpdateLangFiles()
        {
            if (!JsonPath.IsNullOrWhiteSpace())
            {
                MessageContainerService.StartReadingDirectory(JsonPath);
            }
        }
    }
}
