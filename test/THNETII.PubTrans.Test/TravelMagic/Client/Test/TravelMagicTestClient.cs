using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;

using THNETII.Networking.Http;

using Xunit;

namespace THNETII.PubTrans.TravelMagic.Client.Test
{
    public class TravelMagicTestClient
    {
        private TestServer CreateServer(PathString requestPath, RequestDelegate requestDelegate)
        {
            var builder = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .Configure(app =>
                {
                    app.Map(requestPath, subApp => subApp.Run(requestDelegate));
                });

            return new TestServer(builder);
        }

        [Fact]
        public void CanDeserializeSampleResponse()
        {
            using (var server = CreateServer("/" + TravelMagicPath.DepartureSearch, async httpCtx =>
            {
                httpCtx.Response.ContentType = HttpWellKnownMediaType.ApplicationXml;
                using (var dataStream = Datafiles.DepartureSearch().CreateReadStream())
                    await dataStream.CopyToAsync(httpCtx.Response.Body).ConfigureAwait(false);
            }))
            {
                var client = new TravelMagicClient(new Uri("http://example.com/"), server.CreateClient());

                var result = client
                    .GetDepartureSearch(new DepartureSearchRequest("Test"))
                    .ConfigureAwait(false).GetAwaiter().GetResult();

                Assert.NotNull(result);
            }
        }
    }
}
