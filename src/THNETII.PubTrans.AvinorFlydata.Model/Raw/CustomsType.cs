using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType("dom_intType"), DataContract]
    public enum CustomsType
    {
        [XmlEnum(""), EnumMember(Value = "")]
        Unspecified = 0,
        [XmlEnum("D"), EnumMember(Value = "D")]
        Domestic,
        [XmlEnum("I"), EnumMember(Value = "I")]
        International,
        [XmlEnum("S"), EnumMember(Value = "S")]
        SchengenArea,
        Unknown = -1,
    }
}
