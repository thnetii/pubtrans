using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType("arr_depType"), DataContract]
    public enum FlightDirection
    {
        [XmlEnum(null), EnumMember(Value = null)]
        Unspecified = 0,
        [XmlEnum("A"), EnumMember(Value = "A")]
        Arrival,
        [XmlEnum("D"), EnumMember(Value = "D")]
        Departure,
        [XmlEnum(""), EnumMember(Value = "")]
        Both,
        Unknown = -1
    }
}
