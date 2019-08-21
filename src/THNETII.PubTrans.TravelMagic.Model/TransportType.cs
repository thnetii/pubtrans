using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum TransportType
    {
        [XmlEnum("")]
        Unknown,
        [XmlEnum("Buss")]
        Bus
    }
}
