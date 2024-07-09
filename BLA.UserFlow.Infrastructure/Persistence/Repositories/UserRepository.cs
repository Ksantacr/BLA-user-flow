using System.Data;
using System.Data.Common;
using BLA.UserFlow.Core.Entities;
using BLA.UserFlow.Core.Repositories;
using BLA.UserFlow.Infrastructure.DatabaseConnection;
using Npgsql;

namespace BLA.UserFlow.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionProvider _connection;

    public UserRepository(DbConnectionProvider connection)
    {
        _connection = connection;
    }

    public async Task<User?> RegisterUserAsync(User model)
    {
        var connection = _connection.GetConnection();
        const string insertQuery =
            "INSERT INTO users (firstname, lastname, email, password_hash) VALUES (@Firstname, @Lastname, @Email, @PasswordHash) RETURNING id;";

        var cmd = connection.CreateCommand();
        cmd.CommandText = insertQuery;
        AddParameters(cmd, model);

        await connection.OpenAsync();

        // Execute and retrieve the ID
        var insertedId = await cmd.ExecuteScalarAsync();
        await connection.CloseAsync();

        if (insertedId is not null)
        {
            return new User()
            {
                Id = (int)insertedId
            };
        }

        return null;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var connection = _connection.GetConnection();
        const string insertQuery = "SELECT * FROM users WHERE email = @email";

        var cmd = connection.CreateCommand();
        cmd.CommandText = insertQuery;

        var emailParameter = cmd.CreateParameter();
        emailParameter.ParameterName = "@email";
        emailParameter.Value = email;

        cmd.Parameters.Add(emailParameter);
        await connection.OpenAsync();

        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    FirstName = reader["firstname"] == DBNull.Value
                        ? null
                        : reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader["lastname"] == DBNull.Value
                        ? null
                        : reader.GetString(reader.GetOrdinal("lastname")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                };
            }
        }

        await connection.CloseAsync();
        return null;
    }

    private static void AddParameters(DbCommand cmd, User model)
    {
        var firstname = cmd.CreateParameter();
        firstname.ParameterName = "@Firstname";
        firstname.Value = model.FirstName;

        var lastname = cmd.CreateParameter();
        lastname.ParameterName = "@Lastname";
        lastname.Value = model.LastName;

        var email = cmd.CreateParameter();
        email.ParameterName = "@email";
        email.Value = model.Email;

        var password = cmd.CreateParameter();
        password.ParameterName = "@PasswordHash";
        password.Value = model.PasswordHash;

        cmd.Parameters.Add(firstname);
        cmd.Parameters.Add(lastname);
        cmd.Parameters.Add(email);
        cmd.Parameters.Add(password);
    }

    // private static void AddParameters(NpgsqlCommand command, User user)
    // {
    //     var parameters = command.Parameters;
    //
    //     parameters.AddWithValue("@Firstname", user.FirstName);
    //     parameters.AddWithValue("@Lastname", user.LastName);
    //     parameters.AddWithValue("@Email", user.Email);
    //     parameters.AddWithValue("@PasswordHash", user.PasswordHash);
    // }
}