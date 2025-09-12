<div align="center">

<img src="https://raw.githubusercontent.com/lucafabbri/maybe/main/maybe_logo.png" alt="drawing" width="350"/></br>

# Maybe

[![NuGet](https://img.shields.io/nuget/v/Maybe.svg)](https://www.nuget.org/packages/Maybe)
[![Build](https://github.com/lucafabbri/maybe/actions/workflows/main.yml/badge.svg)](https://github.com/lucafabbri/maybe/actions/workflows/main.yml) 
[![codecov](https://codecov.io/gh/lucafabbri/maybe/branch/main/graph/badge.svg)](https://codecov.io/gh/lucafabbri/maybe)

[![GitHub Stars](https://img.shields.io/github/stars/lucafabbri/maybe.svg)](https://github.com/lucafabbri/maybe/stargazers) 
[![GitHub license](https://img.shields.io/github/license/lucafabbri/maybe)](https://github.com/lucafabbri/maybe/blob/main/LICENSE)

---

### An elegant, fluent, and intuitive way to handle operations that may succeed or fail.

`dotnet add package Maybe`

</div>

- [Give it a star ⭐!](#give-it-a-star-)
- [Philosophy: Beyond Error Handling](#philosophy-beyond-error-handling)
- [Core Concepts](#core-concepts)
  - [Simplified Usage with `Maybe<TValue>`](#simplified-usage-with-maybetvalue)
  - [Advanced Usage with `Maybe<TValue, TError>`](#advanced-usage-with-maybetvalue-terror)
  - [Progressive Enhancement with `IOutcome`](#progressive-enhancement-with-ioutcome)
- [Getting Started 🏃](#getting-started-)
  - [From Throwing Exceptions to Returning Outcomes](#from-throwing-exceptions-to-returning-outcomes)
  - [Fluent Chaining with Sync & Async Interop](#fluent-chaining-with-sync--async-interop)
- [Creating a `Maybe` instance](#creating-a-maybe-instance)
- [API Reference: Our Vocabulary](#api-reference-our-vocabulary)
  - [Then (Bind / FlatMap)](#then-bind--flatmap)
  - [Select (Map)](#select-map)
  - [Ensure (Validate)](#ensure-validate)
  - [Recover (Error Handling Bind)](#recover-error-handling-bind)
  - [Match (Unwrap)](#match-unwrap)
  - [Else (Fallback)](#else-fallback)
  - [IfSome / IfNone (Side Effects)](#ifsome--ifnone-side-effects)
  - [ThenDo / ElseDo (Terminal Side Effects)](#thendo--elsedo-terminal-side-effects)
- [Expressive Success Outcomes](#expressive-success-outcomes)
- [Custom Errors](#custom-errors)
- [Contribution 🤲](#contribution-)
- [License 🪪](#license-)

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

## Simplified Usage with `Maybe<TValue>`

For the majority of use cases, you only need to specify the success type. The error type defaults to a built-in `Error` struct that covers all common failure scenarios.

```csharp
// This signature is clean and simple.
public Maybe<User> FindUser(int id)
{
    if (id > 0)
    {
        return new User(id, "Alice");
    }

    // Return a built-in error type.
    return Error.NotFound("User.NotFound", "The user was not found.");
}
```

## Advanced Usage with `Maybe<TValue, TError>`

When you need to return a **custom, strongly-typed error** with specific data, you can use the two-parameter version. This gives you full control over the failure path.

```csharp
public record UserCreationError(string Field, string Message) : IError { /* ... */ }

public Maybe<User, UserCreationError> CreateUser(string email)
{
    if (string.IsNullOrEmpty(email))
    {
        return new UserCreationError("Email", "Email cannot be empty.");
    }

    // ...
}
```

## Progressive Enhancement with `IOutcome`

When you need to communicate a **more specific success state** (like `Created` or `Updated`), you can return a value that implements the `IOutcome` interface. `Maybe` will automatically inspect the value and adopt its specific `OutcomeType`, enriching your return value.

```csharp
// 'Created' implements IOutcome and has its own OutcomeType
public Maybe<Created> CreateUser(string name)
{
    // ... create user ...
    return Outcomes.Created;
}

var result = CreateUser("Bob");
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
        throw new UserNotFoundException("User not found");
    }
    return user;
}
```

Turns into this 👇, using the powerful `Match` method to handle both outcomes safely.

```csharp
public Maybe<User> GetUserById(int id)
{
    var user = _db.Users.Find(id);
    if (user is null)
    {
        return Error.NotFound("User.NotFound", "User was not found.");
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
// Notice how .Select and .Ensure are used on an async source without needing an "Async" suffix.

var result = await Api.FindUserAsync(userId)              // Start: Task<Maybe<User>>
    .Ensure(user => user.IsActive, Errors.UserInactive)   // Then:  Sync validation
    .Select(user => user.Name.ToUpper())                   // Then:  Sync transformation
    .ThenAsync(name => Api.GetPermissionsAsync(name))      // Then:  Async chain
    .Select(permissions => permissions.ToUpper());         // Finally: Sync transformation
```

# Creating a `Maybe` instance

Creating a `Maybe` is designed to be frictionless, primarily through **implicit conversions**.

```csharp
public Maybe<User> FindUser(int id)
{
    if (id == 1)
    {
        return new User(1, "Alice", true); // Implicit conversion from User to Maybe<User>
    }

    return Error.NotFound(); // Implicit conversion from Error to Maybe<User>
}
```

# API Reference: Our Vocabulary

### Then (Bind / FlatMap)

**Purpose**: To chain an operation that *itself returns a `Maybe`*. This is the primary method for sequencing operations that can fail.

```csharp
// Finds a user, and if successful, gets their permissions.
Maybe<string, PermissionsError> result = Api.FindUserInDb(1)
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

2. **Unifying (Changes Error Type)**: Used when the validation introduces a new, potentially incompatible error type, unifying the result to a `Maybe<TValue>`.

   ```csharp
   Maybe<User> validatedUser = GetUser() // Returns Maybe<User, UserNotFoundError>
       .Ensure(u => u.Age > 18, Error.Validation("User.NotAdult")); // Introduces a new Error
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
    onSome: user => $"Welcome, {user.Name}!",
    onNone: error => $"Error: {error.Message}"
);
```

### Else (Fallback)

**Purpose**: To exit the `Maybe` context by providing a default value in case of an error.

```csharp
string userName = maybeUser.Select(u => u.Name).Else("Guest");
```

### IfSome / IfNone (Side Effects)

**Purpose**: To perform an action (like logging) without altering the `Maybe`. It returns the original `Maybe`, allowing the chain to continue.

```csharp
Maybe<User, UserNotFoundError> finalResult = Api.FindUserInDb(1)
    .IfSome(user => Console.WriteLine($"User found: {user.Id}"))
    .IfNone(error => Console.WriteLine($"Failed to find user: {error.Code}"));
```

### ThenDo / ElseDo (Terminal Side Effects)

**Purpose**: To perform a final action on success (`ThenDo`) or failure (`ElseDo`). These methods terminate the fluent chain.

```csharp
// Example: Final logging after a chain of operations
await Api.FindUserInDbAsync(1)
    .Then(Api.GetPermissions)
    .ThenDoAsync(permissions => Log.Information($"Permissions granted: {permissions}"))
    .ElseDoAsync(error => Log.Error($"Operation failed: {error.Code}"));
```

# Expressive Success Outcomes

As explained in the [Core Concepts](#core-concepts), you can use types that implement `IOutcome` to communicate richer success states. `Maybe` provides a set of built-in, stateless `struct` types for common "void" operations, accessible via the `Outcomes` static class:

* `Outcomes.Success`

* `Outcomes.Created`

* `Outcomes.Updated`

* `Outcomes.Deleted`

* `Outcomes.Accepted`

* `Outcomes.Unchanged`

* `new Cached<T>(value)`

```csharp
public Maybe<Deleted> DeleteUser(int id)
{
    if (UserExists(id))
    {
        _db.Users.Remove(id);
        return Outcomes.Deleted; // More expressive than returning void or true
    }

    return Error.NotFound();
}
```

# Custom Errors

While the built-in `Error` struct is sufficient for many cases, you can create your own strongly-typed errors by implementing `IError`. This is the primary use case for `Maybe<TValue, TError>`.

```csharp
public record InvalidEmailError(string Email) : IError
{
    public OutcomeType Type => OutcomeType.Validation;
    public string Code => "Email.Invalid";
    public string Message => $"The email '{Email}' is not a valid address.";
}

public Maybe<User, InvalidEmailError> CreateUser(string email)
{
    if (!IsValid(email))
    {
        return new InvalidEmailError(email);
    }
    // ...
}

```

# Contribution 🤲

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# License 🪪

This project is licensed under the terms of the [MIT](https://github.com/your-repo/maybe/blob/main/LICENSE) license.
