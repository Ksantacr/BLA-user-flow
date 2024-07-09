using System.Collections;
using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.OperationResult;
using BLA.UserFlow.Core.Repositories;

namespace BLA.UserFlow.Application.Services.Posts;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostService(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<PostDto>>> GetAllPosts()
    {
        var result = await _postRepository.GetAllPosts();
        return Result<IEnumerable<PostDto>>.Success(result.Select(PostDto.PostsToDto));
    }

    public async Task<Result<PostDto?>> GetPostById(int id)
    {
        var result = await _postRepository.GetPostById(id);
        if (result is null)
        {
            return Result<PostDto?>.Failure();
        }

        return Result<PostDto?>.Success(PostDto.PostsToDto(result));
    }

    public async Task<Result<PostDto?>> CreatePost(CreatePostRequestDto model, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null)
        {
            return Result<PostDto?>.Failure();
        }

        var posts = CreatePostRequestDto.DtoToPosts(model);
        posts.CreatedBy = user.Id;

        var result = await _postRepository.CreatePost(posts);
        if (result is null)
        {
            return Result<PostDto?>.Failure();
        }

        return Result<PostDto?>.Success(PostDto.PostsToDto(result));
    }

    public async Task<Result<PostDto?>> UpdatePost(int id, UpdatePostRequestDto model)
    {
        var postModel = CreatePostRequestDto.DtoToPosts(model);
        var result = await _postRepository.UpdatePost(id, postModel);
        if (result == 0)
        {
            return Result<PostDto?>.Failure();
        }

        return Result<PostDto?>.Success(PostDto.PostsToDto(postModel));
    }

    public async Task<Result<PostDto?>> DeletePost(int id)
    {
        var result = await _postRepository.DeletePost(id);
        if (result is 0)
        {
            return Result<PostDto?>.Failure();
        }

        return Result<PostDto?>.Success(new PostDto() { Id = id });
    }
}