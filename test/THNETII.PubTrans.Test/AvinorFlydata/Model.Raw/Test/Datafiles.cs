using Microsoft.Extensions.FileProviders;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw.Test
{
    internal static class XmlDatafiles
    {
        private static readonly IFileProvider fileProvider = new EmbeddedFileProvider(
            typeof(XmlDatafiles).Assembly, "THNETII.PubTrans.AvinorFlydata.Model.Raw.Data");

        public static IFileInfo GetAirlineNames() => fileProvider.GetFileInfo("AirlineNames.xml");
        public static IFileInfo GetAirportNames() => fileProvider.GetFileInfo("AirportNames.xml");
        public static IFileInfo GetFlightStatuses() => fileProvider.GetFileInfo("FlightStatuses.xml");
        public static IFileInfo GetGateStatuses() => fileProvider.GetFileInfo("GateStatuses.xml");
        public static IFileInfo GetBeltStatuses() => fileProvider.GetFileInfo("BeltStatuses.xml");
        public static IFileInfo GetXmlFeed() => fileProvider.GetFileInfo("XmlFeed.xml");
    }

    internal static class JsonDatafiles
    {
        private static readonly IFileProvider fileProvider = new EmbeddedFileProvider(
            typeof(JsonDatafiles).Assembly, "THNETII.PubTrans.AvinorFlydata.Model.Raw.Data");

        public static IFileInfo GetAirlineNames() => fileProvider.GetFileInfo("AirlineNames.json");
        public static IFileInfo GetAirportNames() => fileProvider.GetFileInfo("AirportNames.json");
        public static IFileInfo GetFlightStatuses() => fileProvider.GetFileInfo("FlightStatuses.json");
        public static IFileInfo GetGateStatuses() => fileProvider.GetFileInfo("GateStatuses.json");
        public static IFileInfo GetBeltStatuses() => fileProvider.GetFileInfo("BeltStatuses.json");
        public static IFileInfo GetXmlFeed() => fileProvider.GetFileInfo("XmlFeed.json");
    }
}
