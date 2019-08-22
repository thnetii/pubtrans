using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using THNETII.Common;
using THNETII.TypeConverter.Xml;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class DepartureItem
    {
        private readonly DuplexConversionTuple<string, DateTime> d =
            TravelMagicUtils.GetDateTimeConversionTuple();
        private readonly DuplexConversionTuple<string, DateTime> a =
            TravelMagicUtils.GetDateTimeConversionTuple();
        private readonly DuplexConversionTuple<string, DateTime> d2 =
            TravelMagicUtils.GetDateTimeConversionTuple();
        private readonly DuplexConversionTuple<string, DateTime> a2 =
            TravelMagicUtils.GetDateTimeConversionTuple();
        private readonly DuplexConversionTuple<string, TripDirection> dir =
            new DuplexConversionTuple<string, TripDirection>(
                s => XmlEnumStringConverter.ParseOrDefault(s, TripDirection.Unknown),
                TravelMagicUtils.StringComparerCaseInsensitive,
                v => XmlEnumStringConverter.ToString(v)
            );



        [XmlAttribute("tp")]
        public string TransportTypeSymbolPathString { get; set; }

        [XmlAttribute("tn")]
        public string TransportTypeName { get; set; }

        [XmlAttribute("d")]
        public string ScheduledDepartureString
        {
            get => d.RawValue;
            set => d.RawValue = value;
        }

        [XmlIgnore]
        public DateTime ScheduledDeparture
        {
            get => d.ConvertedValue;
            set => d.ConvertedValue = value;
        }

        [XmlAttribute("a")]
        public string ScheduledArrivalString
        {
            get => a.RawValue;
            set => a.RawValue = value;
        }

        [XmlIgnore]
        public DateTime ScheduledArrival
        {
            get => a.ConvertedValue;
            set => a.ConvertedValue = value;
        }

        [XmlAttribute("d2")]
        public string EstimatedDepartureString
        {
            get => d2.RawValue;
            set => d2.RawValue = value;
        }

        [XmlIgnore]
        public DateTime EstimatedDeparture
        {
            get => d2.ConvertedValue;
            set => d2.ConvertedValue = value;
        }

        [XmlAttribute("a2")]
        public string EstimatedArrivalString
        {
            get => a2.RawValue;
            set => a2.RawValue = value;
        }

        [XmlIgnore]
        public DateTime EstimatedArrival
        {
            get => a2.ConvertedValue;
            set => a2.ConvertedValue = value;
        }

        [XmlAttribute("v")]
        public string PointStageId { get; set; }

        [XmlAttribute("hplnr")]
        public int StationNumber { get; set; }

        [XmlAttribute("stopnr")]
        public int PlatformNumber { get; set; }

        [XmlAttribute("l")]
        public string Line { get; set; }

        [XmlAttribute("nd")]
        public string DestinationName { get; set; }

        [XmlAttribute("c")]
        public string OperatorName { get; set; }

        [XmlAttribute("dir")]
        public string TripDirectionString
        {
            get => dir.RawValue;
            set => dir.RawValue = value;
        }

        [XmlIgnore]
        public TripDirection TripDirection
        {
            get => dir.ConvertedValue;
            set => dir.ConvertedValue = value;
        }

        //[XmlAttribute("ns")]
        //public string Ns { get; set; }

        [XmlAttribute("fnt")]
        public string Footnote { get; set; }

        [XmlAttribute("tid")]
        public string TripId { get; set; }

        [XmlAttribute("monitored")]
        public bool IsMonitored { get; set; }

        [XmlAttribute("updateid")]
        public string RealtimeId { get; set; }

        [XmlAttribute("lineref")]
        public int LineReference { get; set; }

        [XmlAttribute("vehiclejourneyref")]
        public string VehicleJourneyReference { get; set; }

#if DEBUG
        [XmlAnyAttribute]
        public XmlAttribute[] UnmatchedAttributes { get; set; }
#endif

        [XmlArray("notes")]
        [XmlArrayItem("i")]
        public NotesItem[] Notes { get; set; }

        [XmlArray("fromnotes")]
        [XmlArrayItem("i")]
        public NotesItem[] FromNotes { get; set; }

        [XmlIgnore]
        public IEnumerable<NotesItem> AllNotes =>
            (Notes ?? Array.Empty<NotesItem>()).Concat(FromNotes ?? Array.Empty<NotesItem>());

#if DEBUG
        [XmlAnyElement]
        public XmlElement[] UnmatchedElements { get; set; }
#endif

        private string DebuggerDisplay() => ToString();
    }
}
