using System;

using Xunit;

namespace THNETII.PubTrans.TravelMagic.Client.Test
{
    public class DepartureSearchRequestTest
    {
        [Fact]
        public void DefaultRequestQueryStringEqualsEmptyQueryString()
        {
            var req = default(DepartureSearchRequest);

            Assert.Equal("?from=", req.ToQueryString());
        }

        [Fact]
        public void RequestWithFromOnlyReturnsFromOnlyQueryString()
        {
            const string from = @"Test (Municipality)";
            var req = new DepartureSearchRequest(from);

            Assert.Equal($"?from={Uri.EscapeDataString(from)}", req.ToQueryString());
        }

        [Fact]
        public void FullRequestReturnsCorrectQueryString()
        {
            const string from = @"Test (Municipality)";
            DateTime dt = new DateTime(2019, 08, 23, 13, 45, 00, DateTimeKind.Local);
            var lines = new string[] { "42", "24" };

            var req = new DepartureSearchRequest
            {
                From = from,
                DateTime = dt,
                IncludeRealtime = true,
                Lines = lines
            };

            Assert.Equal($"?from={Uri.EscapeDataString(from)}&date=23.08.2019&time={Uri.EscapeDataString("13:45:00")}&realtime=1&linjer={Uri.EscapeDataString("42,24")}", req.ToQueryString());
        }
    }
}
