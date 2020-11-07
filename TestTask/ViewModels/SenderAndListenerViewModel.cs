using System;
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
            RequestsCount = Settings.Default.DefaultRequestsCount;
            App.MessageContainerChanged += App_MessageContainerChanged;
            EnableListening = true;
        }

        private int _currentRequestNumber = 1;
        private HttpHelper _httpHelper = new HttpHelper();

        private void App_MessageContainerChanged(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(nameof(RequestInCase));
        }

        public ObservableCollection<RequestStatus> Requests { get; } = new ObservableCollection<RequestStatus>();
        public ObservableCollection<ListenerStatus> ListenerStatuses { get; private set; } = new ObservableCollection<ListenerStatus>();

        public int RequestsCount
        {
            get => GetVal<int>();
            set => SetVal(value, () => RaisePropertyChanged(nameof(RequestInCase)));
        }

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

        public ICommand SendRequestsCommand => new RelayCommand(_ =>
        {
            for (int i = 0; i < RequestsCount; i++)
            {
                Requests.Add(new RequestStatus(_currentRequestNumber++));
            }
        });

        public ICommand ClearStatusesCommand => new RelayCommand(_ =>
        {
            ListenerStatuses.Clear();
        });

        public ICommand ClearRequestsCommand => new RelayCommand(_ =>
        {
            Requests.ForEach(ss => ss.AbortRequest());
            Requests.Clear();
        });

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
