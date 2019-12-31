using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using THNETII.Networking.Http;
using THNETII.PubTrans.TravelMagic.Model;

using StringKeyValuePair = System.Collections.Generic.KeyValuePair<string, string>;

namespace THNETII.PubTrans.TravelMagic.Client
{
    public class TravelMagicClient
    {
        private static readonly MediaTypeWithQualityHeaderValue xmlTextMediaType =
            new MediaTypeWithQualityHeaderValue(HttpWellKnownMediaType.TextXml);
        private static readonly MediaTypeWithQualityHeaderValue xmlAppMediaType =
            new MediaTypeWithQualityHeaderValue(HttpWellKnownMediaType.ApplicationXml);

        private static readonly XmlSerializer departureSearchResultSerializer =
            new XmlSerializer(typeof(DepartureSearchResult));

        private readonly Uri travelMagicUri;
        private readonly HttpClient httpClient;

        protected virtual XmlSerializer DepartureSearchResultSerializer =>
            departureSearchResultSerializer;

        public TravelMagicClient(Uri travelMagicUri, HttpClient httpClient)
        {
            this.travelMagicUri = travelMagicUri ?? throw new ArgumentNullException(nameof(travelMagicUri));
            this.httpClient = httpClient ?? new HttpClient();
        }

        protected virtual async Task<T> GetAndDeserialize<T>(Uri requestUri,
            XmlSerializer xmlSerializer, CancellationToken cancelToken = default)
        {
            if (xmlSerializer is null)
                throw new ArgumentNullException(nameof(xmlSerializer));

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequest.Headers.Accept.Add(xmlTextMediaType);
            httpRequest.Headers.Accept.Add(xmlAppMediaType);

            using (httpRequest)
            using (var httpResponse =
                (
                await httpClient.SendAsync(httpRequest, cancelToken)
                .ConfigureAwait(false)
                )
                .EnsureSuccessStatusCode()
                )
            using (var xmlReader = XmlReader.Create(
                await httpResponse.Content
                .ReadAsStreamAsync()
                .ConfigureAwait(false)
                ))
            {
                return (T)xmlSerializer.Deserialize(xmlReader);
            }
        }

        protected virtual Task<DepartureSearchResult> GetDepartureSearch(
            Uri requestUri, CancellationToken cancelToken = default)
        {
            return GetAndDeserialize<DepartureSearchResult>(requestUri,
                DepartureSearchResultSerializer, cancelToken);
        }

        protected Task<DepartureSearchResult> GetDepartureSearch(
            string queryString, CancellationToken cancelToken = default)
        {
            var requestUri = new Uri(
                travelMagicUri,
                TravelMagicPath.DepartureSearch + queryString
                );
            return GetDepartureSearch(requestUri, cancelToken);
        }

        protected Task<DepartureSearchResult> GetDepartureSearch(
            IEnumerable<StringKeyValuePair> queryParams,
            CancellationToken cancelToken = default) =>
            GetDepartureSearch(HttpUrlHelper.ToQueryString(queryParams), cancelToken);

        public Task<DepartureSearchResult> GetDepartureSearch(
            in DepartureSearchRequest request,
            CancellationToken cancelToken = default) =>
            GetDepartureSearch(request.ToQueryString(), cancelToken);
    }
}
