using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using THNETII.Common.Collections.Generic;
using THNETII.Networking.Http;
using THNETII.PubTrans.TravelMagic.Model;

namespace THNETII.PubTrans.TravelMagic.Client
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    [DebuggerDisplay("{" + nameof(ToQueryString) + "()}")]
    public struct DepartureSearchRequest
    {
        private static readonly ArrayPool<KeyValuePair<string, string>> queryPool =
            ArrayPool<KeyValuePair<string, string>>.Create();

        private const string from = nameof(from);
        private const string date = nameof(date);
        private const string time = nameof(time);
        private const string realtime = nameof(realtime);
        private const string now = nameof(now);
        private const string lines = "linjer";

        private const string @true = "1";

        public DepartureSearchRequest(string from) : this() => From = from;

        public string From { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IncludeRealtime { get; set; }
        public bool SearchFromCurrentTime { get; set; }
        public IEnumerable<string> Lines { get; set; }

        public string ToQueryString()
        {
            var queryArgs = queryPool.Rent(5);
            try
            {
                int queryIdx = 0;
                queryArgs[queryIdx++] = (from, From).AsKeyValuePair();
                if (SearchFromCurrentTime)
                    queryArgs[queryIdx++] = (now, @true).AsKeyValuePair();
                else if (DateTime.HasValue)
                {
                    queryArgs[queryIdx++] = (date, DateTime.Value.ToString("d", TravelMagicUtils.Culture)).AsKeyValuePair();
                    queryArgs[queryIdx++] = (time, DateTime.Value.ToString("T", TravelMagicUtils.Culture)).AsKeyValuePair();
                }
                if (IncludeRealtime)
                    queryArgs[queryIdx++] = (realtime, @true).AsKeyValuePair();
                if (!(Lines is null))
                    queryArgs[queryIdx++] = (lines, string.Join(",", Lines)).AsKeyValuePair();

                return HttpUrlHelper.ToQueryString(queryArgs.AsSpan(0, queryIdx));
            }
            finally
            {
                queryPool.Return(queryArgs, clearArray: true);
            }
        }
    }
}
