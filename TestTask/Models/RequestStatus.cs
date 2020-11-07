using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TestTask.ExtensionMethods;
using TestTask.Properties;
using TestTask.Services;

namespace TestTask.Models
{
    public class RequestStatus : StatusBase
    {
        private readonly HttpHelper _httpHelper = new HttpHelper();
        private readonly Regex _numbersRegex = new Regex(@"Thread[\s](\d*).*request[\s](\d*)");

        public readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public RequestStatus(int requestNumber)
        {
            RequestNumber = requestNumber;
            StartRequestThread();
        }

        public void StartRequestThread()
        {
            var thread = new Thread(_ => SendRequest());
            thread.Start();
        }

        public async void SendRequest()
        {
            var startTime = DateTime.Now;
            Status = App.MessageContainer?[LangKeys.WaitingForResponse] ?? "Waiting For Response";
            IsSuccess = null;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            var url = $"{Settings.Default.LocalAddress}?ThreadId={ThreadId}&RequestNumber={RequestNumber}";
            ResponseInfo responseInfo;
            try
            {
                responseInfo = await _httpHelper.SendRequest(url, cancellationToken: CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                responseInfo = new ResponseInfo()
                    { StatusCode = 500, Content = ex.InnerException?.Message ?? ex.Message };
            }

            DurationMs = (int) (DateTime.Now - startTime).TotalMilliseconds;
            if (!responseInfo.IsSuccess)
            {
                Status = (App.MessageContainer?[LangKeys.RequestError] ?? "Error: {0}").Formatted(responseInfo.Content);
                IsSuccess = false;
                return;
            }

            var match = _numbersRegex.Match(responseInfo.Content);
            if (match.Success && match.Groups.Count == 3
                              && int.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var threadIdFromResponse)
                              && int.TryParse(match.Groups[2].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var requestNumberFromResponse)
                              && threadIdFromResponse == ThreadId && requestNumberFromResponse == RequestNumber)
            {
                Status = App.MessageContainer?[LangKeys.Success] ?? "Success";
                IsSuccess = true;
                return;
            }

            Status = App.MessageContainer?[LangKeys.BadResponseFormat]?.Formatted(responseInfo.Content) ?? $"Bad Response: {responseInfo.Content}";
            IsSuccess = false;
        }

        public int DurationMs
        {
            get => GetVal<int>();
            protected set => SetVal(value);
        }

        public void AbortRequest()
        {
            CancellationTokenSource.Cancel(false);
        }
    }
}
