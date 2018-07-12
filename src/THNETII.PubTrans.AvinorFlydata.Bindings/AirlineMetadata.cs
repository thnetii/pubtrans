using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;

namespace THNETII.PubTrans.AvinorFlydata.Bindings
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirlineMetadata
    {
        [XmlAttribute("code")]
        public string IataCode { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        private string DebuggerDisplay() => $"{GetType()}(IATA: {IataCode}, {Name})";
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
            AirlineNames = airlines ?? Array.Empty<AirlineMetadata>();
        }

        [XmlElement("airlineName")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirlineMetadata[] AirlineNames { get; set; }

        private string DebuggerDisplay() => $"{GetType()}({nameof(AirlineNames.Length)}: {AirlineNames?.Length})";
    }
}
