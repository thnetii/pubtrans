using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportMetadata
    {
        [XmlAttribute("code", DataType = "NMTOKEN"), DataMember(Name = "code")]
        public string IataCode { get; set; }

        [XmlAttribute("icao", DataType = "NMTOKEN"), DataMember(Name = "icao")]
        public string IcaoCode { get; set; }

        [XmlAttribute("name"), DataMember(Name = "name")]
        public string Name { get; set; }

        [XmlAttribute("name_uk"), DataMember(Name = "nameUk")]
        public string NameUK { get; set; }

        [XmlAttribute("shortname8"), DataMember(Name = "shortname8")]
        public string Shortname8 { get; set; }

        [XmlAttribute("shortname8_uk"), DataMember(Name = "shortname8Uk")]
        public string Shortname8UK { get; set; }

        [XmlAttribute("shortname15"), DataMember(Name = "shortname15")]
        public string Shortname15 { get; set; }

        [XmlAttribute("shortname15_uk"), DataMember(Name = "shortname15Uk")]
        public string Shortname15UK { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirportMetadata)}(IATA: {IataCode}, {Name})";
    }

    [XmlRoot("airportNames")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportMetadataListing
    {
        [XmlElement("airportName")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirportMetadata[] Airports { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirportMetadataListing)}({nameof(Airports.Length)}: {Airports?.Length})";
    }
}
