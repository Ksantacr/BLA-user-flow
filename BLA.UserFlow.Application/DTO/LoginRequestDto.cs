using System.ComponentModel.DataAnnotations;

namespace BLA.UserFlow.Application.DTO;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}