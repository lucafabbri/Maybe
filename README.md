namespace Maybe.Docs;

/// <summary>
/// Contains the raw source code of the README.md file for easy access and display.
/// </summary>
public static class ReadmeSource
{
    /// <summary>
    /// The complete content of the README.md file.
    /// </summary>
    public const string Content = """
<div align="center">

<!-- TODO: Add a cool logo/icon for the library -->
<img src="https://raw.githubusercontent.com/lucafabbri/maybe-logo.png" alt="drawing" width="200"/></br>

# Maybe

[![NuGet](https://img.shields.io/nuget/v/Maybe.svg)](https://www.nuget.org/packages/Maybe)
[![Build](https://github.com/your-repo/maybe/actions/workflows/build.yml/badge.svg)](https://github.com/your-repo/maybe/actions/workflows/build.yml) 
[![codecov](https://codecov.io/gh/your-repo/maybe/branch/main/graph/badge.svg)](https://codecov.io/gh/your-repo/maybe)

[![GitHub Stars](https://img.shields.io/github/stars/your-repo/maybe.svg)](https://github.com/your-repo/maybe/stargazers) 
[![GitHub license](https://img.shields.io/github/license/your-repo/maybe)](https://github.com/your-repo/maybe/blob/main/LICENSE)

---

### An elegant, fluent, and intuitive way to handle operations that may succeed or fail.

`dotnet add package Maybe`

</div>

- [Maybe](#maybe)
		- [An elegant, fluent, and intuitive way to handle operations that may succeed or fail.](#an-elegant-fluent-and-intuitive-way-to-handle-operations-that-may-succeed-or-fail)
- [Give it a star ⭐!](#give-it-a-star-)
- [Philosophy: Beyond Error Handling](#philosophy-beyond-error-handling)
- [Core Concepts: Progressive Enhancement](#core-concepts-progressive-enhancement)
	- [Simple by Default](#simple-by-default)
	- [Powerful When Needed](#powerful-when-needed)
- [Getting Started 🏃](#getting-started-)
	- [From Throwing Exceptions to Returning Outcomes](#from-throwing-exceptions-to-returning-outcomes)
	- [Fluent Chaining with Sync \& Async Interop](#fluent-chaining-with-sync--async-interop)
- [Creating a `Maybe` instance](#creating-a-maybe-instance)
	- [Using Implicit Conversion](#using-implicit-conversion)
	- [Using Static Factory Methods](#using-static-factory-methods)
- [Inspecting the `Maybe`](#inspecting-the-maybe)
	- [`IsSuccess` \& `IsError`](#issuccess--iserror)
	- [`Type`](#type)
- [Unwrapping the `Maybe` (Exiting the Context)](#unwrapping-the-maybe-exiting-the-context)
	- [`Match`](#match)
	- [Unsafe Access (Explicit)](#unsafe-access-explicit)
	- [Fallbacks](#fallbacks)
- [The Fluent DSL: Our Vocabulary](#the-fluent-dsl-our-vocabulary)
	- [`Select` (Map)](#select-map)
	- [`Then` (Chain)](#then-chain)
	- [`Ensure`](#ensure)
	- [`IfSome` \& `IfNone`](#ifsome--ifnone)
	- [`Recover`](#recover)
- [Expressive Success Outcomes](#expressive-success-outcomes)
	- [Built-in Outcome Types](#built-in-outcome-types)
	- [Inspirational Outcomes (`Accepted`, `Unchanged`, `Cached<T>`)](#inspirational-outcomes-accepted-unchanged-cachedt)
- [Organizing Errors](#organizing-errors)
- [Contribution 🤲](#contribution-)
- [License 🪪](#license-)

# Give it a star ⭐!

Loving it? Show your support by giving this project a star!

# Philosophy: Beyond Error Handling

`Maybe` is more than just an error-handling library; it's a tool for writing clearer, more expressive, and more resilient code. It encourages you to think about the different **outcomes** of your operations, not just success or failure.

By using an elegant, fluent API, `Maybe` guides you to:
- **Write code that reads like a business process.**
- **Handle both success and failure paths explicitly.**
- **Eliminate unexpected runtime exceptions.**
- **Seamlessly compose synchronous and asynchronous operations.**

# Core Concepts: Progressive Enhancement

`Maybe` is designed to be **simple by default, but powerful when you need it**. This is achieved through the optional use of the `IOutcome` interface.

## Simple by Default

You can use `Maybe` with any of your existing domain types without modification. The library doesn't force you into a specific structure.

```csharp
// User does not need to implement any special interface
public record User(int Id, string Name);

public Maybe<User, IError> FindUser(int id)
{
    if (id > 0)
    {
        return new User(id, "Alice");
    }

    return Errors.UserNotFound;
}

var result = FindUser(1);
// result.Type will be 'OutcomeType.Success' by default
```

## Powerful When Needed

When you need to communicate a more specific success state (like `Created` or `Updated`), you can use a success type that implements `IOutcome`. `Maybe` will automatically inspect the value and adopt its specific `OutcomeType`.

```csharp
// 'Created' implements IOutcome and has its own OutcomeType
public Maybe<Created, IError> CreateUser(string name)
{
    // ... create user ...
    return Outcomes.Created;
}

var result = CreateUser("Bob");
// result.Type is now 'OutcomeType.Created'
```
This approach gives you the flexibility to choose the level of detail you need for each operation.

# Getting Started 🏃

## From Throwing Exceptions to Returning Outcomes

This 👇

```csharp
public User GetUserById(int id)
{
    var user = _db.Users.Find(id);
    if (user is null)
    {
        throw new UserNotFoundException("User not found");
    }

    return user;
}

try
{
    var user = GetUserById(1);
    Console.WriteLine(user.Name);
}
catch (UserNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
```

Turns into this 👇, using the powerful `Match` method to handle both outcomes safely.

```csharp
public Maybe<User, IError> GetUserById(int id)
{
    var user = _db.Users.Find(id);
    if (user is null)
    {
        return Errors.UserNotFound;
    }

    return user;
}

GetUserById(1)
    .Match(
        onSome: user => Console.WriteLine(user.Name),
        onNone: error => Console.WriteLine(error.Message));
```

## Fluent Chaining with Sync & Async Interop

The true power of `Maybe` lies in its fluent DSL that allows you to chain operations elegantly. The API is designed to be intuitive, automatically handling the transition between synchronous and asynchronous contexts.

```csharp
// This example finds a user, validates their status, gets their permissions, and transforms the result.
// Notice how .Select and .Ensure are used on an async source without needing an "Async" suffix.

var result = await Api.FindUserAsync(userId)                   // Start with Task<Maybe<User>>
    .Ensure(user => user.IsActive, Errors.UserInactive)         // Apply a sync validation
    .Select(user => user.Name.ToUpper())                        // Apply a sync transformation
    .ThenAsync(name => Api.GetPermissionsAsync(new User(userId, name, true))) // Chain with an async operation
    .Select(permissions => permissions.ToUpper());            // Apply another sync transformation
```

# Creating a `Maybe` instance

## Using Implicit Conversion

There are implicit converters from your value `TValue` or your error `TError` to `Maybe<TValue, TError>`.

```csharp
public Maybe<User, IError> FindUser(int id)
{
    if (id == 1)
    {
        return new User(1, "Alice", true); // Implicit conversion from User
    }

    return Errors.UserNotFound; // Implicit conversion from IError
}
```

## Using Static Factory Methods

You can also be more explicit using the `Maybe.Some` and `Maybe.None` factory methods.

```csharp
return Maybe<User, IError>.Some(new User(1, "Alice", true));
return Maybe<User, IError>.None(Errors.UserNotFound);
```

# Inspecting the `Maybe`

## `IsSuccess` & `IsError`

You can check the state of the `Maybe` using these boolean properties.

```csharp
var result = Api.FindUser(1);

if (result.IsSuccess)
{
    // ...
}
else if(result.IsError)
{
    // ...
}
```

## `Type`

The `Type` property gives you the `OutcomeType` of the result. For errors, this comes from the `IError` implementation. For successes, it's inferred from your value if it implements `IOutcome`; otherwise, it defaults to `OutcomeType.Success`. This is incredibly useful for infrastructure code (e.g., mapping results to HTTP status codes).

```csharp
var result = Api.FindUser(1);
var outcomeType = result.Type; // Could be Success, NotFound, Forbidden, etc.
```

# Unwrapping the `Maybe` (Exiting the Context)

## `Match`

`Match` is the **primary and safest** way to get the value out of a `Maybe`. It forces you to handle both the success and error paths, guaranteeing that your code is correct at compile time.

```csharp
string message = result.Match(
    onSome: user => $"Welcome, {user.Name}!",
    onNone: error => $"An error occurred: {error.Code}");
```
There are also `MatchAsync` overloads for handling asynchronous transformations.

## Unsafe Access (Explicit)

For scenarios like unit tests where you are certain of the state, you can use methods that will throw an exception if your assumption is wrong. The names are deliberately explicit to signal danger.

- `ValueOrThrow()`: Gets the success value or throws an `InvalidOperationException`.
- `ErrorOrThrow()`: Gets the error or throws an `InvalidOperationException`.

## Fallbacks

You can provide default values or fallbacks if an operation fails.

- `ValueOrDefault()`: Returns the success value, or `default` (`null` for reference types, `0` for `int`, etc.) if it's an error.

# The Fluent DSL: Our Vocabulary

This is the core of the fluent API, allowing you to compose complex operations.

## `Select` (Map)

Transforms the success value while staying inside the `Maybe` context. Errors are propagated automatically.

```csharp
Maybe<string, IError> maybeName = Api.FindUser(1).Select(user => user.Name);
```

## `Then` (Chain)

Chains another operation that itself returns a `Maybe`. This is the primary tool for sequencing operations that can fail.

```csharp
var permissions = Api.FindUser(1)
    .Then(user => Api.GetPermissions(user));
```

## `Ensure`

Applies a validation rule to the success value. If the rule fails, it returns a specified error.

```csharp
var activeUser = Api.FindUser(2)
    .Ensure(user => user.IsActive, Errors.UserInactive); // This will result in an error
```

## `IfSome` & `IfNone`

Executes a side-effect (like logging) without changing the state of the `Maybe`.

```csharp
var result = Api.FindUser(3)
    .IfSome(user => Console.WriteLine("User found!"))
    .IfNone(error => Console.WriteLine($"Failed to find user: {error.Message}"));
```

## `Recover`

Provides a fallback operation in case of an error, allowing the chain to "recover" into a success state.

```csharp
var result = Api.FindUser(3) // This fails
    .Recover(error => Api.FindUser(1)); // Recovers by finding the default user
```

# Expressive Success Outcomes

As explained in the [Core Concepts](#core-concepts-progressive-enhancement), you can use types that implement `IOutcome` to communicate richer success states. `Maybe` provides a set of built-in, stateless `struct` types for common "void" operations.

## Built-in Outcome Types

```csharp
public Maybe<Deleted, IError> DeleteUser(int id)
{
    if (UserExists(id))
    {
        _db.Users.Remove(id);
        return Outcomes.Deleted; // More expressive than returning void or true
    }

    return Errors.UserNotFound;
}
```

The available outcomes are accessible via the `Outcomes` static class:
- `Outcomes.Success`
- `Outcomes.Created`
- `Outcomes.Updated`
- `Outcomes.Deleted`

## Inspirational Outcomes (`Accepted`, `Unchanged`, `Cached<T>`)

`Maybe` also provides "inspirational" outcomes that encourage better architectural patterns.

- `Outcomes.Accepted`: For long-running operations. Signals that a request was accepted and is being processed in the background.
- `Outcomes.Unchanged`: For update operations that resulted in no change. This allows you to avoid costly downstream operations like cache invalidation.
- `new Cached<T>(value)`: Signals that a value was successfully retrieved from a cache.

# Organizing Errors

A good practice is to organize your domain-specific errors in static classes.

```csharp
public static class Errors
{
    public static readonly TestError UserNotFound = new(OutcomeType.NotFound, "User.NotFound", "The user was not found.");
    public static readonly TestError UserInactive = new(OutcomeType.Forbidden, "User.Inactive", "The user account is inactive.");
}
```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/your-repo/maybe/blob/main/LICENSE) license.
""";
}

