using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, XmlRoot("result")]
    public class DepartureSearchResult
    {
        [XmlArray("departures")]
        [XmlArrayItem("i")]
        public DepartureItem[] Departures { get; set; }

        [XmlArray("stages")]
        [XmlArrayItem("i")]
        public PointStageItem[] Stages { get; set; }
    }
}
