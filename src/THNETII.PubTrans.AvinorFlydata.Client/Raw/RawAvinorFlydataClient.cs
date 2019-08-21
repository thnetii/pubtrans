using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using THNETII.Common;
using THNETII.Common.Collections.Generic;
using THNETII.TypeConverter.Xml;
using THNETII.Networking.Http;
using THNETII.PubTrans.AvinorFlydata.Model.Raw;

using StringKeyValuePair = System.Collections.Generic.KeyValuePair<string, string>;

namespace THNETII.PubTrans.AvinorFlydata.Client.Raw
{
    public class RawAvinorFlydataClient
    {
        private static readonly ArrayPool<StringKeyValuePair> queryParamsPool =
            ArrayPool<StringKeyValuePair>.Create();
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

        protected virtual XmlSerializer AirlineSerializer { get; } =
            airlineSerializer;
        protected virtual XmlSerializer AirportSerializer { get; } =
            airportSerializer;
        protected virtual XmlSerializer FlightStatusSerializer { get; } =
            flightStatusSerializer;
        protected virtual XmlSerializer GateStatusSerializer { get; } =
            gateStatusSerializer;
        protected virtual XmlSerializer BeltStatusSerializer { get; } =
            beltStatusSerializer;
        protected virtual XmlSerializer AirportFeedSerializer { get; } =
            airportFeedSerializer;

        private readonly HttpClient httpClient;

        private async Task<T> PerformHttpGetRequest<T>(Uri uri,
            XmlSerializer serializer, CancellationToken cancelToken = default)
        {
            var responseMessage = await httpClient.GetAsync(uri, cancelToken)
                .ConfigureAwait(false);
            using (responseMessage)
            {
                _ = responseMessage.EnsureSuccessStatusCode();
                if (!responseMessage.Content.IsXml())
                    throw new SerializationException($"Returned media type '{responseMessage.Content.Headers.ContentType}' is not recognized as XML content.");
                var responseStream = await responseMessage.Content
                    .ReadAsStreamAsync().ConfigureAwait(false);
                using (responseStream)
                using (var responseReader = XmlReader.Create(responseStream))
                    return (T)serializer.Deserialize(responseReader);
            }
        }

        protected static string ConstructQuery(
            IReadOnlyList<StringKeyValuePair> parameters)
        {
            if (parameters is null || parameters.Count == 0)
                return "?";
            var qBuilder = new StringBuilder();
            qBuilder.Append('?');
            bool first = true;
            for (int i = 0; i < parameters.Count; i++)
            {
                var param = parameters[i];
                if (param.Key.TryNotNullOrWhiteSpace(out var key))
                {
                    if (!first)
                        qBuilder.Append('&');
                    else
                        first = false;
                    qBuilder
                        .Append(key)
                        .Append('=')
                        .Append(Uri.EscapeDataString(param.Value ?? string.Empty));
                }
            }
            return qBuilder.ToString();
        }

        protected virtual Uri GetAirlineNamesUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.AirlineNames
                : new Uri(DefaultUri.AirlineNames, query);

        private async Task<IReadOnlyList<AirlineMetadata>> GetAirlineNames(
            Uri uri = default, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<AirlineMetadataListing>(
                uri ?? DefaultUri.AirlineNames, AirlineSerializer,
                ct).ConfigureAwait(false);
            return listing?.Airlines;
        }

        public Task<IReadOnlyList<AirlineMetadata>> GetAirlineNames(
            CancellationToken ct = default) =>
            GetAirlineNames(GetAirlineNamesUri(default), ct);

