using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using THNETII.Common;
using THNETII.TypeConverter.Xml;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public static class TravelMagicUtils
    {
        public static CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("nb-no");
        public static StringComparer StringComparer { get; } = StringComparer.Create(Culture, ignoreCase: false);
        public static StringComparer StringComparerCaseInsensitive { get; } = StringComparer.Create(Culture, ignoreCase: true);

        public static DuplexConversionTuple<string, double> GetDoubleConversionTuple() =>
            new DuplexConversionTuple<string, double>(
                s => double.Parse(s, Culture),
                StringComparerCaseInsensitive,
                d => d.ToString(Culture)
                );

        public static DuplexConversionTuple<string, DateTime> GetDateTimeConversionTuple() =>
            new DuplexConversionTuple<string, DateTime>(
                s => DateTime.Parse(s, TravelMagicUtils.Culture, System.Globalization.DateTimeStyles.AssumeLocal),
                TravelMagicUtils.StringComparerCaseInsensitive,
                d => d.ToString(TravelMagicUtils.Culture)
                );

        public static DuplexConversionTuple<string, TransportType> GetTransportTypeTuple() =>
            new DuplexConversionTuple<string, TransportType>(
                s => XmlEnumStringConverter.ParseOrDefault(s, TransportType.Unknown),
                StringComparerCaseInsensitive,
                t => XmlEnumStringConverter.ToString(t)
                );

        public static ConversionTuple<string, IReadOnlyList<ConversionTuple<string, TransportType>>> GetTransportTypeListTuple()
        {
            return new ConversionTuple<string, IReadOnlyList<ConversionTuple<string, TransportType>>>(
                s =>
                {
                    var l = ListFromCommaSeparatedString(s)?
                        .Distinct(StringComparerCaseInsensitive)
                        ?? Enumerable.Empty<string>();
                    var t = l.Select(i =>
                    {
                        var tpl = GetTransportTypeTuple();
                        tpl.RawValue = i;
                        return (ConversionTuple<string, TransportType>)tpl;
                    });
                    return t.ToList();
                },
                StringComparerCaseInsensitive
            );
        }

        internal static IEnumerable<string> EnumerableFromCommaSeparatedString(string s)
        {
            return s?.Split(',')
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                ;
        }

        public static List<string> ListFromCommaSeparatedString(string s) =>
            EnumerableFromCommaSeparatedString(s)?.ToList();

        public static List<string> DistincListFromCommaSeparatedString(string s) =>
            EnumerableFromCommaSeparatedString(s)?
            .Distinct(StringComparerCaseInsensitive)
            .ToList();
    }
}
