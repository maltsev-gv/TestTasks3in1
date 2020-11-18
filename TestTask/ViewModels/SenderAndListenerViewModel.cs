﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestTask.Common;
using TestTask.ExtensionMethods;
using TestTask.Models;
using TestTask.Properties;
using TestTask.Services;

namespace TestTask.ViewModels
{
    public class SenderAndListenerViewModel : ObservableObject
    {
        public SenderAndListenerViewModel()
        {
            // число запросов по умолчанию
            RequestsCount = Settings.Default.DefaultRequestsCount;
            // если юзер поменяет язык - надо обновить свойство RequestInCase
            App.MessageContainerChanged += App_MessageContainerChanged;
            // включаем прослушивание
            EnableListening = true;
        }

        // счётчик запросов
        private int _currentRequestNumber = 1;
        private readonly HttpHelper _httpHelper = new HttpHelper();

        private void App_MessageContainerChanged(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(nameof(RequestInCase));
        }

        /// <summary>
        /// Коллекция запросов. Не должна быть пустой (инициализируется при первом чтении). При пересоздании вызывает RaisePropertyChanged()
        /// </summary>
        public ObservableCollection<RequestStatus> Requests
        {
            get
            {
                SetInitialVal(new ObservableCollection<RequestStatus>());
                return GetVal<ObservableCollection<RequestStatus>>();
            }
            set => SetVal(value);
        }

        /// <summary>
        /// Коллекция статусов прослушивателя. Не должна быть пустой (инициализируется при первом чтении).
        /// При пересоздании вызывает RaisePropertyChanged()
        /// </summary>
        public ObservableCollection<ListenerStatus> ListenerStatuses
        {
            get
            {
                SetInitialVal(new ObservableCollection<ListenerStatus>());
                return GetVal<ObservableCollection<ListenerStatus>>();
            }
            set => SetVal(value);
        }


        /// <summary>
        /// Число запросов по одному нажатию кнопки. Настраивается во вьюшке с помощью контрола NumbersTextBox
        /// При изменении обновляет зависимую от RequestsCount строку RequestInCase
        /// </summary>
        public int RequestsCount
        {
            get => GetVal<int>();
            set => SetVal(value, () => RaisePropertyChanged(nameof(RequestInCase)));
        }

        /// <summary>
        /// Слово "запрос"/"запросы"/"запросов". Зависит от языка и значения RequestsCount.
        /// При отсутствии перевода в контейнере возвращает "requests"
        /// </summary>
        public string RequestInCase
        {
            get
            {
                var cases = App.MessageContainer?[LangKeys.Requests3Cases]?.Split('|');
                if (cases == null || cases.Length != 3)
                {
                    return "requests";
                }

                return RequestsCount.GetDeclension(cases[0], cases[1], cases[2], false);
            }
        }

        /// <summary>
        /// Команда создания RequestsCount запросов к хосту.
        /// Создаваемые объекты RequestStatus получают уникальный номер и сразу создают HTTP-запрос
        /// </summary>
        public ICommand SendRequestsCommand => new RelayCommand(_ =>
        {
            for (int i = 0; i < RequestsCount; i++)
            {
                Requests.Add(new RequestStatus(_currentRequestNumber++));
            }
        });

        /// <summary>
        /// Команда очистки списка ответов.
        /// </summary>
        public ICommand ClearStatusesCommand => new RelayCommand(_ =>
        {
            ListenerStatuses.Clear();
        });

        /// <summary>
        /// Команда очистки списка запросов. Перед удалением каждого запроса делается попытка его отменить
        /// </summary>
        public ICommand ClearRequestsCommand => new RelayCommand(_ =>
        {
            Requests.ForEach(ss => ss.AbortRequest());
            Requests.Clear();
        });

        /// <summary>
        /// Свойство, связанное с чекбоксом "Разрешить прослушивание" на вьюшке. При изменении запускает/останавливает лисенер
        /// </summary>
        public bool? EnableListening
        {
            get => GetVal<bool?>();
            set => SetVal(value, ChangeListenerState);
        }

        public string LocalAddress { get; } = $"{Settings.Default.LocalAddress}?ThreadId=000&RequestNumber=000";

