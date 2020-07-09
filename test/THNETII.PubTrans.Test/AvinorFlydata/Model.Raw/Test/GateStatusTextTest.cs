using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;

using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class GateStatusTextTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetGateStatuses();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetGateStatuses();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(GateStatusTextListing));
            GateStatusTextListing datalisting;
            using (var dataStream = xmlfile.CreateReadStream())
            using (var xmlReader = XmlReader.Create(dataStream))
            {
                datalisting =
                    (GateStatusTextListing)serializer.Deserialize(xmlReader);
            }
            Assert.NotNull(datalisting);
            GateStatusText[] statuses = datalisting.GateStatuses;
            Assert.NotNull(statuses);
            Assert.NotEmpty(statuses);
            Assert.DoesNotContain(statuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(statuses, AssertGateStatusText);
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            GateStatusText[] statuses;
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonfile.CreateReadStream())))
                statuses = serializer.Deserialize<GateStatusText[]>(jsonReader);

            Assert.NotNull(statuses);
            Assert.NotEmpty(statuses);
            Assert.DoesNotContain(statuses, status => string.IsNullOrWhiteSpace(status?.CodeString));
            Assert.All(statuses, AssertGateStatusText);
        }

        void AssertGateStatusText(GateStatusText status)
        {
            _ = status.Code;
        }
    }
}
