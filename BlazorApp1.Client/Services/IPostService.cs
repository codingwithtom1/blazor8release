using BlazorApp1.Shared;

namespace BlazorApp1.Client.Services
{
    public interface IPostService
    {
        public bool OnClient();
        public Task<List<PostDTO>> GetPostsAsync();
        public Task<string> AddPostAsync(PostDTO post);
        public Task<bool> UpdatePostAsync(PostDTO post);
        public Task<bool> DeletePostAsync(string id);
    }
}
