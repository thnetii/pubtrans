using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class GateStatusText
    {
        private readonly DuplexConversionTuple<string, GateStatusCode> code =
            ModelHelpers.GetGateStatusCodeConversion();

        [XmlAttribute("code", DataType = "NMTOKEN"), DataMember(Name = "code")]
        public string CodeString
        {
            get => code.RawValue;
            set => code.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public GateStatusCode Code
        {
            get => code.ConvertedValue;
            set => code.ConvertedValue = value;
        }

        [XmlAttribute("gateStatusTextEn"), DataMember(Name = "gateStatusTextEn")]
        public string TextEnglish { get; set; }

        [XmlAttribute("gateStatusTextNo"), DataMember(Name = "gateStatusTextNo")]
        public string TextNorwegian { get; set; }

        private string DebuggerDisplay() => $"{nameof(GateStatusText)}({nameof(Code)}: {CodeString} ({Code}), {TextNorwegian})";
    }

    [XmlRoot("gateStatuses")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class GateStatusTextListing
    {
        [XmlElement("gateStatus")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public GateStatusText[] GateStatuses { get; set; }

        private string DebuggerDisplay() => $"{nameof(GateStatusTextListing)}({nameof(GateStatuses.Length)}: {GateStatuses?.Length})";
    }
}
