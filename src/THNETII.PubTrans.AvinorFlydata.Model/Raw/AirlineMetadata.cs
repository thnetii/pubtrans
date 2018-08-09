using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirlineMetadata
    {
        [DataMember(Name = "code")]
        [XmlAttribute("code", DataType = "NMTOKEN")]
        public string IataCode { get; set; }

        [DataMember(Name = "name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirlineMetadata)}(IATA: {IataCode}, {Name})";
    }

    [XmlRoot("airlineNames")]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirlineMetadataListing
    {
        [XmlElement("airlineName")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirlineMetadata[] Airlines { get; set; }

        private string DebuggerDisplay() => $"{nameof(AirlineMetadataListing)}({nameof(Airlines.Length)}: {Airlines?.Length})";
    }
}
