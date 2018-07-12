using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace THNETII.PubTrans.AvinorFlydata.Bindings.Test
{
    internal static class Datafiles
    {
        private static readonly IFileProvider fileProvider = new EmbeddedFileProvider(
            typeof(Datafiles).Assembly, "THNETII.PubTrans.AvinorFlydata.Bindings.Data");

        public static IFileInfo GetAirlineNames() => fileProvider.GetFileInfo("AirlineNames.xml");
        public static IFileInfo GetAirportNames() => fileProvider.GetFileInfo("AirportNames.xml");
        public static IFileInfo GetFlightStatuses() => fileProvider.GetFileInfo("FlightStatuses.xml");
        public static IFileInfo GetGateStatuses() => fileProvider.GetFileInfo("GateStatuses.xml");
        public static IFileInfo GetXmlFeed() => fileProvider.GetFileInfo("XmlFeed.xml");
    }
}