        public async Task<AirlineMetadata> GetAirlineName(string iataCode,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = ("airline", iataCode).AsKeyValuePair();
            var requestTask = GetAirlineNames(
                GetAirlineNamesUri(ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            var airlines = await requestTask.ConfigureAwait(false);
            return (airlines?.Count ?? 0) > 0 ? airlines[0] : null;
        }

        protected virtual Uri GetAirportNamesUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.AirportNames
                : new Uri(DefaultUri.AirportNames, query);

        private async Task<IReadOnlyList<AirportMetadata>> GetAirportNames(
            Uri uri = default, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<AirportMetadataListing>(
                uri ?? DefaultUri.AirportNames, AirportSerializer,
                ct).ConfigureAwait(false);
            return listing?.Airports;
        }

        public Task<IReadOnlyList<AirportMetadata>> GetAirportNames(
            bool? includeShortnames = true, CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = includeShortnames.HasValue
                ? ("shortname", includeShortnames.Value ? "Y" : "N").AsKeyValuePair()
                : default;
            var req = GetAirportNames(
                GetAirportNamesUri(ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            return req;
        }

        public async Task<AirportMetadata> GetAirportName(string iataCode,
            bool? includeShortnames = true, CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(2);
            queryParams[0] = ("airport", iataCode).AsKeyValuePair();
            queryParams[1] = includeShortnames.HasValue
                ? ("shortname", includeShortnames.Value ? "Y" : "N").AsKeyValuePair()
                : default;
            var req = GetAirportNames(
                GetAirportNamesUri(ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return (listing?.Count ?? 0) > 0 ? listing[0] : null;
        }

        protected virtual Uri GetFlightStatusTextsUri(string query) =>
            string.IsNullOrWhiteSpace(query)
                ? DefaultUri.FlightStatuses
                : new Uri(DefaultUri.FlightStatuses, query);

        private async Task<IReadOnlyList<FlightStatusText>> GetFlightStatusTexts(
            Uri uri = default, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<FlightStatusTextListing>(
                uri ?? DefaultUri.FlightStatuses, FlightStatusSerializer,
                ct).ConfigureAwait(false);
            return listing?.FlightStatuses;
        }

        public Task<IReadOnlyList<FlightStatusText>> GetFlightStatusTexts(
            CancellationToken ct = default) =>
            GetFlightStatusTexts(GetAirportNamesUri(default), ct);

        public Task<FlightStatusText> GetFlightStatusText(FlightStatusCode code,
            CancellationToken ct = default) =>
            GetFlightStatusText(XmlEnumStringConverter.ToString(code), ct);

        public async Task<FlightStatusText> GetFlightStatusText(string code,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = string.IsNullOrWhiteSpace(code)
                ? default : ("code", code).AsKeyValuePair();
            var req = GetFlightStatusTexts(
                GetFlightStatusTextsUri(ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return (listing?.Count ?? 0) > 0 ? listing[0] : null;
        }

        protected virtual Uri GetGateStatusTextsUri(string query) =>
            string.IsNullOrWhiteSpace(query) ? DefaultUri.GateStatuses
                : new Uri(DefaultUri.GateStatuses, query);

        private async Task<IReadOnlyList<GateStatusText>> GetGateStatusTexts(
            Uri uri = default, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<GateStatusTextListing>(
                uri ?? DefaultUri.GateStatuses, GateStatusSerializer,
                ct).ConfigureAwait(false);
            return listing?.GateStatuses;
        }

        public Task<IReadOnlyList<GateStatusText>> GetGateStatusTexts(
            CancellationToken ct = default) =>
            GetGateStatusTexts(uri: default, ct);

        public Task<GateStatusText> GetGateStatusText(GateStatusCode code,
            CancellationToken ct = default) =>
            GetGateStatusText(XmlEnumStringConverter.ToString(code), ct);

        public async Task<GateStatusText> GetGateStatusText(string code,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = string.IsNullOrWhiteSpace(code)
                ? default : ("code", code).AsKeyValuePair();
            var req = GetGateStatusTexts(
                GetGateStatusTextsUri(ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return (listing?.Count ?? 0) > 0 ? listing[0] : null;
        }

        protected virtual Uri GetBeltStatusTextsUri(string query) =>
            string.IsNullOrWhiteSpace(query) ? DefaultUri.BeltStatuses
                : new Uri(DefaultUri.BeltStatuses, query);

        private async Task<IReadOnlyList<BeltStatusText>> GetBeltStatusTexts(
            Uri uri = default, CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<BeltStatusTextListing>(
                uri ?? DefaultUri.BeltStatuses, BeltStatusSerializer,
                ct).ConfigureAwait(false);
            return listing?.BeltStatuses;
        }

        public Task<IReadOnlyList<BeltStatusText>> GetBeltStatusTexts(
            CancellationToken ct = default) =>
            GetBeltStatusTexts(uri: default, ct);

        public Task<BeltStatusText> GetBeltStatusText(BeltStatusCode code,
            CancellationToken ct = default) =>
            GetBeltStatusText(XmlEnumStringConverter.ToString(code), ct);

        public async Task<BeltStatusText> GetBeltStatusText(string code,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = string.IsNullOrWhiteSpace(code)
                ? default : ("code", code).AsKeyValuePair();
            var req = GetBeltStatusTexts(GetBeltStatusTextsUri(
                ConstructQuery(queryParams)), ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return (listing?.Count ?? 0) > 0 ? listing[0] : null;
        }
    }
}
