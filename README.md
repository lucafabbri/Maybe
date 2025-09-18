<div align="center">

<img src="https://raw.githubusercontent.com/lucafabbri/maybe/main/maybe_logo.png" alt="drawing" width="350"/></br>

# Maybe

[![NuGet](https://img.shields.io/nuget/v/FluentCoder.Maybe.svg)](https://www.nuget.org/packages/FluentCoder.Maybe)
[![Build](https://github.com/lucafabbri/maybe/actions/workflows/main.yml/badge.svg)](https://github.com/lucafabbri/maybe/actions/workflows/main.yml) 
[![codecov](https://codecov.io/gh/lucafabbri/maybe/branch/main/graph/badge.svg)](https://codecov.io/gh/lucafabbri/maybe)

[![GitHub Stars](https://img.shields.io/github/stars/lucafabbri/maybe.svg)](https://github.com/lucafabbri/maybe/stargazers) 
[![GitHub license](https://img.shields.io/github/license/lucafabbri/maybe)](https://github.com/lucafabbri/maybe/blob/main/LICENSE)

---

### An elegant, fluent, and intuitive way to handle operations that may succeed or fail.

`dotnet add package FluentCoder.Maybe`

</div>

* [Give it a star ⭐!](https://www.google.com/search?q=%23give-it-a-star-)

* [Philosophy: Beyond Error Handling](https://www.google.com/search?q=%23philosophy-beyond-error-handling)

* [Core Concepts](https://www.google.com/search?q=%23core-concepts)

  * [Usage with `Maybe<TValue, TError>`](https://www.google.com/search?q=%23usage-with-maybetvalue-terror)

  * [Progressive Enhancement with `IOutcome`](https://www.google.com/search?q=%23progressive-enhancement-with-ioutcome)

* [Getting Started 🏃](https://www.google.com/search?q=%23getting-started-)

  * [From Throwing Exceptions to Returning Outcomes](https://www.google.com/search?q=%23from-throwing-exceptions-to-returning-outcomes)

  * [Fluent Chaining with Sync & Async Interop](https://www.google.com/search?q=%23fluent-chaining-with-sync--async-interop)

* [Creating a `Maybe` instance](https://www.google.com/search?q=%23creating-a-maybe-instance)

* [Advanced Error Handling: Specialized Errors](https://www.google.com/search?q=%23advanced-error-handling-specialized-errors)

* [Powerful Logging with `ToFullString()`](https://www.google.com/search?q=%23powerful-logging-with-tofullstring)

* [API Reference: Our Vocabulary](https://www.google.com/search?q=%23api-reference-our-vocabulary)

  * [Then (Bind / FlatMap)](https://www.google.com/search?q=%23then-bind--flatmap)

  * [Select (Map)](https://www.google.com/search?q=%23select-map)

  * [Ensure (Validate)](https://www.google.com/search?q=%23ensure-validate)

  * [Recover (Error Handling Bind)](https://www.google.com/search?q=%23recover-error-handling-bind)

  * [Match (Unwrap)](https://www.google.com/search?q=%23match-unwrap)

  * [Else (Fallback)](https://www.google.com/search?q=%23else-fallback)

  * [IfSome / IfNone (Side Effects)](https://www.google.com/search?q=%23ifsome--ifnone-side-effects)

  * [ThenDo / ElseDo (Terminal Side Effects)](https://www.google.com/search?q=%23thendo--elsedo-terminal-side-effects)

* [Expressive Success Outcomes](https://www.google.com/search?q=%23expressive-success-outcomes)

* [Generic Constraints & Custom Errors](https://www.google.com/search?q=%23generic-constraints--custom-errors)

* [Contribution 🤲](https://www.google.com/search?q=%23contribution-)

* [License 🪪](https://www.google.com/search?q=%23license-)

# Give it a star ⭐!

Loving it? Show your support by giving this project a star!

# Philosophy: Beyond Error Handling

`Maybe` is more than just an error-handling library; it's a tool for writing clearer, more expressive, and more resilient code. It encourages you to think about the different **outcomes** of your operations, not just success or failure.

By using an elegant, fluent API, `Maybe` guides you to:

* **Write code that reads like a business process.**

* **Handle both success and failure paths explicitly.**

* **Eliminate unexpected runtime exceptions.**

* **Seamlessly compose synchronous and asynchronous operations.**

# Core Concepts

`Maybe` is designed to be **simple for common cases, but powerful for advanced scenarios**.

## Usage with `Maybe<TValue, TError>`

You can specify both a success type and an error type. For common scenarios, you can use the built-in `Error` class, which provides a rich, specialized error system.

```csharp
// The error type defaults to the built-in Error class.
public Maybe<User, Error> FindUser(int id)
{
    if (id > 0)
    {
        return new User(id, ""Alice"");
    }

    // Return a built-in, specialized error type.
    return Error.NotFound(itemName: ""User"", identifier: id);
}
```

When you need to return a **custom, strongly-typed error** with specific data, you can provide your own error type.

```csharp
public class UserCreationError : GenericError { /* ... */ }

public Maybe<User, UserCreationError> CreateUser(string email)
{
    if (string.IsNullOrEmpty(email))
    {
        return new UserCreationError(""Email cannot be empty."");
    }

    // ...
}
```

## Progressive Enhancement with `IOutcome`

When you need to communicate a **more specific success state** (like `Created` or `Updated`), you can return a value that implements the `IOutcome` interface. `Maybe` will automatically inspect the value and adopt its specific `OutcomeType`, enriching your return value.

```csharp
// 'Created' implements IOutcome and has its own OutcomeType
public Maybe<Created, Error> CreateUser(string name)
{
    // ... create user ...
    return Outcomes.Created;
}

var result = CreateUser(""Bob"");
// result.Type is now 'OutcomeType.Created', not the default 'Success'.
```

# Getting Started 🏃

## From Throwing Exceptions to Returning Outcomes

This 👇

```csharp
public User GetUserById(int id)
{
    var user = _db.Users.Find(id);
    if (user is null)
    {
        throw new UserNotFoundException(""User not found"");
    }
    return user;
}
```

Turns into this 👇, using the powerful `Match` method to handle both outcomes safely.

```csharp
public Maybe<User, Error> GetUserById(int id)
{
    var user = _db.Users.Find(id);
    if (user is null)
    {
        return Error.NotFound(itemName: ""User"", identifier: id);
    }
    return user;
}

GetUserById(1)
    .Match(
        onSome: user => Console.WriteLine(user.Name),
        onNone: error => Console.WriteLine(error.Message));
```

## Fluent Chaining with Sync & Async Interop

The true power of `Maybe` lies in its fluent DSL. The API is designed to be intuitive, automatically handling the transition between synchronous and asynchronous contexts without needing different method names.

```csharp
// This example finds a user, validates their status, gets their permissions, and transforms the result.
// Notice how .Select and .Ensure are used on an async source without needing an ""Async"" suffix.

var result = await Api.FindUserAsync(userId)              // Start: Task<Maybe<User, Error>>
    .Ensure(user => user.IsActive, Error.Failure(""User is inactive""))  // Then:  Sync validation
    .Select(user => user.Name.ToUpper())                 // Then:  Sync transformation
    .ThenAsync(name => Api.GetPermissionsAsync(name))    // Then:  Async chain
    .Select(permissions => permissions.ToUpper());       // Finally: Sync transformation
```

# Creating a `Maybe` instance

Creating a `Maybe` is designed to be frictionless, primarily through **implicit conversions**.

```csharp
public Maybe<User, Error> FindUser(int id)
{
    if (id == 1)
    {
        return new User(1, ""Alice"", true); // Implicit conversion from User to Maybe<User, Error>
    }

    return Error.NotFound(itemName: ""User"", identifier: id); // Implicit conversion from Error to Maybe<User, Error>
}
```

# Advanced Error Handling: Specialized Errors

`Maybe` shines with its rich, specialized error system. Instead of returning generic errors, you can use the built-in factory methods on the `Error` class to create descriptive, structured errors.

### `ValidationError`

For handling invalid input data, including field-specific details.

```csharp
var fieldErrors = new Dictionary<string, string>
{
    [""Email""] = ""Email address is already in use."",
    [""Password""] = ""Password is too weak.""
};
var validationError = Error.Validation(fieldErrors, ""User registration failed."");
// You can access the specific field errors later:
// if (validationError is ValidationError v) { ... v.FieldErrors ... }
```

### `NotFoundError`

For when a requested resource cannot be found.

```csharp
var notFoundError = Error.NotFound(itemName: ""Product"", identifier: ""SKU-12345"");
// notFoundError.EntityName -> ""Product""
// notFoundError.Identifier -> ""SKU-12345""
```

### `ConflictError`

For conflicts with the current state of a resource (e.g., duplicates, stale data).

```csharp
var conflictingParams = new Dictionary<string, object> { [""Username""] = ""john.doe"" };
var conflictError = Error.Conflict(
    ConflictType.Duplicate, 
    resourceType: ""User"", 
    conflictingParameters: conflictingParams);
```

### `AuthorizationError`

For authentication (`Unauthorized`) or permission (`Forbidden`) failures.

```csharp
var authError = Error.Forbidden(
    action: ""DeleteResource"", 
    resourceIdentifier: ""res-abc"", 
    userId: ""user-789"");
```

### `UnexpectedError`

For wrapping system exceptions while preserving the original context for logging.

```csharp
try { /* ... */ }
catch (Exception ex)
{
    return Error.Unexpected(ex, ""Failed to communicate with the payment gateway."");
}
```

### `FailureError`

For expected but significant process failures, with additional context for debugging.

```csharp
var context = new Dictionary<string, object> { [""TransactionId""] = ""txn_54321"" };
var failure = Error.Failure(
    message: ""The payment was declined by the gateway."",
    code: ""Payment.GatewayDeclined"",
    contextData: context);
```

# Powerful Logging with `ToFullString()`

Every `Error` object, including its inner errors, can be formatted into a detailed, aligned, and readable string perfect for logging.

```csharp
// Create a chain of errors
var dbError = Error.NotFound(""User"", 123);
var serviceError = Error.Failure(
    message: ""Failed to process order"", 
    code: ""Order.Processing"", 
    innerError: dbError);

// Print the full, formatted error chain
Console.WriteLine(serviceError.ToFullString());
```

**Output:**

```
[Failure]    Order.Processing    [2025-09-14 11:00:00]   Failed to process order
  [NotFound]   NotFound.User       [2025-09-14 11:00:00]   User with identifier '123' was not found.
```

# API Reference: Our Vocabulary

### Then (Bind / FlatMap)

**Purpose**: To chain an operation that *itself returns a `Maybe`*. This is the primary method for sequencing operations that can fail.

```csharp
// Finds a user, and if successful, gets their permissions.
Maybe<Permissions, PermissionsError> result = Api.FindUserInDb(1)
    .Then(user => Api.GetPermissions(user));
```

### Select (Map)

**Purpose**: To transform the *value* inside a successful `Maybe` into something else, without altering the `Maybe`'s state.

```csharp
// Finds a user, and if successful, selects their email address.
Maybe<string, UserNotFoundError> userEmail = Api.FindUserInDb(1)
    .Select(user => user.Email);
```

### Ensure (Validate)

**Purpose**: To check if the value inside a successful `Maybe` meets a specific condition. If the condition is not met, the chain is switched to an error state.

The library provides two sets of `Ensure` overloads:

1. **Ergonomic (Preserves Error Type)**: Used when the validation error is of the same type as the `Maybe`'s error channel.

   ```csharp
   Maybe<User, PermissionsError> validatedUser = GetUser() // Returns Maybe<User, PermissionsError>
       .Ensure(u => u.IsActive, new PermissionsError());   // Error is also PermissionsError
   ```

2. **Unifying (Changes Error Type)**: Used when the validation introduces a new, potentially incompatible error type. The result is unified to a `Maybe` whose error channel is a common base type, typically `Error`.

   ```csharp
   // GetUser() returns Maybe<User, UserNotFoundError>
   // The result is Maybe<User, Error> to accommodate both UserNotFoundError and ValidationError.
   Maybe<User, Error> validatedUser = GetUser()
       .Ensure(u => u.Age > 18, Error.Validation(new()));
   ```

### Recover (Error Handling Bind)

**Purpose**: To handle a failure by executing a recovery function that can return a new `Maybe`.

```csharp
// Try to find a user in the database. If not found, try the cache.
Maybe<User, CacheError> result = await Api.FindUserInDbAsync(1)
    .RecoverAsync(error => Api.FindUserInCache(1));
```

### Match (Unwrap)

**Purpose**: To safely exit the `Maybe` context by providing functions for both success and error cases.

```csharp
string message = maybeUser.Match(
    onSome: user => $""Welcome, {user.Name}!"",
    onNone: error => $""Error: {error.Message}""
);
```

### Else (Fallback)

**Purpose**: To exit the `Maybe` context by providing a default value in case of an error.

```csharp
string userName = maybeUser.Select(u => u.Name).Else(""Guest"");
```

### IfSome / IfNone (Side Effects)

**Purpose**: To perform an action (like logging) without altering the `Maybe`. It returns the original `Maybe`, allowing the chain to continue.

```csharp
Maybe<User, UserNotFoundError> finalResult = Api.FindUserInDb(1)
    .IfSome(user => Console.WriteLine($""User found: {user.Id}""))
    .IfNone(error => Console.WriteLine($""Failed to find user: {error.Code}""));
```

### ThenDo / ElseDo (Terminal Side Effects)

**Purpose**: To perform a final action on success (`ThenDo`) or failure (`ElseDo`). These methods terminate the fluent chain.

```csharp
// Example: Final logging after a chain of operations
await Api.FindUserInDbAsync(1)
    .Then(Api.GetPermissions)
    .ThenDoAsync(permissions => Log.Information($""Permissions granted: {permissions}""))
    .ElseDoAsync(error => Log.Error($""Operation failed: {error.Code}""));
```

# Expressive Success Outcomes

As explained in the [Core Concepts](https://www.google.com/search?q=%23core-concepts), you can use types that implement `IOutcome` to communicate richer success states. `Maybe` provides a set of built-in, stateless `struct` types for common ""void"" operations, accessible via the `Outcomes` static class:

* `Outcomes.Success`

* `Outcomes.Created`

* `Outcomes.Updated`

* `Outcomes.Deleted`

* `Outcomes.Accepted`

* `Outcomes.Unchanged`

* `new Cached<T>(value)`

```csharp
public Maybe<Deleted, Error> DeleteUser(int id)
{
    if (UserExists(id))
    {
        _db.Users.Remove(id);
        return Outcomes.Deleted; // More expressive than returning void or true
    }

    return Error.NotFound(itemName: ""User"", identifier: id);
}
```

# Generic Constraints & Custom Errors

The `Maybe<TValue, TError>` struct requires `TError` to have a parameterless constructor via the `where TError : Error, new()` constraint. All specialized errors provided by this library fulfill this requirement.

If you create your own custom error classes, they must also provide a public parameterless constructor. It's recommended to inherit from `GenericError` for simplicity and to gain access to features like `ToFullString()`.

```csharp
// Your custom error must have a parameterless constructor.
public class MyCustomError : GenericError 
{
    public MyCustomError() { /* ... */ }

    public MyCustomError(string message) 
        : base(OutcomeType.Failure, ""Custom.Code"", message) { }
}

// This allows it to be used in generic methods with the `new()` constraint.
public Maybe<T, TError> GenericOperation<T, TError>() where TError : Error, new()
{
    // ...
    if (someCondition)
    {
        // Now this is possible
        return new TError(); 
    }
    // ...
}
```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/your-repo/maybe/blob/main/LICENSE) license.

