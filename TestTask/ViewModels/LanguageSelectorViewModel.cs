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
            MessageContainerService.FilesChanged += MessageContainerService_FilesChanged;
            UpdateLangFiles();
        }

        private void MessageContainerService_FilesChanged(object sender, EventArgs e)
        {
            var newContainers = MessageContainerService.MessageContainers;
            var newLocales = newContainers
                .Where(nc => LangContainers.All(lc => lc.FileInfo.FullName != nc.FileInfo.FullName)).ToArray();
            var updatedLocales = newContainers.Where(nc =>
                LangContainers.Any(lc => lc.FileInfo.FullName == nc.FileInfo.FullName && lc != nc)).ToArray();
            var removedLocales = LangContainers.Where(lc => newContainers.All(nc => lc.FileInfo != nc.FileInfo)
                && !updatedLocales.Any(ul => ul.LocaleName == lc.LocaleName)).ToArray();

            if (SelectedMessageContainer == null)
            {
                var localeName = CultureInfo.CurrentUICulture.Name.ToLower();
                SelectedMessageContainer = newContainers.FirstOrDefault(nc => nc.LocaleName.ToLower() == localeName)
                                           ?? newContainers.FirstOrDefault(nc => localeName.StartsWith(nc.LocaleName.ToLower()));
            }
            else
            {
                SelectedMessageContainer = newContainers.FirstOrDefault(nc => nc.LocaleName == SelectedMessageContainer.LocaleName)
                                           ?? newContainers.FirstOrDefault();
            }

            var status = new StringBuilder();
            if (SelectedMessageContainer != null)
            {
                AppendStatus(status, newLocales, LangKeys.LocalesRead);
                AppendStatus(status, updatedLocales, LangKeys.LocalesUpdated);
                AppendStatus(status, removedLocales, LangKeys.LocalesRemoved);
            }

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

        public string JsonPath
        {
            get
            {
                SetInitialVal(new DirectoryInfo(Settings.Default.DefaultJsonPath).FullName);
                return GetVal<string>();
            }
            set => SetVal(value, UpdateLangFiles);
        }

        public ObservableCollection<MessageContainer> LangContainers { get; } = new ObservableCollection<MessageContainer>();

        public string Status
        {
            get => GetVal<string>();
            set => SetVal(value);
        }

        public MessageContainer SelectedMessageContainer
        {
            get => GetVal<MessageContainer>();
            set => SetVal(value, SetNewLanguage);
        }

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

        public ICommand OpenFolderCommand => new RelayCommand(_ =>
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(new DirectoryInfo(JsonPath).FullName);
                process.Start();
            }
        });

        public ICommand CreateTestJsonsCommand => new RelayCommand(_ =>
        {
            TestContainersCreator.SaveAllContainers();
            JsonPath = new DirectoryInfo(Settings.Default.DefaultJsonPath).FullName;
        });

        private void SetNewLanguage()
        {
            if (SelectedMessageContainer?[LangKeys.SelectedLocale] != null)
            {
                Status = SelectedMessageContainer[LangKeys.SelectedLocale].Formatted(SelectedMessageContainer.LocaleName);
            }

            App.MessageContainer = SelectedMessageContainer;
            ThreadHelper.RunInMainThread(() => Translator.UpdateElems(SelectedMessageContainer));
        }

        private void UpdateLangFiles()
        {
            if (!JsonPath.IsNullOrWhiteSpace())
            {
                MessageContainerService.StartReadingDirectory(JsonPath);
            }
        }
    }
}
