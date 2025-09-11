namespace Maybe.Functional.Test;

// --- Test Setup: Simulation of a real-world scenario ---

public record User(int Id, string Name, bool IsActive);

public record TestError(OutcomeType Type, string Code, string Message) : IError;

public static class Errors
{
    public static readonly TestError UserNotFound = new(OutcomeType.NotFound, "User.NotFound", "The user was not found.");
    public static readonly TestError UserInactive = new(OutcomeType.Forbidden, "User.Inactive", "The user account is inactive.");
    public static readonly TestError InvalidInput = new(OutcomeType.Validation, "Input.Invalid", "The provided input is invalid.");
    public static readonly TestError PermissionDenied = new(OutcomeType.Forbidden, "Permissions.Denied", "Permission denied.");
}

public static class Api
{
    // --- Sync Methods ---
    public static Maybe<User, IError> FindUser(int id)
    {
        Console.WriteLine($"[SYNC] Finding user {id}...");
        if (id == 1) return new User(1, "Alice", true);
        if (id == 2) return new User(2, "Bob", false); // Inactive user
        return Errors.UserNotFound;
    }

    public static Maybe<string, IError> GetPermissions(User user)
    {
        Console.WriteLine($"[SYNC] Getting permissions for {user.Name}...");
        // Note: The original test returned "Admin" for "ALICE" and "Admin" for "Alice".
        // This makes the test succeed for the wrong reasons. Correcting the logic.
        if (user.Name.Equals("Alice", StringComparison.OrdinalIgnoreCase)) return "Admin";
        return Errors.PermissionDenied;
    }

    // --- Async Methods ---
    public static async Task<Maybe<User, IError>> FindUserAsync(int id)
    {
        Console.WriteLine($"[ASYNC] Finding user {id}...");
        await Task.Delay(50); // Simulate I/O
        return FindUser(id);
    }

    public static async Task<Maybe<string, IError>> GetPermissionsAsync(User user)
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

        var expectedChain = userId == 1 ? "Admin" : (userId == 2 ? "Error: User.Inactive" : "Error: User.NotFound");
        if (userId == 2) expectedChain = "Error: Permissions.Denied"; // Bob has no permissions

        Console.WriteLine("--- 1. Chain (Sync source -> Sync func) ---");
        var result1 = Api.FindUser(userId)
            .Chain(Api.GetPermissions);
        PrintResult(result1, userId == 1 ? "Admin" : "Error: User.NotFound");


        Console.WriteLine("\n--- 2. Chain (Async source -> Sync func) ---");
        var result2 = await Api.FindUserAsync(userId)
            .ChainAsync(Api.GetPermissions);
        PrintResult(result2, userId == 1 ? "Admin" : "Error: User.NotFound");


        Console.WriteLine("\n--- 3. Chain (Sync source -> Async func) ---");
        var result3 = await Api.FindUser(userId)
            .ChainAsync(Api.GetPermissionsAsync);
        PrintResult(result3, userId == 1 ? "Admin" : "Error: User.NotFound");

        Console.WriteLine("\n--- 4. Chain (Async source -> Async func) ---");
        var result4 = await Api.FindUserAsync(userId)
            .ChainAsync(Api.GetPermissionsAsync);
        PrintResult(result4, userId == 1 ? "Admin" : "Error: User.NotFound");


        Console.WriteLine("\n--- 5. Full Fluent Flow Highlighting Async -> Sync ---");
        string finalExpected;
        if (userId == 1) finalExpected = "ADMIN";
        else if (userId == 2) finalExpected = "Error: User.Inactive";
        else finalExpected = "Error: User.NotFound";

        var result5 = await Api.FindUserAsync(userId)                   // Start with Task<Maybe<User>>
            .EnsureAsync(user => user.IsActive, Errors.UserInactive)    // 5a. Async source -> Sync predicate
            .SelectAsync(user => user.Name.ToUpper())                   // 5b. Async source -> Sync func
            .ChainAsync(name => Api.GetPermissionsAsync(new User(userId, name, true))) // 5c. Async source -> Async func
            .SelectAsync(permissions => permissions.ToUpper());       // 5d. Async source -> Sync func
        PrintResult(result5, finalExpected);
    }

    private static void PrintResult<TValue>(Maybe<TValue, IError> result, string expected)
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

