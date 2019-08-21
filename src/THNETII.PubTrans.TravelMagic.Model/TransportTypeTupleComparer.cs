using System.Collections.Generic;

using THNETII.Common;

namespace THNETII.PubTrans.TravelMagic.Model
{
    public class TransportTypeTupleComparer : IEqualityComparer<DuplexConversionTuple<string, TransportType>>
    {
        public static TransportTypeTupleComparer Instance { get; } = new TransportTypeTupleComparer();

        public bool Equals(DuplexConversionTuple<string, TransportType> x, DuplexConversionTuple<string, TransportType> y) =>
            TravelMagicUtils.StringComparerCaseInsensitive.Equals(x?.RawValue, y?.RawValue);

        public int GetHashCode(DuplexConversionTuple<string, TransportType> tuple) =>
            TravelMagicUtils.StringComparerCaseInsensitive.GetHashCode(tuple?.RawValue);
    }
}
