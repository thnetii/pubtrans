using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace THNETII.PubTrans.TravelMagic.Model
{
    [XmlType, XmlRoot("stages")]
    [DebuggerDisplay("{" + nameof(Items) + "}")]
    [SuppressMessage("Naming", "CA1710: Identifiers should have correct suffix", Justification = nameof(System.Xml.Serialization))]
    public class PointStageResult : IReadOnlyList<PointStageItem>
    {
        private PointStageItem[] items;

        [XmlElement("i")]
        public PointStageItem[] Items
        {
            get => items ?? Array.Empty<PointStageItem>();
            set => items = value ?? Array.Empty<PointStageItem>();
        }

        [XmlIgnore]
        public PointStageItem this[int index] => Items[index];

        [XmlIgnore]
        public int Count => Items.Length;

        public IEnumerator<PointStageItem> GetEnumerator() =>
            ((IEnumerable<PointStageItem>)Items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Items.GetEnumerator();
    }
}
