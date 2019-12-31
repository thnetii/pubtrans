using System;

namespace THNETII.PubTrans.TravelMagic.Client
{
    public static class TravelMagicEndpoint
    {
        /// <summary>
        /// The default path to the TravelMagic system.
        /// </summary>
        /// <remarks>
        /// Traling slash is important, so that paths in the
        /// <see cref="TravelMagicPath"/> class can be used as relative paths.
        /// </remarks>
        public const string DefaultPath = @"/scripts/TravelMagic/TravelMagicWE.dll/";

        /// <summary>
        /// Fram &#x2013; kollektivtransporten i Møre og Romsdal
        /// </summary>
        public static Uri FramMr { get; } = new Uri(@"https://reiseplanlegger.frammr.no" + DefaultPath);
        /// <summary>
        /// Kolumbus &#x2013; Rogaland fylkeskommune
        /// </summary>
        public static Uri Kolumbus { get; } = new Uri(@"https://reiseplanlegger.kolumbus.no" + DefaultPath);
        /// <summary>
        /// Skyss &#x2013; Hordaland fylkeskommune
        /// </summary>
        public static Uri Skyss { get; } = new Uri(@"https://reiseplanlegger.skyss.no" + DefaultPath);
        /// <summary>
        /// Vestfold kollektivtrafikk A/S
        /// </summary>
        public static Uri Vkt { get; } = new Uri(@"https://reiseplanlegger.vkt.no" + DefaultPath);
        /// <summary>
        /// Snelandia &#x2013; Finnmark fylkeskommune
        /// </summary>
        public static Uri Ffk { get; } = new Uri(@"https://rp.177.ffk.no/scripts" + DefaultPath);
        /// <summary>
        /// 117 Nordland &#x2013; Nordland fylkeskommune
        /// </summary>
        public static Uri Nordland { get; } = new Uri(@"https://rp.177nordland.no" + DefaultPath);
        /// <summary>
        /// Agder Kollektivtrafikk AS (AKT)
        /// </summary>
        public static Uri Akt { get; } = new Uri(@"https://rp.akt.no" + DefaultPath);
        /// <summary>
        /// AtB &#x2013; kollektivtrafikken i Trøndelag
        /// </summary>
        public static Uri AtB { get; } = new Uri(@"https://rp.atb.no" + DefaultPath);
        /// <summary>
        /// Troms fylkestrafikk
        /// </summary>
        public static Uri Tromskortet { get; } = new Uri(@"https://rp.tromskortet.no" + DefaultPath);
    }
}
