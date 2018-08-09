using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType("gateStatusType"), DataContract]
    public enum GateStatusCode
    {
        [XmlEnum(""), EnumMember(Value = "")]
        Unspecified = 0,
        [XmlEnum("O"), EnumMember(Value = "O")]
        GoToGate,
        [XmlEnum("P"), EnumMember(Value = "P")]
        Preboarding,
        [XmlEnum("B"), EnumMember(Value = "B")]
        Boarding,
        [XmlEnum("F"), EnumMember(Value = "F")]
        GateClosing,
        [XmlEnum("C"), EnumMember(Value = "C")]
        Closed,
        Unknown = -1
    }
}
