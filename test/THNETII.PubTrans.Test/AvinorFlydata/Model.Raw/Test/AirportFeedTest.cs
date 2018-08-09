using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    public class AirportFeedTest
    {
        private readonly IFileInfo xmlfile = XmlDatafiles.GetXmlFeed();
        private readonly IFileInfo jsonfile = JsonDatafiles.GetXmlFeed();
        private readonly string xmlfileAirportIata;

        public AirportFeedTest()
        {
            xmlfileAirportIata = GetAirportIata();
        }

        private string GetAirportIata()
        {
            XmlDocument airportFeedDocument = new XmlDocument();
            using (var dataStream = xmlfile.CreateReadStream())
            {
                airportFeedDocument.Load(dataStream);
            }
            var iataNode = airportFeedDocument.SelectSingleNode("//airport/@name");
            return iataNode?.Value;
        }

        [Fact]
        public void CanDeserializeXml()
        {
            var serializer = new XmlSerializer(typeof(AirportFeed));
            AirportFeed feed;
            using (var dataStream = xmlfile.CreateReadStream())
            {
                feed = (AirportFeed)serializer.Deserialize(dataStream);
            }
            Assert.NotNull(feed);
            Assert.Equal(xmlfileAirportIata, feed.AirportIata);
            Assert.NotNull(feed.Content);
            Assert.NotNull(feed.Content.Flights);
            Assert.NotEmpty(feed.Content.Flights);
            var flightIds = new HashSet<uint>();
            Assert.All(feed.Content.Flights, f => Assert.True(flightIds.Add(f.UniqueId)));
            Assert.All(feed.Content.Flights, AssertAirportFeedFlight);
        }

        [Fact]
        public void CanDeserializeJson()
        {
            var serializer = new JsonSerializer();
            AirportFeed feed;
            using (var dataStream = jsonfile.CreateReadStream())
            using (var textReader = new StreamReader(dataStream))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                feed = serializer.Deserialize<AirportFeed>(jsonReader);
            }

            Assert.NotNull(feed);
            Assert.Equal(xmlfileAirportIata, feed.AirportIata);
            Assert.NotNull(feed.Content);
            Assert.NotNull(feed.Content.Flights);
            Assert.NotEmpty(feed.Content.Flights);
            var flightIds = new HashSet<uint>();
            Assert.All(feed.Content.Flights, f => Assert.True(flightIds.Add(f.UniqueId)));
            Assert.All(feed.Content.Flights, AssertAirportFeedFlight);
        }

        void AssertAirportFeedFlight(AirportFeedFlight f)
        {
            _ = f.BaggageClaimStart;
            _ = f.BaggageClaimStartEpoch;
            _ = f.BaggageClaimStatusCode;
            _ = f.BaggageClaimStop;
            _ = f.BaggageClaimStopEpoch;
            _ = f.CustomsType;
            _ = f.DateOfOperation;
            _ = f.DateOfOperationEpoch;
            _ = f.Delayed;
            _ = f.Direction;
            _ = f.GateStatusCode;
            _ = f.ScheduleTime;
            _ = f.ScheduleTimeEpoch;
            _ = f.Status;
            _ = f.Status?.Code;
            _ = f.Status?.Time;
            Assert.All(f.ViaAirports ?? Array.Empty<string>(),
                a => Assert.False(string.IsNullOrWhiteSpace(a)));
        }
    }
}
