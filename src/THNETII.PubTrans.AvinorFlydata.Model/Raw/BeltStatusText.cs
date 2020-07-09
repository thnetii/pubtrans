using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType, DataContract, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class BeltStatusText
    {
        private readonly DuplexConversionTuple<string, BeltStatusCode> code =
            ModelHelpers.GetBeltStatusCodeConversion();

        [XmlAttribute("code"), DataMember(Name = "code")]
        public string CodeString
        {
            get => code.RawValue;
            set => code.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public BeltStatusCode Code
        {
            get => code.ConvertedValue;
            set => code.ConvertedValue = value;
        }

        [XmlAttribute("beltStatusTextEn"), DataMember(Name = "beltStatusTextEn")]
        public string TextEnglish { get; set; }

        [XmlAttribute("beltStatusTextNo"), DataMember(Name = "beltStatusTextNo")]
        public string TextNorwegian { get; set; }

        private string DebuggerDisplay() => $"{nameof(GateStatusText)}({nameof(Code)}: {CodeString} ({Code}), {TextNorwegian})";
    }

    [XmlRoot("beltStatuses")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class BeltStatusTextListing
    {
        [XmlElement("beltStatus")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public BeltStatusText[] BeltStatuses { get; set; }

        private string DebuggerDisplay() => $"{nameof(BeltStatusTextListing)}({nameof(BeltStatuses.Length)}: {BeltStatuses?.Length})";
    }
}
