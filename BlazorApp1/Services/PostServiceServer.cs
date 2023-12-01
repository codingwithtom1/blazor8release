using BlazorApp1.Client.Services;
using BlazorApp1.Data;
using BlazorApp1.Repository;
using BlazorApp1.Shared;
using Humanizer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class PostServiceServer : IPostService
    {
        private readonly IPostsRepository _posts;
        private readonly AuthenticationStateProvider _authstate;
        public PostServiceServer(IPostsRepository posts, AuthenticationStateProvider authstate)
        {
            _posts = posts;
            _authstate = authstate;
        }

        public async Task<List<PostDTO>> GetPostsAsync()
        {
            return await _posts.GetPostsAsync();
        }

        public bool OnClient()
        {
            return false;
        }

        public async Task<string> AddPostAsync(PostDTO post)
        {

            var auth = await _authstate.GetAuthenticationStateAsync();
            var claim = auth.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim != null)
            {
                string userid = claim.Value;
                return await _posts.AddPostAsync(post, userid);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            return await _posts.DeletePostAsync(id);
        }

        public async Task<bool> UpdatePostAsync(PostDTO post)
        {
            return await _posts.UpdatePostAsync(post);
        }
    }
}
