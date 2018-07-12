using System;
using System.Xml.Serialization;
using THNETII.Common;
using THNETII.Common.XmlSerializer;

namespace THNETII.PubTrans.AvinorFlydata.Bindings
{
    [XmlType(TypeName = "statusCodeType")]
    public enum FlightStatusCode
    {
        [XmlEnum("")]
        Unspecified = 0,
        [XmlEnum("N")]
        NewInfo,
        [XmlEnum("E")]
        EstimatedTime,
        [XmlEnum("A")]
        Landed,
        [XmlEnum("C")]
        Cancelled,
        [XmlEnum("D")]
        Departed,
        [XmlEnum("O")]
        Arrived,
        Unknown = -1
    }

    public static class FlightStatusCodeHelpers
    {
        public static DuplexConversionTuple<string, FlightStatusCode> GetDuplexConversionTuple()
        {
            return new DuplexConversionTuple<string, FlightStatusCode>(
                GetCodeFromString, StringComparer.OrdinalIgnoreCase,
                XmlEnumStringConverter<FlightStatusCode>.ToString 
                );
        }

        private static FlightStatusCode GetCodeFromString(string s)
        {
            FlightStatusCode GetUnknownOrUnspecified(string u)
            {
                return string.IsNullOrWhiteSpace(u)
                    ? FlightStatusCode.Unspecified
                    : FlightStatusCode.Unknown
                    ;
            }
            return XmlEnumStringConverter<FlightStatusCode>.ParseOrDefault(s,
                GetUnknownOrUnspecified
                );
        }
    }
}
