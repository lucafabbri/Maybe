using System;
using System.Threading.Tasks;

namespace Maybe.Tests;

// --- Test Setup: Simulation of a real-world scenario ---

public record User(int Id, string Name, bool IsActive);

public static class Api
{
    // --- Sync Methods ---
    public static Maybe<User> FindUser(int id)
    {
        Console.WriteLine($"[SYNC] Finding user {id}...");
        if (id == 1) return new User(1, "Alice", true);
        if (id == 2) return new User(2, "Bob", false); // Inactive user
        return Error.NotFound(code: "User.NotFound", message: "The user was not found.");
    }

    public static Maybe<string> GetPermissions(User user)
    {
        Console.WriteLine($"[SYNC] Getting permissions for {user.Name}...");
        if (user.Name.Equals("Alice", StringComparison.OrdinalIgnoreCase)) return "Admin";
        if (user.Name.Equals("ALICE", StringComparison.OrdinalIgnoreCase)) return "Admin";
        return Error.Forbidden(code: "Permissions.Denied", message: "Permission denied.");
    }

    // --- Async Methods ---
    public static async Task<Maybe<User>> FindUserAsync(int id)
    {
        Console.WriteLine($"[ASYNC] Finding user {id}...");
        await Task.Delay(50); // Simulate I/O
        return FindUser(id);
    }

    public static async Task<Maybe<string>> GetPermissionsAsync(User user)
    {
        Console.WriteLine($"[ASYNC] Getting permissions for {user.Name}...");
        await Task.Delay(50); // Simulate I/O
        return GetPermissions(user);
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("--- Testing Successful Path (User 1) ---\n");
        await TestAllScenarios(1);

        Console.WriteLine("\n\n--- Testing Failure Path (User 3 - Not Found) ---\n");
        await TestAllScenarios(3);

        Console.WriteLine("\n\n--- Testing Failure Path (User 2 - Inactive) ---\n");
        await TestAllScenarios(2);
    }

    private static async Task TestAllScenarios(int userId)
    {
        Console.WriteLine($"--- Testing for User ID: {userId} ---");
        var userInactiveError = Error.Forbidden("User.Inactive", "The user account is inactive.");

        Console.WriteLine("--- 1. Then (Sync source -> Sync func) ---");
        var result1 = Api.FindUser(userId)
            .Then(Api.GetPermissions);
        PrintResult(result1, userId == 1 ? "Admin" : (userId == 2 ? "Error: Permissions.Denied" : "Error: User.NotFound"));


        Console.WriteLine("\n--- 2. Then (Async source -> Sync func) ---");
        var result2 = await Api.FindUserAsync(userId)
            .Then(Api.GetPermissions);
        PrintResult(result2, userId == 1 ? "Admin" : (userId == 2 ? "Error: Permissions.Denied" : "Error: User.NotFound"));


        Console.WriteLine("\n--- 3. ThenAsync (Sync source -> Async func) ---");
        var result3 = await Api.FindUser(userId)
            .ThenAsync(Api.GetPermissionsAsync);
        PrintResult(result3, userId == 1 ? "Admin" : (userId == 2 ? "Error: Permissions.Denied" : "Error: User.NotFound"));

        Console.WriteLine("\n--- 4. ThenAsync (Async source -> Async func) ---");
        var result4 = await Api.FindUserAsync(userId)
            .ThenAsync(Api.GetPermissionsAsync);
        PrintResult(result4, userId == 1 ? "Admin" : (userId == 2 ? "Error: Permissions.Denied" : "Error: User.NotFound"));


        Console.WriteLine("\n--- 5. Full Fluent Flow Highlighting Intuitive Async Syntax ---");
        string finalExpected;
        if (userId == 1) finalExpected = "ADMIN";
        else if (userId == 2) finalExpected = "Error: User.Inactive";
        else finalExpected = "Error: User.NotFound";

        var result5 = await Api.FindUserAsync(userId)                   // Start with Task<Maybe<User>>
            .Ensure(user => user.IsActive, userInactiveError)           // Now uses .Ensure (sync func)
            .Select(user => user.Name.ToUpper())                        // Now uses .Select (sync func)
            .ThenAsync(async name => await Api.GetPermissionsAsync(new User(userId, name, true))) // Uses .ThenAsync (async func)
            .Select(permissions => permissions.ToUpper());            // Now uses .Select (sync func)
        PrintResult(result5, finalExpected);
    }

    private static void PrintResult<TValue, TError>(Maybe<TValue, TError> result, string expected)
        where TError : IError
    {
        var outcome = result.Match(
            onSome: value => value?.ToString() ?? "null",
            onNone: error => $"Error: {error.Code}"
        );

        Console.ForegroundColor = outcome == expected ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"--> Result: {outcome} | Expected: {expected}");
        Console.ResetColor();
    }

    private static void PrintResult<TValue>(Maybe<TValue> result, string expected)
    {
        var outcome = result.Match(
            onSome: value => value?.ToString() ?? "null",
            onNone: error => $"Error: {error.Code}"
        );

        Console.ForegroundColor = outcome == expected ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"--> Result: {outcome} | Expected: {expected}");
        Console.ResetColor();
    }
}

