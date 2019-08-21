using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType("statusType"), DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportFlightInfo
    {
        private readonly DuplexConversionTuple<string, FlightStatusCode> code =
            ModelHelpers.GetFlightStatusCodeConversion();
        private readonly DuplexConversionTuple<DateTime, long> time =
            ModelHelpers.GetUnixEpochConversion();

        [XmlAttribute("code", DataType = "NMTOKEN"), DataMember(Name = "code")]
        public string CodeString
        {
            get => code.RawValue;
            set => code.RawValue = value;
        }

        [XmlIgnore]
        public FlightStatusCode Code
        {
            get => code.ConvertedValue;
            set => code.ConvertedValue = value;
        }

        [XmlAttribute("time"), IgnoreDataMember]
        public DateTime Time
        {
            get => time.RawValue;
            set => time.RawValue = value;
        }

        [XmlIgnore, DataMember(Name = "time")]
        public long TimeEpoch
        {
            get => time.ConvertedValue;
            set => time.ConvertedValue = value;
        }

        private string DebuggerDisplay() => $"{nameof(AirportFlightInfo)}({nameof(Code)}: {CodeString} ({Code}), {Time})";
    }
}
