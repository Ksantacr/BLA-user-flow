using BLA.UserFlow.Core.Entities;

namespace BLA.UserFlow.Core.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Posts>> GetAllPosts();
    Task<Posts?> GetPostById(int id);
    Task<Posts?> CreatePost(Posts model);
    Task<int> UpdatePost(int id, Posts model);
    Task<int> DeletePost(int id);
}