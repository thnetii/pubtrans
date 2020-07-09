using Microsoft.Extensions.FileProviders;

namespace THNETII.PubTrans.TravelMagic.Client
{
    internal static class Datafiles
    {
        private static readonly IFileProvider fileProvider = new EmbeddedFileProvider(
            typeof(Datafiles).Assembly, $"{typeof(Datafiles).Namespace}.Data");

        public static IFileInfo DepartureSearch() => fileProvider.GetFileInfo($"{nameof(DepartureSearch)}.xml");
    }
}
