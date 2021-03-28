using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace QG.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [BindProperty]
        public UserModel UserModel { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UserModel != null)
            {
                var json = JsonSerializer.Serialize<UserModel>(UserModel, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    PropertyNameCaseInsensitive = true
                });

                var s = new MemoryStream(await GetDataInStream(json));
                Guid key = Guid.NewGuid();
                BlobContainerClient client = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=storageaccount323;AccountKey=b2GhAeaVITks7PhB/bsN6NZ0pJ//WyHBGmow79SAUCBk2VI1+YtO4NS/bgTVuUiLKMm5geopQsdI6WLtPKwQ8g==;EndpointSuffix=core.windows.net", "userform");
                await client.UploadBlobAsync($"user-{key}.json", s);
                return Redirect("/");
            }
            else
            {
                return Page();
            }
        }

        private async Task<byte[]> GetDataInStream(string data)
        {
            if (data != "")
            {
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms))
                {
                    await sw.WriteAsync(data);
                    await sw.FlushAsync();
                    return ms.ToArray();
                }
            }
            return null;
        }
    }
    public class UserModel
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string Email { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }
}
