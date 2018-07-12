using System.Diagnostics;
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
        public string CodeString {
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
}
