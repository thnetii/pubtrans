using System;
using System.Collections.Generic;
using System.Text;

namespace THNETII.PubTrans.AvinorFlydata.Client.Raw
{
    public static class UrlConstant
    {
        public const string DefaultBaseAddressString = @"https://flydata.avinor.no";
        public static Uri DefaultBaseUri { get; } = new Uri(DefaultBaseAddressString);

        public const string AirlineNames = @"/AirlineNames.asp";

        public const string AirportNames = @"/AirportNames.asp";
    }
}
