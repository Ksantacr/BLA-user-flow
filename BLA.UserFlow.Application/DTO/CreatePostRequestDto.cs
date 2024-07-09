using System.ComponentModel.DataAnnotations;
using BLA.UserFlow.Core.Entities;

namespace BLA.UserFlow.Application.DTO;

public class CreatePostRequestDto
{
    [Required] public string Title { get; set; }
    public string? Description { get; set; }
    
    public static Posts DtoToPosts(CreatePostRequestDto post) => new()
        { Description = post.Description, Title = post.Title };
}

public class UpdatePostRequestDto : CreatePostRequestDto
{
}