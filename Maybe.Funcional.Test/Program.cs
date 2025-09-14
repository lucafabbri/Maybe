using System;
using System.Threading.Tasks;
using Maybe;

namespace Maybe.Tests;

#region --- Test Setup ---

public record User(int Id, string Name, bool IsActive);
public record Profile(string Email);

// CORRECTED: Added copy constructors to all error types for safe propagation
public class UserNotFoundError : NotFoundError
{
    public override OutcomeType Type => OutcomeType.NotFound;
    public override string Code => "User.NotFound";
    public override string Message => "The user was not found.";
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
	// --- Sync Methods ---
	public static Maybe<User, UserNotFoundError> FindUserInDb(int id)
	{
		Console.WriteLine($"[DB] Finding user {id}...");
		if (id == 1) return new User(1, "Alice", true);
		if (id == 2) return new User(2, "Bob", false);
		return new UserNotFoundError();
	}

	public static Maybe<User, CacheError> FindUserInCache(int id)
	{
		Console.WriteLine($"[CACHE] Finding user {id}...");
		if (id == 1) return new User(1, "Alice", true);
		return new CacheError();
	}

	public static Maybe<string, PermissionsError> GetPermissions(User user)
	{
		Console.WriteLine($"[API] Getting permissions for {user.Name}...");
		if (user.Name.Equals("ALICE", StringComparison.OrdinalIgnoreCase) && user.IsActive) return "Admin";
		return new PermissionsError();
	}

	// --- Async Methods ---
	public static Task<Maybe<User, UserNotFoundError>> FindUserInDbAsync(int id) => Task.FromResult(FindUserInDb(id));

	public static Task<Maybe<string, ValidationError>> ValidateNameAsync(string name)
	{
		Console.WriteLine($"[API] Validating name {name}...");
		if (!string.IsNullOrWhiteSpace(name) && name.Equals("ALICE"))
		{
			return Task.FromResult((Maybe<string, ValidationError>)name);
		}
		return Task.FromResult((Maybe<string, ValidationError>)new ValidationError());
	}
}
#endregion

public static class Program
{
	public static async Task Main()
    {

        var rootCause = Error.NotFound("User with ID 123 not found.", "Database.UserNotFound");
        // Attendo un secondo per rendere i timestamp diversi
        System.Threading.Thread.Sleep(1000);
        var serviceError = Error.Failure("Failed to retrieve user. While we tries to open the connection everything exploded.", "Service.GetUser", innerError: rootCause);
        System.Threading.Thread.Sleep(1000);
        var apiError = Error.Custom(OutcomeType.Unexpected, "Error processing request.", "Api.Controller", serviceError);

        // --- TEST DEI LAYOUT ---

        Console.WriteLine("--- Layout: Flat ---");
        Console.WriteLine(apiError.ToFullString());

        Console.WriteLine("--- Testing Successful Path (User 1) ---\n");
		await TestAllScenarios(1);

		Console.WriteLine("\n\n--- Testing Failure Path (User 2) ---\n");
		await TestAllScenarios(2);

		Console.WriteLine("\n\n--- Testing Failure Path (User 3) ---\n");
		await TestAllScenarios(3);
	}

	private static async Task TestAllScenarios(int userId)
	{
		Console.WriteLine($"---------- STARTING TESTS FOR USER ID: {userId} ----------");

		// --- Then Tests ---
		Console.WriteLine("\n--- 1. Then (Sync -> Sync) ---");
		var res1 = Api.FindUserInDb(userId).Then(Api.GetPermissions);
		PrintResult(res1, GetExpected(userId, "Admin", "Permissions.Denied", "User.NotFound"));

		// --- Select Tests ---
		Console.WriteLine("\n--- 2. Select (Sync) ---");
		var res2 = Api.FindUserInDb(userId).Select(user => new Profile(user.Name + "@test.com"));
		PrintResult(res2, GetExpected(userId, "Profile { Email = Alice@test.com }", "Profile { Email = Bob@test.com }", "User.NotFound"));

		// --- Ensure Tests ---
		Console.WriteLine("\n--- 3. Ensure (Sync) ---");
		var res3 = Api.FindUserInDb(userId).Ensure(user => user.IsActive, Error.Forbidden("User.Inactive"));
		PrintResult(res3, GetExpected(userId, "User { Id = 1, Name = Alice, IsActive = True }", "User.Inactive", "User.NotFound"));

		// --- Recover Tests ---
		Console.WriteLine("\n--- 4. RecoverAsync (from specific error) ---");
		var res4 = await Api.FindUserInDbAsync(userId).Recover(err => Api.FindUserInCache(userId));
		PrintResult(res4, GetExpected(userId, "User { Id = 1, Name = Alice, IsActive = True }", "User { Id = 2, Name = Bob, IsActive = False }", "Cache.Miss"));

		// --- Else/ElseAsync Tests ---
		Console.WriteLine("\n--- 5. Else (Sync Fallback) ---");
		var res5 = Api.FindUserInDb(userId).Select(u => u.Name).Else("DefaultUser");
		PrintOutcome(res5, GetExpected(userId, "Alice", "Bob", "DefaultUser"));

		// --- If/IfSome Tests ---
		Console.WriteLine("\n--- 6. IfSome (Sync Action) ---");
		Api.FindUserInDb(userId).IfSome(u => Console.WriteLine("   -> IfSome executed!"));

		// --- Full Fluent Flow ---
		Console.WriteLine("\n--- 7. Full Fluent Flow ---");
		var finalResult = await Api.FindUserInDbAsync(userId)
			.Ensure(user => user.IsActive, Error.Forbidden("User.IsInactive"))
			.Select(user => user.Name.ToUpper())
			.ThenAsync(Api.ValidateNameAsync)
			.Select(validatedName => "AccessGranted:" + validatedName)
			.RecoverAsync(err => Task.FromResult((Maybe<string, ValidationError>)"FallbackPermission"));
		PrintResult(finalResult, GetExpected(userId, "AccessGranted:ALICE", "User.Inactive", "FallbackPermission"));

		Console.WriteLine($"---------- FINISHED TESTS FOR USER ID: {userId} ----------\n");

    }

    private static string GetExpected(int userId, string success, string user2_fail, string user3_fail)
	{
		if (userId == 1) return success;
		if (userId == 2) return $"Error: {user2_fail}";
		return $"Error: {user3_fail}";
	}

	private static void PrintResult<TValue, TError>(Maybe<TValue, TError> result, string expected)
		where TError : BaseError, new()
	{
		var outcome = result.Match(
			onSome: value => value?.ToString() ?? "null",
			onNone: error => $"Error: {error.Code}"
		);
		PrintOutcome(outcome, expected);
	}

	private static void PrintOutcome(string outcome, string expected)
	{
		Console.ForegroundColor = outcome == expected ? ConsoleColor.Green : ConsoleColor.Red;
		Console.WriteLine($"--> Result: {outcome} | Expected: {expected}");
		Console.ResetColor();
	}
}

