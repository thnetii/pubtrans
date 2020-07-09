using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [XmlType("beltStatusType")]
    [DataContract]
    public enum BeltStatusCode
    {
        [XmlEnum(""), EnumMember(Value = "")]
        Unspecified = 0,
        [XmlEnum("O"), EnumMember(Value = "O")]
        BagOnBelt,
        [XmlEnum("C"), EnumMember(Value = "C")]
        LastBagOnBelt,
        Unknown = -1
    }
}
