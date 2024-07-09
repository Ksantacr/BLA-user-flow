namespace BLA.UserFlow.Core.Entities;

public class Posts
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}