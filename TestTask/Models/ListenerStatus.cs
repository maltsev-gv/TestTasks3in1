using System;
using System.Globalization;
using System.Net;

namespace TestTask.Models
{
    public class ListenerStatus : StatusBase
    {
        public bool ParseQueryString(HttpListenerRequest request)
        {
            var query = request.QueryString;
            var threadIdStr = query[nameof(ThreadId)];
            var reqNumStr = query[nameof(RequestNumber)];
            if (reqNumStr != null && int.TryParse(reqNumStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var reqNum)
                && threadIdStr != null && int.TryParse(threadIdStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var threadId))
            {
                RequestNumber = reqNum;
                ThreadId = threadId;
                return true;
            }

            return false;
        }

        public DateTime TimeReceived { get; set; }
    }
}
