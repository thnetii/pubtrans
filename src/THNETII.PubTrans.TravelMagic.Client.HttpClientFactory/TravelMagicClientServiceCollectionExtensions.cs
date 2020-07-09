using System;

using Microsoft.Extensions.DependencyInjection;

namespace THNETII.PubTrans.TravelMagic.Client
{
    public static class TravelMagicClientServiceCollectionExtensions
    {
        public static IServiceCollection AddTravelMagicClient(this IServiceCollection services, Uri endpoint) =>
            AddTravelMagicClient(services, name: null, endpoint);

        public static IServiceCollection AddTravelMagicClient(this IServiceCollection services, string name, Uri endpoint)
        {
            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));
            services.AddHttpClient(name ?? endpoint.Host)
                .AddTypedClient(httpClient => new TravelMagicClient(endpoint, httpClient));

            return services;
        }
    }
}
