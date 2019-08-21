using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class DepartureItem
    {
        [XmlArray("notes")]
        [XmlArrayItem("i")]
        public NotesItem[] Notes { get; set; }

        [XmlArray("fromnotes")]
        [XmlArrayItem("i")]
        public NotesItem[] FromNotes { get; set; }

        [XmlIgnore]
        public IEnumerable<NotesItem> AllNotes =>
            (Notes ?? Array.Empty<NotesItem>()).Concat(FromNotes ?? Array.Empty<NotesItem>());

        private string DebuggerDisplay() => ToString();
    }
}
