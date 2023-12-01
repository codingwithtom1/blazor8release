using BlazorApp1.Data;
using BlazorApp1.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository
{
    public interface IPostsRepository
    {
        public Task<List<PostDTO>> GetPostsAsync();
        public Task<PostDTO> GetPostByIdAsync(string id);
        public Task<string> AddPostAsync(PostDTO post, string userid);
        public Task<bool> UpdatePostAsync(PostDTO post);
        public Task<bool> DeletePostAsync(string id);
    }

    public class PostsRepository : IPostsRepository
    {
        private readonly ApplicationDbContext _context;
        public PostsRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<PostDTO> GetPostByIdAsync(string id)
        {
            Post? post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
            if (post is not null)
            {
                PostDTO postDTO = new PostDTO();
                postDTO.Id = post.Id;
                postDTO.Title = post.Title;
                postDTO.Description = post.Description;
                postDTO.Author = post.User.Email!;
                postDTO.CreatedDate = post.CreatedDate;
                return postDTO;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<PostDTO>> GetPostsAsync()
        {
            List<PostDTO> results = new List<PostDTO>();

            List<Post> posts = await _context.Posts.Include("User").ToListAsync();
            foreach (var post in posts)
            {
                PostDTO postDTO = new PostDTO();
                postDTO.Id = post.Id;
                postDTO.Title = post.Title;
                postDTO.Description = post.Description;
                postDTO.Author = post.User.Email!;
                postDTO.CreatedDate = post.CreatedDate;
                results.Add(postDTO);
            }

            return results;
        }
        public async Task<string> AddPostAsync(PostDTO post, string userid)
        {
            Post p = new Post();
            p.Id = Guid.NewGuid().ToString();
            p.CreatedDate = DateTime.Now;
            p.Title = post.Title;
            p.Description = post.Description;
            p.UserId = userid;
            _context.Add(p);
            await _context.SaveChangesAsync();
            return p.Id;
        }
        public async Task<bool> UpdatePostAsync(PostDTO post)
        {
            var p = await _context.Posts.Where(p => p.Id == post.Id).FirstOrDefaultAsync(); ;
            if (post != null)
            {
                // update the modifiable fields
                p.Title = post.Title;
                p.Description = post.Description;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeletePostAsync(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
