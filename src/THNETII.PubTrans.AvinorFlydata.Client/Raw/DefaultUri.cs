using System;

namespace THNETII.PubTrans.AvinorFlydata.Client.Raw
{
    public static class DefaultUri
    {
        public static Uri BaseAddress { get; } = new Uri(UrlConstant.BaseAddress);

        public static Uri AirlineNames { get; } = new Uri(BaseAddress, UrlConstant.AirlineNames);

        public static Uri AirportNames { get; } = new Uri(BaseAddress, UrlConstant.AirportNames);

        public static Uri FlightStatuses { get; } = new Uri(BaseAddress, UrlConstant.FlightStatuses);

        public static Uri GateStatuses { get; } = new Uri(BaseAddress, UrlConstant.GateStatuses);

        public static Uri BeltStatuses { get; } = new Uri(BaseAddress, UrlConstant.BeltStatuses);

        public static Uri AirportFeed { get; } = new Uri(BaseAddress, UrlConstant.AirportFeed);
    }
}
