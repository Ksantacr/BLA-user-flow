using BLA.UserFlow.API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace BLA.UserFlow.Tests.UnitTests.API.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _usersController;
    public UsersControllerTests()
    {
        _usersController = new UsersController();
    }

    [Fact]
    [Trait("API", "Returns OK with user list")]
    public void Should_Return_OKObjectResult_GetUsers()
    {
        var result = _usersController.GetUsers();

        result.Should().BeOfType<OkObjectResult>();
    }
}