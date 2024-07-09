using System.Data.Common;
using BLA.UserFlow.Core.Entities;
using BLA.UserFlow.Core.Repositories;
using BLA.UserFlow.Infrastructure.DatabaseConnection;
using Npgsql;

namespace BLA.UserFlow.Infrastructure.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DbConnectionProvider _connection;

    public PostRepository(DbConnectionProvider connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Posts>> GetAllPosts()
    {
        var posts = new List<Posts>();
        var connection = _connection.GetConnection();
        const string query = "SELECT * FROM posts";

        var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        await connection.OpenAsync();

        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var post = new Posts
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Description = reader["description"] == DBNull.Value
                    ? null
                    : reader.GetString(reader.GetOrdinal("description")),
                CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };

            posts.Add(post);
        }

        await connection.CloseAsync();
        return posts;
    }

    public async Task<Posts?> GetPostById(int id)
    {
        var connection = _connection.GetConnection();
        const string query = "SELECT * FROM posts WHERE id = @postId";

        var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        var idParam = cmd.CreateParameter();
        idParam.ParameterName = "@postId";
        idParam.Value = id;

        cmd.Parameters.Add(idParam);
        await connection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Posts
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Description = reader["description"] == DBNull.Value
                    ? null
                    : reader.GetString(reader.GetOrdinal("description")),
                CreatedBy = reader.GetInt32(reader.GetOrdinal("created_by")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };
        }

        await connection.CloseAsync();
        return null;
    }

    public async Task<Posts?> CreatePost(Posts model)
    {
        var connection = _connection.GetConnection();
        const string query = @"INSERT INTO posts (title, description, created_by, created_at)
                    VALUES (@title, @description, @created_by, CURRENT_TIMESTAMP)
                    RETURNING id, created_at";

        var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        CreateAddParameters(cmd, model);

        await connection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            model.Id = reader.GetInt32(reader.GetOrdinal("id"));
            model.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"));
            return model;
        }

        await connection.CloseAsync();
        return null;
    }

    public async Task<int> UpdatePost(int id, Posts model)
    {
        var connection = _connection.GetConnection();
        const string query = @"UPDATE posts
                    SET title = @title, description = @description
                    WHERE id = @id
                    RETURNING id";

        var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        model.Id = id;
        UpdateAddParameters(cmd, model);
        await connection.OpenAsync();
        
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return rowsAffected;
    }

    public async Task<int> DeletePost(int id)
    {
        var connection = _connection.GetConnection();
        const string query = "DELETE FROM posts WHERE id = @id";

        var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        DeleteAddParameters(cmd, id);
        await connection.OpenAsync();

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        return rowsAffected;
    }

    private static void CreateAddParameters(DbCommand cmd, Posts model)
    {
        var title = cmd.CreateParameter();
        title.ParameterName = "@title";
        title.Value = model.Title;

        var description = cmd.CreateParameter();
        description.ParameterName = "@description";
        description.Value = model.Description ?? (object)DBNull.Value;

        var created_by = cmd.CreateParameter();
        created_by.ParameterName = "@created_by";
        created_by.Value = model.CreatedBy;

        cmd.Parameters.Add(title);
        cmd.Parameters.Add(description);
        cmd.Parameters.Add(created_by);
    }

    private static void UpdateAddParameters(DbCommand cmd, Posts model)
    {
        var title = cmd.CreateParameter();
        title.ParameterName = "@title";
        title.Value = model.Title;

        var description = cmd.CreateParameter();
        description.ParameterName = "@description";
        description.Value = model.Description ?? (object)DBNull.Value;

        var idParameter = cmd.CreateParameter();
        idParameter.ParameterName = "@id";
        idParameter.Value = model.Id;

        cmd.Parameters.Add(title);
        cmd.Parameters.Add(description);
        cmd.Parameters.Add(idParameter);
    }

    private static void DeleteAddParameters(DbCommand cmd, int id)
    {
        var idParameter = cmd.CreateParameter();
        idParameter.ParameterName = "@id";
        idParameter.Value = id;
        cmd.Parameters.Add(idParameter);
    }
}