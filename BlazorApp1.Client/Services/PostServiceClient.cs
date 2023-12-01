using BlazorApp1.Shared;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BlazorApp1.Client.Services
{
    public class PostServiceClient : IPostService
    {
        private readonly HttpClient _httpClient;
        public PostServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostDTO>> GetPostsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PostDTO>>("api/post");
        }

        public bool OnClient()
        {
            return true;
        }

        public async Task<string> AddPostAsync(PostDTO post)
        {
            var result = await _httpClient.PostAsJsonAsync("api/post", post);
            
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Uri u = result.Headers.Location;
                string id = u.Segments.Last();
                return id;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdatePostAsync(PostDTO post)
        {
            var result = await _httpClient.PutAsJsonAsync("api/post/"+post.Id, post);
            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            var result = await _httpClient.DeleteAsync("api/post/" + id);
            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;

        }
    }
}
