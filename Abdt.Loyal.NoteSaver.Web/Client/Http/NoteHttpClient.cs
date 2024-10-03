using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Abdt.Loyal.NoteSaver.Web.Shared;

namespace Abdt.Loyal.NoteSaver.Web.Client.Http
{
    public class NoteHttpClient : INoteClient<Note>
    {
        private readonly HttpClient _httpClient;

        public NoteHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Note?> Add(Note note)
        {
            try
            {
                var uri = new Uri("add", UriKind.Relative);
                var content = new StringContent(JsonSerializer.Serialize(note), Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await _httpClient.PostAsync(uri, content);

                return await response.Content.ReadFromJsonAsync<Note>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Note?> Update(Note note)
        {
            try
            {
                var uri = new Uri("update", UriKind.Relative);
                var response = await _httpClient.PutAsJsonAsync(uri, note);

                return await response.Content.ReadFromJsonAsync<Note>();
            }
            catch
            {
                return null;
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                var relativeUri = new Uri("delete", UriKind.Relative);
                var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress, relativeUri));
                uriBuilder.Query = $"id={id}";

                await _httpClient.DeleteAsync(uriBuilder.Uri);
            }
            catch
            {
                return;
            }
        }

        public async Task<Note?> Get(long id)
        {
            try
            {
                var uri = new Uri($"{id}", UriKind.Relative);
                var response = await _httpClient.GetAsync(uri);

                return await response.Content.ReadFromJsonAsync<Note>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Page<Note>?> GetPage(ushort pageNumber, int itemsCount)
        {
            try
            {
                var relativeUri = new Uri("list", UriKind.Relative);
                var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress, relativeUri));
                uriBuilder.Query = $"pageNumber={pageNumber}&itemsCount={itemsCount}";

                var response = await _httpClient.GetAsync(uriBuilder.Uri);
                
                return await response.Content.ReadFromJsonAsync<Page<Note>>();
            }
            catch
            {
                return null;
            }
        }
    }
}
