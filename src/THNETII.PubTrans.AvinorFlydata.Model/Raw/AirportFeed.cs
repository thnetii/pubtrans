using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlRoot("airport")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportFeed
    {
        [XmlElement("flights")]
        [DataMember(Name = "flights")]
        public AirportFeedFlightListing Content { get; set; }

        [XmlAttribute("name", DataType = "NMTOKEN")]
        [DataMember(Name = "name")]
        public string AirportIata { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirportFeed)}({AirportIata}, {nameof(Content.Flights)}: {Content?.Flights?.Length})";
    }
}