        public ICommand CopyUrlCommand => new RelayCommand(_ =>
        {
            Clipboard.SetDataObject(LocalAddress);
        });

        /// <summary>
        /// Сортировщик любой коллекции. Требует связки LangKey -> PropertyName
        /// </summary>
        private ObservableCollection<T> SortCollection<T>(IEnumerable<T> sourceEnumerable, ref (LangKeys key, bool asc) lastKeyTuple, LangKeys langKey,
            Dictionary<LangKeys, string> keyToPropNamesDictionary) where T : ObservableObject
        {
            bool isAscending = lastKeyTuple.key != langKey || lastKeyTuple.asc == false;
            if (!keyToPropNamesDictionary.ContainsKey(langKey))
            {
                throw new ArgumentException($"{nameof(SortCollection)}: LangKey {langKey} is not present in keyToPropNamesDictionary");
            }
            var propInfo = typeof(T).GetProperty(keyToPropNamesDictionary[langKey]);
            if (propInfo == null)
            {
                throw new ArgumentException($"{nameof(SortCollection)}: Type {typeof(T).Name} doesn't contains property '{keyToPropNamesDictionary[langKey]}'");
            }

            lastKeyTuple.key = langKey;
            lastKeyTuple.asc = isAscending;
            return isAscending
                ? new ObservableCollection<T>(sourceEnumerable.OrderBy(item => propInfo.GetValue(item)))
                : new ObservableCollection<T>(sourceEnumerable.OrderByDescending(item => propInfo.GetValue(item)));
        }

        private (LangKeys key, bool asc) _lastRequestsSortKey = (LangKeys.NotDefined, true);
        private (LangKeys key, bool asc) _lastListenersSortKey = (LangKeys.NotDefined, true);

        public ICommand SortRequestsCommand => new RelayCommand(obj =>
        {
            var langKey = Translator.GetLangKey((DependencyObject)obj);
            Requests = SortCollection(Requests, ref _lastRequestsSortKey, langKey,
                new Dictionary<LangKeys, string>()
                {
                    [LangKeys.Number] = nameof(RequestStatus.RequestNumber),
                    [LangKeys.ThreadId] = nameof(RequestStatus.ThreadId),
                    [LangKeys.Status] = nameof(RequestStatus.Status),
                    [LangKeys.TimeMs] = nameof(RequestStatus.DurationMs),
                });
        });

        public ICommand SortListenersCommand => new RelayCommand(obj =>
        {
            var langKey = Translator.GetLangKey((DependencyObject)obj);
            ListenerStatuses = SortCollection(ListenerStatuses, ref _lastListenersSortKey, langKey,
                new Dictionary<LangKeys, string>()
                {
                    [LangKeys.Time] = nameof(ListenerStatus.TimeReceived),
                    [LangKeys.ReqNumber] = nameof(ListenerStatus.RequestNumber),
                    [LangKeys.ThreadId] = nameof(ListenerStatus.ThreadId),
                    [LangKeys.Status] = nameof(ListenerStatus.Status),
                });
        });

        private CancellationTokenSource _listenerCancellationTokenSource;
        private void ChangeListenerState()
        {
            _listenerCancellationTokenSource?.Cancel(false);
            if (EnableListening == true)
            {
                _listenerCancellationTokenSource = new CancellationTokenSource();
                Task.Run(() =>
                {
                    try
                    {
                        _httpHelper.StartListen(OnListenerStatusCreated, _listenerCancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        ThreadHelper.RunInMainThread(() => MessageBox.Show(ex.Message, App.MessageContainer[LangKeys.UnhandledException] ?? "Something wrong"));
                        Debug.WriteLine(ex.ToString());
                    }
                });
            }
        }

        private void OnListenerStatusCreated(ListenerStatus listenerStatus)
        {
            ThreadHelper.RunInMainThread(async () =>
            {
                var allItems = ListenerStatuses.Concat(new[] { listenerStatus }).OrderBy(ls => ls.TimeReceived).ToList();
                ListenerStatuses.Insert(allItems.IndexOf(listenerStatus), listenerStatus);
                await Task.Delay(20);
            });
        }
    }
}
