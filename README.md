<div align="center">

<img src="https://raw.githubusercontent.com/lucafabbri/maybe/main/maybe_logo.png" alt="drawing" width="350"/></br>

# Maybe

[![NuGet](https://img.shields.io/nuget/v/Maybe.svg)](https://www.nuget.org/packages/Maybe)
[![Build](https://github.com/lucafabbri/maybe/actions/workflows/build.yml/badge.svg)](https://github.com/lucafabbri/maybe/actions/workflows/build.yml) 
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
- [The Fluent DSL: Our Vocabulary](#the-fluent-dsl-our-vocabulary)
- [Expressive Success Outcomes](#expressive-success-outcomes)
- [Custom Errors](#custom-errors)
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

var result = await Api.FindUserAsync(userId)                   // Start: Task<Maybe<User>>
    .Ensure(user => user.IsActive, Errors.UserInactive)       // Then:  Sync validation
    .Select(user => user.Name.ToUpper())                        // Then:  Sync transformation
    .ThenAsync(name => Api.GetPermissionsAsync(name))           // Then:  Async chain
    .Select(permissions => permissions.ToUpper());            // Finally: Sync transformation
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

# The Fluent DSL: Our Vocabulary
A brief overview of the most common methods for composing operations.

- **`Match`**: The primary and safest way to exit the `Maybe` context by handling both success and error paths.
- **`Select` (Map)**: Transforms the success value while staying inside the `Maybe` context.
- **`Then` (Chain)**: Chains another operation that itself returns a `Maybe`, perfect for sequencing operations.
- **`Ensure`**: Applies a validation rule to the success value.
- **`IfSome` & `IfNone`**: Executes a side-effect (like logging) without changing the state.
- **`Recover`**: Provides a fallback operation in case of an error.
- **`Else`**: Exits the `Maybe` context by providing a fallback value in case of an error.

# Expressive Success Outcomes

As explained in the [Core Concepts](#core-concepts), you can use types that implement `IOutcome` to communicate richer success states. `Maybe` provides a set of built-in, stateless `struct` types for common "void" operations, accessible via the `Outcomes` static class:
- `Outcomes.Success`
- `Outcomes.Created`
- `Outcomes.Updated`
- `Outcomes.Deleted`
- `Outcomes.Accepted`
- `Outcomes.Unchanged`
- `new Cached<T>(value)`

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
