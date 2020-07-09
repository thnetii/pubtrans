using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum NoteSeverity
    {
        [XmlEnum("unknown")]
        Unknown,
        [XmlEnum("normal")]
        Normal
    }
}
