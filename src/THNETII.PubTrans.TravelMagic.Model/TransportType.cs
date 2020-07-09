using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public enum TransportType
    {
        [XmlEnum("")]
        Unknown = 0,
        AirportExpressCoach = 1,
        [XmlEnum("Buss")]
        LocalBus = 2,
        ExpressCoach = 3,
        Other = 4,
        FerryBoat = 5,
        Train = 6,
        Tram = 7,
        Metro = 8
    }
}
