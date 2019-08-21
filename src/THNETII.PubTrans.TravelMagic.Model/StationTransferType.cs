using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum StationTransferType
    {
        [XmlEnum("0")]
        None = 0,
        [XmlEnum("1")]
        Possible = 1,
        [XmlEnum("2")]
        Priority = 2
    }
}
