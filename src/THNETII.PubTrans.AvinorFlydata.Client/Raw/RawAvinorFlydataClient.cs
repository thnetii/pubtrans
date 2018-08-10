using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using THNETII.Common;
using THNETII.Common.Collections.Generic;
using THNETII.Common.XmlSerializer;
using THNETII.Networking.Http;
using THNETII.PubTrans.AvinorFlydata.Model.Raw;

namespace THNETII.PubTrans.AvinorFlydata.Client.Raw
{
    public class RawAvinorFlydataClient
    {
        private static readonly ArrayPool<(string key, string value)> queryParamsPool =
            ArrayPool<(string key, string value)>.Create();
        private static readonly XmlSerializer airlineSerializer =
            new XmlSerializer(typeof(AirlineMetadataListing));
        private static readonly XmlSerializer airportSerializer =
            new XmlSerializer(typeof(AirportMetadataListing));
        private static readonly XmlSerializer flightStatusSerializer =
            new XmlSerializer(typeof(FlightStatusTextListing));
        private static readonly XmlSerializer gateStatusSerializer =
            new XmlSerializer(typeof(GateStatusTextListing));
        private static readonly XmlSerializer beltStatusSerializer =
            new XmlSerializer(typeof(BeltStatusTextListing));
        private static readonly XmlSerializer airportFeedSerializer =
            new XmlSerializer(typeof(AirportFeed));

        protected virtual XmlSerializer AirlineSerializer { get; } = airlineSerializer;
        protected virtual XmlSerializer AirportSerializer { get; } = airportSerializer;
        protected virtual XmlSerializer FlightStatusSerializer { get; } = flightStatusSerializer;
        protected virtual XmlSerializer GateStatusSerializer { get; } = gateStatusSerializer;
        protected virtual XmlSerializer BeltStatusSerializer { get; } = beltStatusSerializer;
        protected virtual XmlSerializer AirportFeedSerializer { get; } = airportFeedSerializer;

        private readonly HttpClient httpClient;

        private async Task<T> PerformHttpGetRequest<T>(Uri uri, XmlSerializer serializer, CancellationToken cancelToken = default)
        {
            using (var responseMessage = await httpClient.GetAsync(uri, cancelToken).ConfigureAwait(false))
            {
                _ = responseMessage.EnsureSuccessStatusCode();
                if (!responseMessage.Content.IsXml())
                    throw new SerializationException($"Returned media type '{responseMessage.Content.Headers.ContentType}' is not recognized as XML content.");
                using (var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    return (T)serializer.Deserialize(responseStream);
            }
        }

        protected string ConstructQuery(IReadOnlyList<(string key, string value)> parameters)
        {
            if ((parameters?.Count ?? 0 )== 0)
                return "?";
            var qBuilder = new StringBuilder();
            qBuilder.Append('?');
            bool first = true;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].key.TryNotNullOrWhiteSpace(out var value))
                {
                    if (!first)
                        qBuilder.Append('&');
                    else
                        first = false;
                    qBuilder
                        .Append(value)
                        .Append('=')
                        .Append(Uri.EscapeDataString(value ?? string.Empty));
                }
            }
            return qBuilder.ToString();
        }

        protected virtual Uri GetAirlineNamesUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.AirlineNames
                : new Uri(DefaultUri.AirlineNames, query);

        private async Task<IReadOnlyList<AirlineMetadata>> GetAirlineNames(
            Uri uri, XmlSerializer serializer, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<AirlineMetadataListing>(
                uri ?? DefaultUri.AirlineNames, serializer ?? AirlineSerializer,
                ct).ConfigureAwait(false);
            return listing?.Airlines;
        }

        public Task<IReadOnlyList<AirlineMetadata>> GetAirlineNames(CancellationToken ct = default) =>
            GetAirlineNames(uri: default, default, ct);

        public async Task<AirlineMetadata> GetAirlineName(string iataCode, CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = ("airline", iataCode);
            var requestTask = GetAirlineNames(
                GetAirlineNamesUri(ConstructQuery(queryParams)),
                default, ct);
            queryParamsPool.Return(queryParams);
            var airlines = await requestTask.ConfigureAwait(false);
            return airlines?.FirstIndexedOrDefault();
        }

        protected virtual Uri GetAirportNamesUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.AirportNames
                : new Uri(DefaultUri.AirportNames, query);

        private async Task<IReadOnlyList<AirportMetadata>> GetAirportNames(
            Uri uri, XmlSerializer serializer, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<AirportMetadataListing>(
                uri ?? DefaultUri.AirportNames, serializer ?? AirportSerializer,
                ct).ConfigureAwait(false);
            return listing?.Airports;
        }

        public Task<IReadOnlyList<AirportMetadata>> GetAirportNames(bool? includeShortnames = true,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = includeShortnames.HasValue
                ? ("shortname", includeShortnames.Value ? "Y" : "N")
                : default;
            var req = GetAirportNames(GetAirportNamesUri(ConstructQuery(queryParams)),
                default, ct);
            queryParamsPool.Return(queryParams);
            return req;
        }

        public async Task<AirportMetadata> GetAirportName(string iataCode,
            bool? includeShortnames = true, CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(2);
            queryParams[0] = ("airport", iataCode);
            queryParams[1] = includeShortnames.HasValue
                ? ("shortname", includeShortnames.Value ? "Y" : "N")
                : default;
            var req = GetAirportNames(GetAirportNamesUri(ConstructQuery(queryParams)),
                default, ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return listing?.FirstIndexedOrDefault();
        }

        protected virtual Uri GetFlightStatusTextsUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.FlightStatuses
                : new Uri(DefaultUri.FlightStatuses, query);

        private async Task<IReadOnlyList<FlightStatusText>> GetFlightStatusTexts(
            Uri uri, XmlSerializer serializer, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<FlightStatusTextListing>(
                uri ?? DefaultUri.FlightStatuses, serializer ?? FlightStatusSerializer,
                ct).ConfigureAwait(false);
            return listing?.FlightStatuses;
        }

        public Task<IReadOnlyList<FlightStatusText>> GetFlightStatusTexts(
            CancellationToken ct = default) =>
            GetFlightStatusTexts(uri: default, default, ct);

        public async Task<FlightStatusText> GetFlightStatusText(FlightStatusCode code,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = code != FlightStatusCode.Unspecified
                ? ("code", XmlEnumStringConverter<FlightStatusCode>.ToString(code))
                : default;
            var req = GetFlightStatusTexts(uri: default, default, ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return listing?.FirstIndexed();
        }
    }
}
