using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace MagazineManagment.Shared.MediaFormatter
{
    public class VndApiFormatter : JsonMediaTypeFormatter
    {
        public VndApiFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.api+json"));
        }
    }
}
