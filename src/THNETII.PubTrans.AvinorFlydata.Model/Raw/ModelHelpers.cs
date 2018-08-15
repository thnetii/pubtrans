using System;
using System.Collections.Generic;
using System.Linq;
using THNETII.Common;
using THNETII.Common.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    public static class ModelHelpers
    {
        #region String to Enum conversion helpers
        public static DuplexConversionTuple<string, FlightStatusCode> GetFlightStatusCodeConversion()
        {
            return new DuplexConversionTuple<string, FlightStatusCode>(
                GetFlightStatusCodeFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter.ToString
                );
        }

        public static DuplexConversionTuple<string, GateStatusCode> GetGateStatusCodeConversion()
        {
            return new DuplexConversionTuple<string, GateStatusCode>(
                GetGateStatusCodeFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter.ToString
                );
        }

        public static DuplexConversionTuple<string, BeltStatusCode> GetBeltStatusCodeConversion()
        {
            return new DuplexConversionTuple<string, BeltStatusCode>(
                GetBeltStatusCodeFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter.ToString
                );
        }

        public static DuplexConversionTuple<string, FlightDirection> GetFlightDirectionConversion()
        {
            return new DuplexConversionTuple<string, FlightDirection>(
                GetFlightDirectionFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter.ToString
                );
        }

        public static DuplexConversionTuple<string, CustomsType> GetCustomsTypeConversion()
        {
            return new DuplexConversionTuple<string, CustomsType>(
                GetCustomsTypeFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter.ToString
                );
        }

        private static FlightStatusCode GetFlightStatusCodeFromString(string s)
        {
            FlightStatusCode GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? FlightStatusCode.Unspecified
                    : FlightStatusCode.Unknown
                    ;
            }
            return XmlEnumStringConverter.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }

        private static GateStatusCode GetGateStatusCodeFromString(string s)
        {
            GateStatusCode GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? GateStatusCode.Unspecified
                    : GateStatusCode.Unknown
                    ;
            }
            return XmlEnumStringConverter.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }

        private static BeltStatusCode GetBeltStatusCodeFromString(string s)
        {
            BeltStatusCode GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? BeltStatusCode.Unspecified
                    : BeltStatusCode.Unknown
                    ;
            }
            return XmlEnumStringConverter.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }

        private static CustomsType GetCustomsTypeFromString(string s)
        {
            CustomsType GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? CustomsType.Unspecified
                    : CustomsType.Unknown
                    ;
            }
            return XmlEnumStringConverter.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }

        private static FlightDirection GetFlightDirectionFromString(string s)
        {
            FlightDirection GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? FlightDirection.Unspecified
                    : FlightDirection.Unknown
                    ;
            }
            return XmlEnumStringConverter.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }
        #endregion
        #region String to collection helpers
        public static DuplexConversionTuple<string, IReadOnlyList<T>> GetCommaSeparatedStringToTConversion<T>(
            Func<string, T> singleItemConverter,
            Func<T, string> singleItemSerialiser,
            int? capacity = default)
        {
            string ToCommaSeparatedString(IReadOnlyList<T> l)
            {
                if (l == null)
                    return null;
                return string.Join(",", l.Select(t => singleItemSerialiser(t)));
            }

            return new DuplexConversionTuple<string, IReadOnlyList<T>>(
                s => ConvertCommaSeparatedString(s, singleItemConverter, capacity),
                ToCommaSeparatedString);
        }

        private static IReadOnlyList<T> ConvertCommaSeparatedString<T>(string cs,
            Func<string, T> singleItemConverter, int? capacity = default)
        {
            if (string.IsNullOrWhiteSpace(cs))
                return Array.Empty<T>();
            List<T> result = capacity.HasValue
                ? new List<T>(capacity.Value)
                : new List<T>();
            var remaining = cs.AsSpan();
            int nextComma = remaining.IndexOf(',');
            int remainingLength = remaining.Length;
            while (remainingLength > 0)
            {
                ReadOnlySpan<char> nextItem;
                if (nextComma < 1)
                {
                    nextItem = remaining.Trim();
                    remaining = ReadOnlySpan<char>.Empty;
                }
                else
                {
                    nextItem = remaining.Slice(0, nextComma).Trim();
                    remaining = remaining.Slice(nextComma + 1);
                }
                if (nextItem.Length < 1)
                    continue;
                var nextString = nextItem.ToString();
                result.Add(singleItemConverter(nextString));
                nextComma = remaining.IndexOf(',');
                remainingLength = remaining.Length;
            }
            return result.AsReadOnly();
        }
        #endregion
        #region String to bool helpers
        public static DuplexConversionTuple<string, bool?> GetYesNoBooleanNullableConversion()
        {
            bool? ToBoolNullable(string s)
            {
                switch (s)
                {
                    case string _ when string.Equals(s, "Y", StringComparison.OrdinalIgnoreCase):
                        return true;
                    case string _ when string.Equals(s, "N", StringComparison.OrdinalIgnoreCase):
                        return false;
                    default:
                        return null;
                }
            }
            string FromBoolNullable(bool? b)
            {
                if (b.HasValue)
                    return b.Value ? "Y" : "N";
                return null;
            }
            return new DuplexConversionTuple<string, bool?>(
                ToBoolNullable, StringComparer.OrdinalIgnoreCase,
                FromBoolNullable);
        }
        #endregion
        #region UNIX Epoch Conversion
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 01, 01, 00, 00, 00, DateTimeKind.Utc);

        public static DuplexConversionTuple<DateTime, long> GetUnixEpochConversion()
        {
            long ToUnixEpoch(DateTime dt)
            {
                if (dt == default)
                    return 0L;
                var ts = dt - UnixEpoch;
                return (long)(ts.TotalMilliseconds);
            }
            DateTime FromUnixEpoch(long ep)
            {
                if (ep == 0L)
                    return default;
                return UnixEpoch.AddMilliseconds(ep);
            }

            return new DuplexConversionTuple<DateTime, long>(
                ToUnixEpoch, FromUnixEpoch);
        }
        #endregion
    }
}
