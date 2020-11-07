using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Common;
using TestTask.Properties;
using TestTask.ExtensionMethods;
using TestTask.Models;

namespace TestTask.Services
{
    public class MessageContainerService
    {
        public static HashSet<MessageContainer> MessageContainers { get; } = new HashSet<MessageContainer>();
        public static event EventHandler FilesChanged;

        public static void FindContainers(string path)
        {
            var jsonFiles = Directory.EnumerateFiles(path, Settings.Default.MessagesFileNamePattern)
                .Select(fileName => new FileInfo(fileName)).ToArray();
            var deletedContainers = MessageContainers.Where(mc => !jsonFiles.Any(jf => jf.FullName == mc.FileInfo.FullName)).ToArray();
            deletedContainers.ForEach(dc => MessageContainers.Remove(dc));

            bool somethingChanged = deletedContainers.Any();

            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    if (MessageContainers.Any(mf => mf.FileInfo.FullName == jsonFile.FullName && mf.FileInfo.LastWriteTime == jsonFile.LastWriteTime))
                    {
                        continue;
                    }

                    somethingChanged = true;
                    MessageContainers.RemoveAll(mf => mf.FileInfo.FullName == jsonFile.FullName);
                    var json = File.ReadAllText(jsonFile.FullName);

                    var container = JsonHelper.GetObjectFromString<MessageContainer>(json);
                    container.FileInfo = jsonFile;
                    MessageContainers.Add(container);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{nameof(FindContainers)}: can't open json file.{Environment.NewLine}{ex}");
                }
            }
            if (somethingChanged)
            {
                FilesChanged?.Invoke(MessageContainers, EventArgs.Empty);
            }
        }

        private static Timer _updatingTimer;
        public static void StartReadingDirectory(string path)
        {
            _updatingTimer?.Dispose();
            _updatingTimer = new Timer(_ => FindContainers(path), null, 0, Settings.Default.ReReadDirectoryPeriodSec * 1000);
        }
    }
}
