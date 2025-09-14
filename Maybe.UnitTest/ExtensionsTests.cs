using Xunit;
using System.Threading.Tasks;

namespace Maybe.Tests.Exts;

#region --- Test Setup ---

public record User(int Id, string Name, bool IsActive);
public record Profile(string Email);

public class UserNotFoundError : NotFoundError
{
    public override string Code => "User.NotFound";
    public override string Message => "User not found.";
}
public class PermissionsError : AuthorizationError
{
    public override OutcomeType Type => OutcomeType.Forbidden;
    public override string Code => "Permissions.Denied";
    public override string Message => "Permission denied.";
}
public class CacheError : FailureError
{
    public override OutcomeType Type => OutcomeType.Failure;
    public override string Code => "Cache.Miss";
    public override string Message => "Could not find user in cache.";
}
public class ValidationError : Maybe.ValidationError
{
    public override OutcomeType Type => OutcomeType.Validation;
    public override string Code => "Name.Invalid";
    public override string Message => "User name is invalid";
}

public static class Api
{
    public static Maybe<User, UserNotFoundError> FindUserInDb(int id)
    {
        if (id == 1) return new User(1, "Alice", true);
        if (id == 2) return new User(2, "Bob", false);
        return new UserNotFoundError();
    }
    public static Maybe<User, CacheError> FindUserInCache(int id)
    {
        if (id == 1) return new User(1, "Alice", true);
        return new CacheError();
    }
    public static Maybe<string, PermissionsError> GetPermissions(User user)
    {
        if (user.Name.Equals("ALICE", System.StringComparison.OrdinalIgnoreCase) && user.IsActive) return "Admin";
        return new PermissionsError();
    }
    public static Task<Maybe<User, UserNotFoundError>> FindUserInDbAsync(int id) => Task.FromResult(FindUserInDb(id));
    public static Task<Maybe<string, ValidationError>> ValidateNameAsync(string name)
    {
        if (!string.IsNullOrWhiteSpace(name) && name.Equals("ALICE"))
        {
            return Task.FromResult((Maybe<string, ValidationError>)name);
        }
        return Task.FromResult((Maybe<string, ValidationError>)new ValidationError());
    }
}
#endregion


public class ExtensionTests
{
    [Theory]
    [InlineData(1, "Admin")]
    [InlineData(2, "Permissions.Denied")]
    [InlineData(3, "Permissions.Denied")]
    public void Then_Sync(int userId, string expectedCode)
    {
        var result = Api.FindUserInDb(userId).Then(Api.GetPermissions);

        var outcome = result.Match(
            onSome: value => value,
            onNone: error => error.Code
        );

        Assert.Equal(expectedCode, outcome);
    }

    [Theory]
    [InlineData(1, "Profile { Email = Alice@test.com }")]
    [InlineData(2, "Profile { Email = Bob@test.com }")]
    [InlineData(3, "User.NotFound")]
    public void Select_Sync(int userId, string expectedResult)
    {
        var result = Api.FindUserInDb(userId).Select(user => new Profile(user.Name + "@test.com"));

        var outcome = result.Match(
            onSome: value => value.ToString(),
            onNone: error => error.Code
        );

        Assert.Equal(expectedResult, outcome);
    }

    [Theory]
    [InlineData(1, "User { Id = 1, Name = Alice, IsActive = True }")]
    [InlineData(2, "User.Inactive")]
    [InlineData(3, "User.NotFound")]
    public void Ensure_Sync(int userId, string expectedResult)
    {
        var result = Api.FindUserInDb(userId).Ensure(user => user.IsActive, Error.Custom(OutcomeType.Validation, "User.Inactive", "User is not active"));

        string GetInnerErrorCode(BaseError error)
        {
            if(error.InnerError == null)
            {
                return error.Code;
            }
            return GetInnerErrorCode(error.InnerError);
        }

        var outcome = result.Match(
            onSome: value => value.ToString(),
            onNone: GetInnerErrorCode
        );

        Assert.Equal(expectedResult, outcome);
    }

    [Theory]
    [InlineData(1, "User { Id = 1, Name = Alice, IsActive = True }")]
    [InlineData(2, "User { Id = 2, Name = Bob, IsActive = False }")]
    [InlineData(3, "Cache.Miss")]
    public async Task RecoverAsync_FromSpecificError(int userId, string expectedResult)
    {
        var result = await Api.FindUserInDbAsync(userId).Recover(err => Api.FindUserInCache(userId));

        var outcome = result.Match(
            onSome: value => value.ToString(),
            onNone: error => error.Code
        );

        Assert.Equal(expectedResult, outcome);
    }

    [Theory]
    [InlineData(1, "Alice")]
    [InlineData(2, "Bob")]
    [InlineData(3, "DefaultUser")]
    public void Else_WithFallbackValue(int userId, string expectedResult)
    {
        var result = Api.FindUserInDb(userId).Select(u => u.Name).Else("DefaultUser");
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, "ALICE")]
    [InlineData(2, "FallbackPermission")]
    [InlineData(3, "FallbackPermission")]
    public async Task Full_Fluent_Flow(int userId, string expectedResult)
    {
        var finalResult = await Api.FindUserInDbAsync(userId)
            .Ensure(user => user.IsActive, Error.Forbidden("User.IsInactive"))
            .Select(user => user.Name.ToUpper())
            .ThenAsync(Api.ValidateNameAsync)
            .Select(validatedName => "AccessGranted:" + validatedName)
            .RecoverAsync(err => Task.FromResult((Maybe<string, ValidationError>)"FallbackPermission"));

        var outcome = finalResult.Match(
            onSome: value => value.Replace("AccessGranted:", ""), // Simplify assertion
            onNone: error => error.Code
        );

        Assert.Equal(expectedResult, outcome);
    }
}

