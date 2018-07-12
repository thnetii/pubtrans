using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Bindings
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportMetadata
    {
        [XmlAttribute("code", DataType = "NMTOKEN")]
        public string IataCode { get; set; }

        [XmlAttribute("icao", DataType = "NMTOKEN")]
        public string IcaoCode { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("name_uk")]
        public string NameUK { get; set; }

        [XmlAttribute("shortname8")]
        public string Shortname8 { get; set; }

        [XmlAttribute("shortname8_uk")]
        public string Shortname8UK { get; set; }

        [XmlAttribute("shortname15")]
        public string Shortname15 { get; set; }

        [XmlAttribute("shortname15_uk")]
        public string Shortname15UK { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirportMetadata)}(IATA: {IataCode}, {Name})";
    }

    [XmlRoot("airportNames")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportMetadataListing
    {
        public static XmlSerializer DefaultSerializer
        { get => new XmlSerializer(typeof(AirportMetadataListing)); }

        [DebuggerStepThrough]
        public AirportMetadataListing() { }

        public AirportMetadataListing(AirportMetadata[] airports) : this()
        {
            Airports = airports ?? Array.Empty<AirportMetadata>();
        }

        [XmlElement("airportName")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirportMetadata[] Airports { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirportMetadataListing)}({nameof(Airports.Length)}: {Airports?.Length})";
    }
}
