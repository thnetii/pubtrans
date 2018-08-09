using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class FlightStatusTextTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetFlightStatuses();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetFlightStatuses();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(FlightStatusTextListing));
            FlightStatusTextListing datalisting;
            using (var dataStream = xmlfile.CreateReadStream())
            {
                datalisting =
                    (FlightStatusTextListing)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.FlightStatuses);
            Assert.NotEmpty(datalisting.FlightStatuses);
            Assert.DoesNotContain(datalisting.FlightStatuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(datalisting.FlightStatuses, AssertFlightStatusText);
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            FlightStatusText[] statuses;
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonfile.CreateReadStream())))
                statuses = serializer.Deserialize<FlightStatusText[]>(jsonReader);

            Assert.NotNull(statuses);
            Assert.NotEmpty(statuses);
            Assert.DoesNotContain(statuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(statuses, AssertFlightStatusText);
        }

        void AssertFlightStatusText(FlightStatusText status)
        {
            _ = status.Code;
        }
    }
}
