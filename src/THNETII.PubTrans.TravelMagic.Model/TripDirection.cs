using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum TripDirection
    {
        Unknown = 0,
        [XmlEnum("Tur")]
        Trip = 1,
        [XmlEnum("Retur")]
        ReturnTrip = 2,
        RoundTrip = 3
    }
}
