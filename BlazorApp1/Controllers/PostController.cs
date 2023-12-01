using BlazorApp1.Data;
using BlazorApp1.Repository;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;



namespace BlazorApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : Controller
    {

        private readonly IPostsRepository _posts;
        
        public PostController(IPostsRepository posts)
        {
            _posts = posts;
        }

        [HttpPost]
        public async Task<ActionResult> Add(PostDTO dto)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim != null)
            {
                string userid = claim.Value;
                string postid = await _posts.AddPostAsync(dto, userid);
                return CreatedAtAction(nameof(GetPostById), new { id = postid }, dto);
            }
            else
            {
                return Unauthorized();
            }      
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            bool result =await _posts.DeletePostAsync(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        

        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> Get()
        {

            List<PostDTO> results = await _posts.GetPostsAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPostById(string id)
        {
            var result = await _posts.GetPostByIdAsync(id);
            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(string id, PostDTO postdto)
        {
            if (id != postdto.Id)
            {
                return BadRequest();
            }
            bool result = await _posts.UpdatePostAsync(postdto);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
