using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;

using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class AirportMetadataTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetAirportNames();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetAirportNames();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(AirportMetadataListing));
            AirportMetadataListing datalisting;
            using (var dataStream = xmlfile.CreateReadStream())
            using (var xmlReader = XmlReader.Create(dataStream))
            {
                datalisting =
                    (AirportMetadataListing)serializer.Deserialize(xmlReader);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.Airports);
            Assert.NotEmpty(datalisting.Airports);
            Assert.DoesNotContain(datalisting.Airports, airport => string.IsNullOrWhiteSpace(airport?.IataCode));
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            AirportMetadata[] airports;
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonfile.CreateReadStream())))
                airports = serializer.Deserialize<AirportMetadata[]>(jsonReader);

            Assert.NotNull(airports);
            Assert.NotEmpty(airports);
            Assert.DoesNotContain(airports, airport => string.IsNullOrWhiteSpace(airport?.IataCode));
        }
    }
}
