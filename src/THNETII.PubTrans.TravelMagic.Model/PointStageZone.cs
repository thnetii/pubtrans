using System.Diagnostics;
using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class PointStageZone
    {
        [XmlAttribute("v")]
        public int Id { get; set; }

        [XmlAttribute("n")]
        public string Name { get; set; }

        private string DebuggerDisplay()
        {
            return $"{GetType()}, {nameof(Id)} = {Id}, {nameof(Name)} = {Name}";
        }
    }
}
