using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.FileProviders;

using Xunit;

namespace THNETII.PubTrans.TravelMagic.Model.Test
{
    public class DeparureSearchResultTest
    {
        private readonly IFileInfo xmlfile = Datafiles.DepartureSearch();

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(DepartureSearchResult));

            using (var dataStream = xmlfile.CreateReadStream())
            using (var xmlReader = XmlReader.Create(dataStream))
            {
                var result = (DepartureSearchResult)serializer.Deserialize(xmlReader);

                Assert.NotNull(result);
#if DEBUG
                Assert.All(result.Departures, dep =>
                {
                    Assert.Empty(dep.UnmatchedAttributes ?? Array.Empty<XmlAttribute>());
                    Assert.Empty(dep.UnmatchedElements ?? Array.Empty<XmlElement>());
                });
                Assert.All(result.Stages, st =>
                {
                    Assert.Empty(st.UnmatchedAttributes ?? Array.Empty<XmlAttribute>());
                    Assert.Empty(st.UnmatchedElements ?? Array.Empty<XmlElement>());
                });
#endif
            }
        }
    }
}
