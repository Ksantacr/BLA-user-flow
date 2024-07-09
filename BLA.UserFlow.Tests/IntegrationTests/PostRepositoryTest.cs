using BLA.UserFlow.Core.Entities;
using BLA.UserFlow.Core.Repositories;
using BLA.UserFlow.Infrastructure.DatabaseConnection;
using BLA.UserFlow.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Testcontainers.PostgreSql;

namespace BLA.UserFlow.Tests.IntegrationTests;

public sealed class PostRepositoryTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16.3")
        .Build();

    private IPostRepository _postRepository;

    public Task InitializeAsync()
    {
        return _postgres.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }

    private void InitDatabase()
    {
        var db = new DbConnectionProvider(_postgres.GetConnectionString());
        var connection = db.GetConnection();
        var command = connection.CreateCommand();

        command.CommandText =
            "CREATE TABLE posts\n(\n    id          SERIAL PRIMARY KEY,\n    title       VARCHAR(255) NOT NULL,\n    description TEXT NULL,\n    created_by   INT4         NOT NULL,\n    created_at   TIMESTAMP    NOT NULL\n);";

        connection.Open();
        command.ExecuteReader();
        connection.Close();
    }

    [Fact]
    [Trait("Database", "List all post")]
    public async Task? Should_List_AllPosts()
    {
        InitDatabase();
        _postRepository = new PostRepository(new DbConnectionProvider(_postgres.GetConnectionString()));

        var postOne = await _postRepository.CreatePost(new Posts()
        {
            Title = "Title",
            Description = "Description",
            CreatedBy = 1
        });

        var postTwo = await _postRepository.CreatePost(new Posts()
        {
            Title = "Title 2",
            Description = "Description 2",
            CreatedBy = 2
        });

        var listResult = await _postRepository.GetAllPosts();
        var postsEnumerable = listResult.ToList();
        postsEnumerable.Should().NotBeNull();
        postsEnumerable.ToList().Should().HaveCount(2);
    }

    [Fact]
    [Trait("Database", "Create one post and retrieve information")]
    public async Task Should_CreateOnePost_Then_GetInformation()
    {
        InitDatabase();

        _postRepository = new PostRepository(new DbConnectionProvider(_postgres.GetConnectionString()));

        await _postRepository.CreatePost(new Posts()
        {
            Title = "Title",
            Description = "Description",
            CreatedBy = 1
        });

        var post = await _postRepository.GetPostById(1);

        post.Should().NotBeNull();
        post.Id.Should().Be(1);
        post.Description.Should().Be("Description");
        post.Title.Should().Be("Title");
        post.CreatedBy.Should().Be(1);
    }

    [Fact]
    [Trait("Database", "Create, update one delete one post")]
    public async Task? Should_Test_CRUD_Operations()
    {
        InitDatabase();
        _postRepository = new PostRepository(new DbConnectionProvider(_postgres.GetConnectionString()));

        var postOne = await _postRepository.CreatePost(new Posts()
        {
            Title = "Title",
            Description = "Description",
            CreatedBy = 1
        });

        var listResult = await _postRepository.GetAllPosts();

        var updatePost = await _postRepository.UpdatePost(postOne.Id, new Posts()
        {
            Title = "My new Title",
            Description = null
        });

        var postAfterUpdating = await _postRepository.GetPostById(postOne.Id);

        var deletePost = await _postRepository.DeletePost(postOne.Id);

        var listResultAfterDeletion = await _postRepository.GetAllPosts();

        var postsEnumerable = listResult.ToList();

        postOne.Id.Should().Be(1);
        postsEnumerable.Should().NotBeNull();
        postsEnumerable.ToList().Should().HaveCount(1);
        postAfterUpdating.Title.Should().Be("My new Title");
        postAfterUpdating.Description.Should().BeNull();
        deletePost.Should().Be(1);
        listResultAfterDeletion.Should().HaveCount(0);
    }
}