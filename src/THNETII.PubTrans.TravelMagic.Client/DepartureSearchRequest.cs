using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using THNETII.Networking.Http;
using THNETII.PubTrans.TravelMagic.Model;

namespace THNETII.PubTrans.TravelMagic.Client
{
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types")]
    [DebuggerDisplay("{" + nameof(ToQueryString) + "()}")]
    public struct DepartureSearchRequest
    {
        private const string from = nameof(from);
        private const string date = nameof(date);
        private const string time = nameof(time);
        private const string realtime = nameof(realtime);
        private const string now = nameof(now);
        private const string lines = "linjer";

        private const string @true = "1";

        private const int idxFrom = 0;
        private const int idxDate = 1;
        private const int idxTime = 2;
        private const int idxRealtime = 3;
        private const int idxNow = 4;
        private const int idxLines = 5;

        public DepartureSearchRequest(string from) : this() => From = from;

        public string From { get; set; }
        public DateTime? DateTime { get; set; }
        public bool IncludeRealtime { get; set; }
        public bool SearchFromCurrentTime { get; set; }
        public IEnumerable<string> Lines { get; set; }

        public unsafe string ToQueryString()
        {
            Span<IntPtr> pKeys = stackalloc IntPtr[6];
            Span<int> cKeys = stackalloc int[6];
            Span<IntPtr> pValues = stackalloc IntPtr[6];
            Span<int> cValues = stackalloc int[6];

            return GetQueryStringFrom(this, pKeys, cKeys, pValues, cValues);

            unsafe string GetQueryStringFrom(in DepartureSearchRequest req,
                Span<IntPtr> pKey, Span<int> cKey,
                Span<IntPtr> pVal, Span<int> cVal)
            {
                fixed (char* pKeyFrom = from)
                fixed (char* pValFrom = req.From)
                {
                    pKey[idxFrom] = new IntPtr(pKeyFrom);
                    cKey[idxFrom] = from.Length;
                    pVal[idxFrom] = new IntPtr(pValFrom);
                    cVal[idxFrom] = req.From?.Length ?? 0;

                    return GetQueryStringDateTimeOrNow(req, pKey, cKey, pVal, cVal);
                }
            }

            unsafe string GetQueryStringDateTimeOrNow(in DepartureSearchRequest req,
                Span<IntPtr> pKey, Span<int> cKey,
                Span<IntPtr> pVal, Span<int> cVal)
            {
                if (req.SearchFromCurrentTime)
                {
                    fixed (char* pKeyNow = now)
                    fixed (char* pValNow = @true)
                    {
                        pKey[idxNow] = new IntPtr(pKeyNow);
                        cKey[idxNow] = now.Length;
                        pVal[idxNow] = new IntPtr(pValNow);
                        cVal[idxNow] = @true.Length;

                        pKey[idxDate] = pKey[idxTime] = IntPtr.Zero;

                        return GetQueryStringRealtime(req, pKey, cKey, pVal, cVal);
                    }
                }
                else if (req.DateTime.HasValue)
                {
                    var dt = req.DateTime.Value;
                    var sValDate = dt.ToString("d", TravelMagicUtils.Culture);
                    var sValTime = dt.ToString("T", TravelMagicUtils.Culture);
                    fixed (char* pKeyDate = date)
                    fixed (char* pValDate = sValDate)
                    fixed (char* pKeyTime = time)
                    fixed (char* pValTime = sValTime)
                    {
                        pKey[idxDate] = new IntPtr(pKeyDate);
                        cKey[idxDate] = date.Length;
                        pVal[idxDate] = new IntPtr(pValDate);
                        cVal[idxDate] = sValDate.Length;
                        pKey[idxTime] = new IntPtr(pKeyTime);
                        cKey[idxTime] = time.Length;
                        pVal[idxTime] = new IntPtr(pValTime);
                        cVal[idxTime] = sValTime.Length;

                        pKey[idxNow] = IntPtr.Zero;

                        return GetQueryStringRealtime(req, pKey, cKey, pVal, cVal);
                    }
                }
                else
                {
                    pKey[idxNow] = pKey[idxDate] = pKey[idxTime] = IntPtr.Zero;
                    return GetQueryStringRealtime(req, pKey, cKey, pVal, cVal);
                }
            }

            unsafe string GetQueryStringRealtime(in DepartureSearchRequest req,
                Span<IntPtr> pKey, Span<int> cKey,
                Span<IntPtr> pVal, Span<int> cVal)
            {
                if (req.IncludeRealtime)
                {
                    fixed (char* pKeyRealtime = realtime)
                    fixed (char* pValRealtime = @true)
                    {
                        pKey[idxRealtime] = new IntPtr(pKeyRealtime);
                        cKey[idxRealtime] = realtime.Length;
                        pVal[idxRealtime] = new IntPtr(pValRealtime);
                        cVal[idxRealtime] = @true.Length;

                        return GetQueryStringLines(req, pKey, cKey, pVal, cVal);
                    }
                }
                else
                {
                    pKey[idxRealtime] = IntPtr.Zero;
                    return GetQueryStringLines(req, pKey, cKey, pVal, cVal);
                }
            }

            unsafe string GetQueryStringLines(in DepartureSearchRequest req,
                Span<IntPtr> pKey, Span<int> cKey,
                Span<IntPtr> pVal, Span<int> cVal)
            {
                if (!(req.Lines is null))
                {
                    var sLines = string.Join(",", req.Lines);
                    fixed (char* pKeyLines = lines)
                    fixed (char* pValLines = sLines)
                    {
                        pKey[idxLines] = new IntPtr(pKeyLines);
                        cKey[idxLines] = lines.Length;
                        pVal[idxLines] = new IntPtr(pValLines);
                        cVal[idxLines] = sLines.Length;

                        return GetQueryStringFinal(pKey, cKey, pVal, cVal);
                    }
                }
                else
                {
                    pKey[idxLines] = IntPtr.Zero;
                    return GetQueryStringFinal(pKey, cKey, pVal, cVal);
                }
            }

            string GetQueryStringFinal(Span<IntPtr> pKey, Span<int> cKey,
                Span<IntPtr> pVal, Span<int> cVal) =>
                HttpUrlHelper.ToQueryString(pKey, cKey, pVal, cVal);
        }
    }
}
