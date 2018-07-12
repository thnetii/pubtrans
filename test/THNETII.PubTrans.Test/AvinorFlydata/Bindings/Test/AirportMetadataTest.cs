using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Bindings.Test
{
    public class AirportMetadataTest
    {
        private readonly IFileInfo datafile = Datafiles.GetAirportNames();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = AirportMetadataListing.DefaultSerializer;
            AirportMetadataListing datalisting;
            using (var dataStream = datafile.CreateReadStream())
            {
                datalisting =
                    (AirportMetadataListing)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.Airports);
            Assert.DoesNotContain(datalisting.Airports, airport => string.IsNullOrWhiteSpace(airport?.IataCode));
        }
    }
}
