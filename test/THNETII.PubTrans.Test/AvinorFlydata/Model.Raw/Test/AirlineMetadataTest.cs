using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class AirlineMetadataTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetAirlineNames();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetAirlineNames();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(AirlineMetadataListing));
            AirlineMetadataListing datalisting;
            using (var dataStream = xmlfile.CreateReadStream())
            {
                datalisting =
                    (AirlineMetadataListing)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.Airlines);
            Assert.NotEmpty(datalisting.Airlines);
            Assert.DoesNotContain(datalisting.Airlines, airline => string.IsNullOrWhiteSpace(airline?.IataCode));
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            AirlineMetadata[] airlines;
            using (var dataStream = jsonfile.CreateReadStream())
            using (var textReader = new StreamReader(dataStream))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                airlines = serializer.Deserialize<AirlineMetadata[]>(jsonReader);
            }

            Assert.NotNull(airlines);
            Assert.NotEmpty(airlines);
            Assert.DoesNotContain(airlines, airline => string.IsNullOrWhiteSpace(airline?.IataCode));
        }
    }
}
