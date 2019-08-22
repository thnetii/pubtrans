using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using THNETII.Common;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class PointStageItem
    {
        private readonly DuplexConversionTuple<int, TimeSpan> m =
            new DuplexConversionTuple<int, TimeSpan>(
                m => TimeSpan.FromMinutes(m),
                t => (int)t.TotalMinutes
                );
        private readonly DuplexConversionTuple<string, double> x =
            TravelMagicUtils.GetDoubleConversionTuple();
        private readonly DuplexConversionTuple<string, double> y =
            TravelMagicUtils.GetDoubleConversionTuple();
        private readonly ConversionTuple<string, IReadOnlyList<ConversionTuple<string, TransportType>>> tn =
            TravelMagicUtils.GetTransportTypeListTuple();
        private readonly ConversionTuple<string, IReadOnlyList<string>> st =
            new ConversionTuple<string, IReadOnlyList<string>>(
                s => TravelMagicUtils.DistincListFromCommaSeparatedString(s),
                TravelMagicUtils.StringComparerCaseInsensitive
                );
        private readonly ConversionTuple<string, IReadOnlyList<string>> l =
            new ConversionTuple<string, IReadOnlyList<string>>(
                s => TravelMagicUtils.DistincListFromCommaSeparatedString(s),
                TravelMagicUtils.StringComparerCaseInsensitive
                );
        private readonly ConversionTuple<string, IReadOnlyList<int>> lineref =
            new ConversionTuple<string, IReadOnlyList<int>>(
                s => TravelMagicUtils.EnumerableFromCommaSeparatedString(s)
                    .Distinct(TravelMagicUtils.StringComparerCaseInsensitive)
                    .Select(v => int.TryParse(v, NumberStyles.Integer, TravelMagicUtils.Culture, out int r) ? (int?)r : null)
                    .Where(v => v.HasValue).Select(v => v.Value)
                    .ToList(),
                TravelMagicUtils.StringComparerCaseInsensitive
                );

        [XmlAttribute("v")]
        public string Id { get; set; }

        [XmlAttribute("hplnr")]
        public int StationNumber { get; set; }

        [XmlAttribute("stopnr")]
        public int PlatformNumber { get; set; }

        [XmlAttribute("n")]
        public string Name { get; set; }

        [XmlAttribute("t")]
        public StationTransferType TransferType { get; set; }

        [XmlAttribute("d")]
        public int Distance { get; set; }

        [XmlAttribute("m")]
        public int WalkingTimeMinutes
        {
            get => m.RawValue;
            set => m.RawValue = value;
        }

        [XmlIgnore]
        public TimeSpan WalkingTime
        {
            get => m.ConvertedValue;
            set => m.ConvertedValue = value;
        }

        [XmlAttribute("x")]
        public string XCoordinateString
        {
            get => x.RawValue;
            set => x.RawValue = value;
        }

        [XmlIgnore]
        public double X
        {
            get => x.ConvertedValue;
            set => x.ConvertedValue = value;
        }

        [XmlAttribute("y")]
        public string YCoordinateString
        {
            get => y.RawValue;
            set => y.RawValue = value;
        }

        [XmlIgnore]
        public double Y
        {
            get => y.ConvertedValue;
            set => y.ConvertedValue = value;
        }

        [XmlAttribute("tn")]
        public string TransportTypeNames
        {
            get => tn.RawValue;
            set => tn.RawValue = value;
        }

        [XmlIgnore]
        public IReadOnlyList<ConversionTuple<string, TransportType>> TransportTypes =>
            tn.ConvertedValue;

        [XmlAttribute("st")]
        public string TransportTypeSymbolPathString
        {
            get => st.RawValue;
            set => st.RawValue = value;
        }

        [XmlIgnore]
        public IReadOnlyList<string> TransportTypeSymbolPaths => st.ConvertedValue;

        [XmlAttribute("l")]
        public string LinesString
        {
            get => l.RawValue;
            set => l.RawValue = value;
        }

        [XmlIgnore]
        public IReadOnlyList<string> Lines => l.ConvertedValue;

        [XmlAttribute("lineref")]
        public string LineReferencesString { get; set; }

        [XmlIgnore]
        public IReadOnlyList<int> LineReferences => lineref.ConvertedValue;

#if DEBUG
        [XmlAnyAttribute]
        public XmlAttribute[] UnmatchedAttributes { get; set; }
#endif

        [XmlArray("zones")]
        [XmlArrayItem("zone")]
        public PointStageZone[] Zones { get; set; }

#if DEBUG
        [XmlAnyElement]
        public XmlElement[] UnmatchedElements { get; set; }
#endif

        private string DebuggerDisplay() =>
            $"{GetType()}, {Name}";
    }
}
