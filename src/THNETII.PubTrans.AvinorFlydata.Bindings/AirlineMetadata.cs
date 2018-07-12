using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Bindings
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirlineMetadata
    {
        [XmlAttribute("code", DataType = "NMTOKEN")]
        public string IataCode { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirlineMetadata)}(IATA: {IataCode}, {Name})";
    }

    [XmlRoot("airlineNames")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirlineMetadataListing
    {
        public static XmlSerializer DefaultSerializer
        { get => new XmlSerializer(typeof(AirlineMetadataListing)); }

        [DebuggerStepThrough]
        public AirlineMetadataListing() { }

        public AirlineMetadataListing(AirlineMetadata[] airlines) : this()
        {
            Airlines = airlines ?? Array.Empty<AirlineMetadata>();
        }

        [XmlElement("airlineName")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirlineMetadata[] Airlines { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirlineMetadataListing)}({nameof(Airlines.Length)}: {Airlines?.Length})";
    }
}
