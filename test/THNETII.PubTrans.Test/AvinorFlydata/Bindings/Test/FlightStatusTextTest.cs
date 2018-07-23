using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Bindings.Test
{
    public class FlightStatusTextTest
    {
        private readonly IFileInfo datafile = Datafiles.GetFlightStatuses();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = FlightStatusTextListing.DefaultSerializer;
            FlightStatusTextListing datalisting;
            using (var dataStream = datafile.CreateReadStream())
            {
                datalisting =
                    (FlightStatusTextListing)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.FlightStatuses);
            Assert.DoesNotContain(datalisting.FlightStatuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
        }
    }
}
