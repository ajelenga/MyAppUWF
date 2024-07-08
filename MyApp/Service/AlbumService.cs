using MyApp.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace MyApp.Service
{
    public class AlbumService
    {
        private const int MAX = 500;
        private const int MIN = 1;

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            var random = new Random();
            var uri = "https://jsonplaceholder.typicode.com/albums";
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(uri);
            var albums = JsonConvert.DeserializeObject<List<Album>>(response);

            foreach (var album in albums)
            {
                album.Cover = $"https://picsum.photos/200?{random.Next(MIN, MAX)}";
            }
            return albums;
        }
    }
}
