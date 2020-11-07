using System.Collections.Generic;
using System.IO;
using TestTask.Common;
using TestTask.Models;
using TestTask.Properties;

namespace TestTask.Services
{
    public class TestContainersCreator
    {
        public MessageContainer GetRusContainer()
        {
            var container = new MessageContainer()
            {
                LanguageName = "Русский",
                LocaleName = "RU",
                Phrases = new Dictionary<LangKeys, string>()
                {
                    [LangKeys.LanguageTitle] = "Язык",
                    [LangKeys.JsonFolder] = "Папка с JSON-контейнерами",
                    [LangKeys.AvailableLanguages] = "Доступные языки",
                    [LangKeys.LocalesRead] = "Прочитаны локали: {0}",
                    [LangKeys.LocalesUpdated] = "Обновлены локали: {0}",
                    [LangKeys.LocalesRemoved] = "Удалены локали: {0}",
                    [LangKeys.SelectedLocale] = "Выбрана локаль: {0}",
                    [LangKeys.OpenFolder] = "Открыть папку",
                    [LangKeys.ChangeFolder] = "Выбрать папку",
                    [LangKeys.LaunchListener] = "Запустить Listener",
                    [LangKeys.StopListener] = "Остановить",
                    [LangKeys.AboutTitle] = "О программе",
                    [LangKeys.Copyleft] = "Copyleft \x00a9 от Геннадия Мальцева",
                    [LangKeys.CreateTestTranslations] = "Создать тестовые Json-переводы",
                    [LangKeys.ProgramDescription] =
                        @"Данная вкладка демонстрирует работу автоматического переводчика интерфейса. 
Вы можете динамически (не останавливая программы) менять файлы в папке с переводами: редактировать, удалять, добавлять файлы. 
Вы можете также сменить папку для поиска переводов. 
Путь по умолчанию, маска JSON-файлов и периодичность обновления списка локалей задаются в файле с расширением .config в папке с исполняемым файлом приложения.",

                    [LangKeys.UnhandledException] = "Непредвиденная ошибка",
                    [LangKeys.WaitingForRequest] = "Ожидание запроса",
                    [LangKeys.InvalidRequest] = "Неверный запрос: {0}",
                    [LangKeys.ResponseSent] = "Отправлен ответ",
                    [LangKeys.SenderAndListenerTitle] = "Отправка/получение запросов",
                    [LangKeys.RequestError] = "Ошибка запроса: {0}",
                    [LangKeys.Success] = "Успешно",
                    [LangKeys.BadResponseFormat] = "Неверный формат ответа: {0}",
                    [LangKeys.WaitingForResponse] = "Ждём ответа",
                    [LangKeys.Client] = "Клиент",
                    [LangKeys.Server] = "Сервер",
                    [LangKeys.Number] = "Номер",
                    [LangKeys.ThreadId] = "Id потока",
                    [LangKeys.Status] = "Статус",
                    [LangKeys.Error] = "Ошибка: {0}",
                    [LangKeys.Time] = "Время",
                    [LangKeys.TimeMs] = "Время, мс",
                    [LangKeys.Send] = "Послать",
                    [LangKeys.Requests3Cases] = "запрос|запроса|запросов",
                    [LangKeys.ReqNumber] = "№ запроса",
                    [LangKeys.EnableListening] = "Разрешить прослушивание",
                    [LangKeys.ClearResponseList] = "Очистить список ответов",
                    [LangKeys.ClearRequestList] = "Очистить список запросов",
                    [LangKeys.RequestsSendingTo] = "Запросы отправляются на",
                    [LangKeys.CopyUrl] = "Скопировать в буфер обмена",
                    [LangKeys.EntireScript] = "Весь скрипт",
                    [LangKeys.DatabaseCreating] = "Создание БД и таблиц",
                    [LangKeys.ProcedureCreating] = "Создание процедуры оценки времени",
                    [LangKeys.DataFilling] = "Заполнение тестовыми данными",
                    [LangKeys.SqlScripts] = "Скрипты SQL",
                    [LangKeys.CopyScript] = "Скопировать выбранный скрипт в буфер обмена",
                    [LangKeys.SaveZippedDb] = "Сохранить копию БД в Zip-архиве",
                    [LangKeys.OpenTaskDocument] = "Открыть документ с заданием",
                }
            };
            return container;
        }
        public MessageContainer GetEngContainer()
        {
            var container = new MessageContainer()
            {
                LanguageName = "English",
                LocaleName = "EN",
                Phrases = new Dictionary<LangKeys, string>()
                {
                    [LangKeys.LanguageTitle] = "Language",
                    [LangKeys.JsonFolder] = "Folder with JSON-containers",
                    [LangKeys.AvailableLanguages] = "Available languages",
                    [LangKeys.LocalesRead] = "Locales read: {0}",
                    [LangKeys.LocalesUpdated] = "Locales updated: {0}",
                    [LangKeys.LocalesRemoved] = "Locales removed: {0}",
                    [LangKeys.SelectedLocale] = "Selected locale: {0}",
                    [LangKeys.OpenFolder] = "Open folder",
                    [LangKeys.ChangeFolder] = "Change folder",
                    [LangKeys.LaunchListener] = "Launch Listener",
                    [LangKeys.StopListener] = "Stop",
                    [LangKeys.AboutTitle] = "About",
                    [LangKeys.Copyleft] = "Copyleft \x00a9 by Gennady V. Maltsev",
                    [LangKeys.CreateTestTranslations] = "Create test Json-translations",
                    [LangKeys.ProgramDescription] =
                    @"This tab shows how the automatic interface translator works.
You can dynamically (without stopping the program) change files in the folder with translations: edit, delete, add files. 
You can also change the folder for searching for translations.
The default path, JSON file mask, and how often the list of locales is updated are set in the .config file in the application executable folder."
                    ,
                    [LangKeys.UnhandledException] = "Unhandled exception",
                    [LangKeys.WaitingForRequest] = "Waiting for request",
                    [LangKeys.InvalidRequest] = "Invalid request: {0}",
                    [LangKeys.ResponseSent] = "Response sent",
                    [LangKeys.SenderAndListenerTitle] = "Sender & listener",
                    [LangKeys.RequestError] = "Request error: {0}",
                    [LangKeys.Success] = "Success",
                    [LangKeys.BadResponseFormat] = "Bad response format: {0}",
                    [LangKeys.WaitingForResponse] = "Waiting for response",
                    [LangKeys.Client] = "Client",
                    [LangKeys.Server] = "Server",
                    [LangKeys.Number] = "Number",
                    [LangKeys.ThreadId] = "Thread id",
                    [LangKeys.Status] = "Status",
                    [LangKeys.Error] = "Error: {0}",
                    [LangKeys.Time] = "Time",
                    [LangKeys.TimeMs] = "Time, ms",
                    [LangKeys.Send] = "Send",
                    [LangKeys.Requests3Cases] = "request|requests|requests",
                    [LangKeys.ReqNumber] = "Request #",
                    [LangKeys.EnableListening] = "Enable listening",
                    [LangKeys.ClearResponseList] = "Clear response list",
                    [LangKeys.ClearRequestList] = "Clear request list",
                    [LangKeys.RequestsSendingTo] = "Requests are sending to",
                    [LangKeys.CopyUrl] = "Copy URL to Clipboard",
                    [LangKeys.EntireScript] = "Entire script",
                    [LangKeys.DatabaseCreating] = "Creating database and tables",
                    [LangKeys.ProcedureCreating] = "Creating worktime calculating procedure",
                    [LangKeys.DataFilling] = "Filling by test data",
                    [LangKeys.SqlScripts] = "SQL Scripts",
                    [LangKeys.CopyScript] = "Copy selected script to Clipboard",
                    [LangKeys.SaveZippedDb] = "Save zipped DB copy",
                    [LangKeys.OpenTaskDocument] = "Open document with current task",
                }
            };
            return container;
        }

        public void SaveContainer(MessageContainer container, string fileName)
        {
            File.WriteAllText(fileName, JsonHelper.GetSerializedString(container));
        }

        public static void SaveAllContainers()
        { 
            var  creator = new TestContainersCreator();
            var path = Settings.Default.DefaultJsonPath;
            Directory.CreateDirectory(path);
            var containers = new[] { creator.GetRusContainer(), creator.GetEngContainer() };
            foreach (var messageContainer in containers)
            {
                var fileName = Path.Combine(path, Settings.Default.MessagesFileNamePattern.Replace("*", messageContainer.LocaleName));
                creator.SaveContainer(messageContainer, fileName);
            }
        }
    }
}
