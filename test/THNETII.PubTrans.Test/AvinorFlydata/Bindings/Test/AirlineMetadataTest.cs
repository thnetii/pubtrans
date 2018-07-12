using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Bindings.Test
{
    public class AirlineMetadataTest
    {
        private readonly IFileInfo datafile = Datafiles.GetAirlineNames();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = AirlineMetadataListing.DefaultSerializer;
            AirlineMetadataListing datalisting;
            using (var dataStream = datafile.CreateReadStream())
            {
                datalisting =
                    (AirlineMetadataListing)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.Airlines);
            Assert.DoesNotContain(datalisting.Airlines, airline => string.IsNullOrWhiteSpace(airline?.IataCode));
        }
    }
}
