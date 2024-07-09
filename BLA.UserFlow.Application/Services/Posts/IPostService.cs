using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.OperationResult;

namespace BLA.UserFlow.Application.Services.Posts;

public interface IPostService
{
    Task<Result<IEnumerable<PostDto>>> GetAllPosts();
    Task<Result<PostDto?>> GetPostById(int id);
    Task<Result<PostDto?>> CreatePost(CreatePostRequestDto model, string email);
    Task<Result<PostDto?>> UpdatePost(int id, UpdatePostRequestDto model);
    Task<Result<PostDto?>> DeletePost(int id);
    
}