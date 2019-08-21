using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum NoteSituationType
    {
        Unknown,
        [XmlEnum("situation")]
        Situation
    }
}
