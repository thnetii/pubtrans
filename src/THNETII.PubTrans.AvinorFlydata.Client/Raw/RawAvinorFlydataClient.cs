using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using THNETII.Networking.Http;
using THNETII.PubTrans.AvinorFlydata.Model.Raw;
using System.Buffers;
using System.Text;
using THNETII.Common;

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

        private readonly HttpClient httpClient;

        [SuppressMessage(category: null, "CA2234", Justification = "HttpClient.BaseAddress is set on construction.")]
        private async Task<T> PerformHttpGetRequest<T>(string path, XmlSerializer serializer, CancellationToken cancelToken = default)
        {
            using (var responseMessage = await httpClient.GetAsync(path, cancelToken).ConfigureAwait(false))
            {
                _ = responseMessage.EnsureSuccessStatusCode();
                if (!responseMessage.Content.IsXml())
                    throw new SerializationException($"Returned media type '{responseMessage.Content.Headers.ContentType}' is not recognized as XML content.");
                using (var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    return (T)serializer.Deserialize(responseStream);
            }
        }

        private string ConstructPathWithQuery(string path, IReadOnlyList<(string key, string value)> parameters)
        {
            if (parameters.Count == 0)
                return path;
            var builder = new StringBuilder();
            builder.Append(path).Append('?');
            bool first = true;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].key.TryNotNullOrWhiteSpace(out var value))
                {
                    if (!first)
                        builder.Append('&');
                    else
                        first = false;
                    builder
                        .Append(value)
                        .Append('=')
                        .Append(Uri.EscapeDataString(value ?? string.Empty));
                }
            }
            return builder.ToString();
        }

        public async Task<IReadOnlyList<AirlineMetadata>> GetAirlines(CancellationToken ct = default)
        {
            var listing = await PerformHttpGetRequest<AirlineMetadataListing>(
                UrlConstant.AirlineNames, airlineSerializer, ct).ConfigureAwait(false);
            return listing?.Airlines ?? Array.Empty<AirlineMetadata>();
        }

        public async Task<AirlineMetadata> GetAirline(string iataCode, CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = ("airline", iataCode);
            var requestTask = PerformHttpGetRequest<AirlineMetadataListing>(
                ConstructPathWithQuery(UrlConstant.AirlineNames, queryParams),
                airlineSerializer, ct);
            queryParamsPool.Return(queryParams);
            var listing = await requestTask.ConfigureAwait(false);
            if ((listing?.Airlines?.Length ?? 0) < 1)
                return null;
            return listing.Airlines[0];
        }

        public async Task<IReadOnlyList<AirportMetadata>> GetAirports(bool? includeShortnames = true,
            CancellationToken ct = default)
        {
            var queryParams = queryParamsPool.Rent(1);
            queryParams[0] = includeShortnames.HasValue
                ? ("shortname", includeShortnames.Value ? "Y" : "N")
                : default;
            var req = PerformHttpGetRequest<AirportMetadataListing>(
                ConstructPathWithQuery(UrlConstant.AirportNames, queryParams),
                airportSerializer, ct);
            queryParamsPool.Return(queryParams);
            var listing = await req.ConfigureAwait(false);
            return listing?.Airports ?? Array.Empty<AirportMetadata>();
        }
    }
}
