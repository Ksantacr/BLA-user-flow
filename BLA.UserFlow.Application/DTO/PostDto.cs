using BLA.UserFlow.Core.Entities;

namespace BLA.UserFlow.Application.DTO;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public static PostDto PostsToDto(Posts post) => new()
        { Id = post.Id, Description = post.Description, Title = post.Title };
}