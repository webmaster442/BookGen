using System.Net;
using System.Threading.Tasks;

namespace BookGen.Editor.Services
{
    public class NHuspellServices
    {
        private const string BaseUrl = "https://github.com/LibreOffice/dictionaries/{0}/{0}.{1}";

        public async Task DownloadDictionary(string code, string target)
        {
            using (var client = new WebClient())
            {
                var aff = string.Format(BaseUrl, code, "aff");
                var dic = string.Format(BaseUrl, code, "dic");
                await client.DownloadFileTaskAsync(aff, target).ConfigureAwait(false);
                await client.DownloadFileTaskAsync(dic, target).ConfigureAwait(false);
            }
        }
    }
}
