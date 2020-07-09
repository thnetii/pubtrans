using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;

using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class BeltStatusTextTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetBeltStatuses();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetBeltStatuses();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(BeltStatusTextListing));
            BeltStatusTextListing datalisting;
            using (var dataStream = xmlfile.CreateReadStream())
            using (var xmlReader = XmlReader.Create(dataStream))
            {
                datalisting =
                    (BeltStatusTextListing)serializer.Deserialize(xmlReader);
            }
            Assert.NotNull(datalisting);
            Assert.NotNull(datalisting.BeltStatuses);
            Assert.NotEmpty(datalisting.BeltStatuses);
            Assert.DoesNotContain(datalisting.BeltStatuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(datalisting.BeltStatuses, AssertBeltStatusText);
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            BeltStatusText[] statuses;
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonfile.CreateReadStream())))
                statuses = serializer.Deserialize<BeltStatusText[]>(jsonReader);

            Assert.NotNull(statuses);
            Assert.NotEmpty(statuses);
            Assert.DoesNotContain(statuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(statuses, AssertBeltStatusText);
        }

        void AssertBeltStatusText(BeltStatusText status)
        {
            _ = status.Code;
        }
    }
}
