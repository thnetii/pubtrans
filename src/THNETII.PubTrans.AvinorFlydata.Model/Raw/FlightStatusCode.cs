using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType(TypeName = "statusCodeType"), DataContract]
    public enum FlightStatusCode
    {
        [XmlEnum(""), EnumMember(Value = "")]
        Unspecified = 0,
        [XmlEnum("N"), EnumMember(Value = "N")]
        NewInfo,
        [XmlEnum("E"), EnumMember(Value = "E")]
        EstimatedTime,
        [XmlEnum("A"), EnumMember(Value = "A")]
        Landed,
        [XmlEnum("C"), EnumMember(Value = "C")]
        Cancelled,
        [XmlEnum("D"), EnumMember(Value = "D")]
        Departed,
        [XmlEnum("O"), EnumMember(Value = "O")]
        Arrived,
        Unknown = -1
    }
}
