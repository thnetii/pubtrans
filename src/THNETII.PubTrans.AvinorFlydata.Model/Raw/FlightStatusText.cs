using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class FlightStatusText
    {
        private readonly DuplexConversionTuple<string, FlightStatusCode> code =
            ModelHelpers.GetFlightStatusCodeConversion();

        [XmlAttribute("code", DataType = "NMTOKEN"), DataMember(Name = "code")]
        public string CodeString
        {
            get => code.RawValue;
            set => code.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public FlightStatusCode Code
        {
            get => code.ConvertedValue;
            set => code.ConvertedValue = value;
        }

        [XmlAttribute("statusTextEn"), DataMember(Name = "statusTextEn")]
        public string TextEnglish { get; set; }

        [XmlAttribute("statusTextNo"), DataMember(Name = "statusTextNo")]
        public string TextNorwegian { get; set; }

        private string DebuggerDisplay() => $"{nameof(FlightStatusText)}({nameof(Code)}: {CodeString} ({Code}), {TextNorwegian})";
    }

    [XmlRoot("flightStatuses")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class FlightStatusTextListing
    {
        [XmlElement("flightStatus")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public FlightStatusText[] FlightStatuses { get; set; }

        private string DebuggerDisplay() => $"{nameof(FlightStatusTextListing)}({nameof(FlightStatuses.Length)}: {FlightStatuses?.Length})";
    }
}
