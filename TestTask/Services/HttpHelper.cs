using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTask.ExtensionMethods;
using TestTask.Models;
using TestTask.Properties;

namespace TestTask.Services
{
    public class HttpHelper
    {
        public void StartListen(Action<ListenerStatus> createdListenerStatus, CancellationToken cancellationToken)
        {
            HttpListener listener = new HttpListener();
            // установка адресов прослушки
            var address = Settings.Default.LocalAddress.EndsWith("/")
                ? Settings.Default.LocalAddress
                : Settings.Default.LocalAddress + "/";
            listener.Prefixes.Add(address);
            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                var listenerStatus = new ListenerStatus();
                try
                {
                    // метод GetContext блокирует текущий поток, ожидая получение запроса 
                    HttpListenerContext context = listener.GetContext();
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    listenerStatus.TimeReceived = DateTime.Now;
                    HttpListenerRequest request = context.Request;

                    if (!listenerStatus.ParseQueryString(request))
                    {
                        listenerStatus.Status = App.MessageContainer?[LangKeys.InvalidRequest]?.Formatted(request.Url)
                                                ?? $"Invalid Request: {request.Url}";
                        listenerStatus.IsSuccess = false;
                        createdListenerStatus.Invoke(listenerStatus);
                        continue;
                    }

                    // получаем объект ответа
                    HttpListenerResponse response = context.Response;
                    // создаем ответ в виде кода html
                    var responseStr =
                        $"<HTML><BODY> Thread {listenerStatus.ThreadId}, request {listenerStatus.RequestNumber} </BODY></HTML>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseStr);
                    // получаем поток ответа и пишем в него ответ
                    response.ContentLength64 = buffer.Length;
                    using (Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }

                    listenerStatus.Status = App.MessageContainer?[LangKeys.ResponseSent] ?? "Answered";
                    listenerStatus.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    listenerStatus.Status = App.MessageContainer?[LangKeys.Error].Formatted(ex.Message) ?? $"Error: {ex.Message}";
                    listenerStatus.IsSuccess = false;
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (!listenerStatus.Status.IsNullOrEmpty())
                    {
                        createdListenerStatus.Invoke(listenerStatus);
                    }
                }
            } // while

            // останавливаем прослушивание подключений
            listener.Stop();
        }

        public async Task<ResponseInfo> SendRequest(string url, Dictionary<string, string> headers = null, CancellationToken? cancellationToken = null)
        {
            var response = new ResponseInfo();
            using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(Settings.Default.RequestTimeoutSec) })
            {
                headers?.ForEach(pair => client.DefaultRequestHeaders.Add(pair.Key, pair.Value));

                var httpResponse = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken ?? CancellationToken.None)
                    .ConfigureAwait(false);
                response.StatusCode = (int) httpResponse.StatusCode;
                response.Headers = httpResponse.Content.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
                response.Content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return response;
        }
    }
}
