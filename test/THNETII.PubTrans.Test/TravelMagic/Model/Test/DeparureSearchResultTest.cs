using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            }
        }

        [Fact]
        public void CanAccessAllPublicInstancePropertiesWithGetters()
        {
            var serializer = new XmlSerializer(typeof(DepartureSearchResult));

            using (var dataStream = xmlfile.CreateReadStream())
            using (var xmlReader = XmlReader.Create(dataStream))
            {
                var result = (DepartureSearchResult)serializer.Deserialize(xmlReader);

                Assert.NotNull(result);
                AssertItemProperties(result);
            }

            void AssertPublicGetProperties(object instance)
            {
                var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(pi => pi.CanRead);
                Assert.All(props, pi =>
                {
                    var pv = pi.GetValue(instance);
                    AssertItemProperties(pv);
                });
            }

            void AssertItemProperties(object item)
            {
                if (item is null)
                    return;
                if (item is IEnumerable<object> items && !(item is string))
                {
                    AssertEnumerableItemProperties(items);
                }
                else if (item.GetType().Assembly == typeof(TravelMagicUtils).Assembly)
                    AssertPublicGetProperties(item);
            }

            void AssertEnumerableItemProperties(IEnumerable<object> enumerable)
            {
                Assert.All(enumerable ?? Enumerable.Empty<object>(), item =>
                {
                    AssertItemProperties(item);
                });
            }
        }
    }
}
