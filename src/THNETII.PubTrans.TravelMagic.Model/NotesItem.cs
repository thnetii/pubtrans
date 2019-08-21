using System;
using System.Diagnostics;
using System.Xml.Serialization;

using THNETII.Common;
using THNETII.Common.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class NotesItem : IEquatable<NotesItem>, IEquatable<string>
    {
        private readonly DuplexConversionTuple<string, NoteSituationType> st =
            new DuplexConversionTuple<string, NoteSituationType>(
                s => XmlEnumStringConverter.ParseOrDefault(s, NoteSituationType.Unknown),
                TravelMagicUtils.StringComparerCaseInsensitive,
                v => XmlEnumStringConverter.ToString(v)
                );
        private readonly DuplexConversionTuple<string, NoteSeverity> sv =
            new DuplexConversionTuple<string, NoteSeverity>(
                s => XmlEnumStringConverter.ParseOrDefault(s, NoteSeverity.Unknown),
                TravelMagicUtils.StringComparerCaseInsensitive,
                v => XmlEnumStringConverter.ToString(v)
                );

        [XmlAttribute("d")]
        public string Message { get; set; }

        [XmlAttribute("st")]
        public string SituationString
        {
            get => st.RawValue;
            set => st.RawValue = value;
        }

        [XmlIgnore]
        public NoteSituationType Situation { get; set; }

        [XmlAttribute("sv")]
        public string SeverityString
        {
            get => sv.RawValue;
            set => sv.RawValue = value;
        }

        [XmlIgnore]
        public NoteSeverity Severity { get; set; }

        public override int GetHashCode() => Message?.GetHashCode() ?? base.GetHashCode();

        private string DebuggerDisplay() => $"{GetType()}, \"{Message}\" ({Situation}, {nameof(Severity)}: {Severity})";

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case NotesItem n: return Equals(n);
                case string s: return Equals(s);
                default:
                case null: return false;
            }
        }

        public bool Equals(string other) => TravelMagicUtils.StringComparerCaseInsensitive.Equals(Message, other);

        public bool Equals(NotesItem other) => Equals(other?.Message) &&
            TravelMagicUtils.StringComparerCaseInsensitive.Equals(SituationString, other?.Situation) &&
            TravelMagicUtils.StringComparerCaseInsensitive.Equals(SeverityString, other?.SeverityString);
    }
}
