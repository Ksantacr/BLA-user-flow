using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.Services.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BLA.UserFlow.API.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        return Ok(await _postService.GetAllPosts());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById([Required] int id)
    {
        var post = await _postService.GetPostById(id);
        if (post.Value is null)
            return NotFound();
        return Ok(post.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDto model)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var emailClaim = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
            return Unauthorized();

        var result = await _postService.CreatePost(model, emailClaim);
        return Ok(result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost([Required] int id, [FromBody] UpdatePostRequestDto model)
    {
        var result = await _postService.UpdatePost(id, model);
        if (result.Succeeded)
        {
            return Ok(result.Value);    
        }
        return BadRequest();

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _postService.DeletePost(id);
        if(result.Succeeded)
            return Ok();

        return BadRequest();
    }
}