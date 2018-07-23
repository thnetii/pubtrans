using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Bindings
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class FlightStatusText
    {
        private readonly DuplexConversionTuple<string, FlightStatusCode> code =
            FlightStatusCodeHelpers.GetDuplexConversionTuple();

        [XmlAttribute("code", DataType = "NMTOKEN")]
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

        [XmlAttribute("statusTextEn")]
        public string TextEnglish { get; set; }

        [XmlAttribute("statusTextNo")]
        public string TextNorwegian { get; set; }

        private string DebuggerDisplay() => $"{nameof(FlightStatusText)}({nameof(Code)}: {CodeString} ({Code}), {TextNorwegian})";
    }

    [XmlRoot("flightStatuses")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class FlightStatusTextListing
    {
        public static XmlSerializer DefaultSerializer
        { get => new XmlSerializer(typeof(FlightStatusTextListing)); }

        [DebuggerStepThrough]
        public FlightStatusTextListing() { }

        public FlightStatusTextListing(FlightStatusText[] statuses) : this()
        {
            FlightStatuses = statuses ?? Array.Empty<FlightStatusText>();
        }

        [XmlElement("flightStatus")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public FlightStatusText[] FlightStatuses { get; set; }

        private string DebuggerDisplay() => $"{nameof(FlightStatusTextListing)}({nameof(FlightStatuses.Length)}: {FlightStatuses?.Length})";
    }
}
