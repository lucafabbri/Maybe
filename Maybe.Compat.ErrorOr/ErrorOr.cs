namespace Maybe;

/// <summary>
/// A compatibility type that mirrors the public API of ErrorOr.ErrorOr<TValue>
/// but is implemented using the core Maybe<TValue> struct internally.
/// </summary>
public readonly record struct ErrorOr<TValue>
{
    private readonly Maybe<TValue> _maybe;

    public bool IsError => _maybe.IsError;
    public TValue Value => _maybe.ValueOrThrow("Cannot access the value of an error outcome.");
    public List<Maybe.Error> Errors => IsError ? [(Maybe.Error)_maybe.ErrorOrThrow("Cannot access the error of a success outcome.")] : [];
    public Maybe.Error FirstError => (Maybe.Error)_maybe.ErrorOrThrow("Cannot access the error of a success outcome.");

    private ErrorOr(Maybe<TValue> maybe) => _maybe = maybe;
    private ErrorOr(TValue value) => _maybe = value;
    private ErrorOr(Maybe.Error error) => _maybe = error;
    private ErrorOr(List<Maybe.Error> errors) => _maybe = errors.FirstOrDefault();

    // --- Implicit Conversions ---
    public static implicit operator ErrorOr<TValue>(TValue value) => new(value);
    public static implicit operator ErrorOr<TValue>(Maybe.Error error) => new(error);
    public static implicit operator ErrorOr<TValue>(List<Maybe.Error> errors) => new(errors);
    public static implicit operator ErrorOr<TValue>(Maybe<TValue> maybe) => new(maybe);

    // --- Core DSL Methods (facade over Maybe) ---

    public TResult Match<TResult>(Func<TValue, TResult> onValue, Func<List<Maybe.Error>, TResult> onErrors)
        => _maybe.Match(onValue, error => onErrors([(Maybe.Error)error]));

    public TResult MatchFirst<TResult>(Func<TValue, TResult> onValue, Func<Maybe.Error, TResult> onFirstError)
        => _maybe.Match(onValue, error => onFirstError((Maybe.Error)error));

    public void Switch(Action<TValue> onValue, Action<List<Maybe.Error>> onErrors)
        => _maybe.IfSome(onValue).IfNone(error => onErrors([(Maybe.Error)error]));

    public void SwitchFirst(Action<TValue> onValue, Action<Maybe.Error> onFirstError)
        => _maybe.IfSome(onValue).IfNone(error => onFirstError((Maybe.Error)error));

    public ErrorOr<TNextValue> Then<TNextValue>(Func<TValue, TNextValue> onValue)
    {
        return _maybe.IsSuccess
            ? Maybe<TNextValue>.Some(onValue(_maybe.ValueOrThrow()))
            : Maybe<TNextValue>.None((Error)_maybe.ErrorOrThrow());
    }

    public ErrorOr<TNextValue> Then<TNextValue>(Func<TValue, ErrorOr<TNextValue>> onValue){
        var maybe = _maybe.Then(v => onValue(v)._maybe);
        return maybe.IsSuccess ?
            new ErrorOr<TNextValue>(maybe.ValueOrThrow()) : 
            new ErrorOr<TNextValue>(maybe.ErrorOrThrow());
    }

    public ErrorOr<TValue> ThenDo(Action<TValue> action)
        => new(_maybe.IfSome(action));

    public ErrorOr<TValue> FailIf(Func<TValue, bool> predicate, Maybe.Error error)
    {
        var maybe = _maybe.Ensure(predicate, error);
        return maybe.IsSuccess ?
            new ErrorOr<TValue>(maybe.ValueOrThrow()) :
            new ErrorOr<TValue>(maybe.ErrorOrThrow());
    }

    public TValue Else(TValue fallbackValue)
        => _maybe.Else(fallbackValue);

    public TValue Else(Func<List<Maybe.Error>, TValue> fallbackFunc)
        => _maybe.Else(error => fallbackFunc([(Maybe.Error)error]));

    // --- Async Methods ---

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<List<Maybe.Error>, Task<TResult>> onErrors)
    {
        if (IsError)
        {
            return await onErrors(Errors).ConfigureAwait(false);
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    public async Task SwitchAsync(Func<TValue, Task> onValue, Func<List<Maybe.Error>, Task> onErrors)
    {
        if (IsError)
        {
            await onErrors(Errors).ConfigureAwait(false);
        }
        else
        {
            await onValue(Value).ConfigureAwait(false);
        }
    }

    public async Task<ErrorOr<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<TNextValue>> onValue)
    {
        if (IsError)
        {
            return FirstError;
        }

        var nextValue = await onValue(Value).ConfigureAwait(false);
        return new ErrorOr<TNextValue>(nextValue);
    }

    public async Task<ErrorOr<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<ErrorOr<TNextValue>>> onValue)
    {
        if (IsError)
        {
            return FirstError;
        }

        return await onValue(Value).ConfigureAwait(false);
    }

    public async Task<ErrorOr<TValue>> ThenDoAsync(Func<TValue, Task> action)
    {
        if (!IsError)
        {
            await action(Value).ConfigureAwait(false);
        }

        return this;
    }
}


public static class ErrorOrExtensions
{
    public static async Task<ErrorOr<TNextValue>> Then<TValue, TNextValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Func<TValue, TNextValue> onValue)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return result.Then(onValue);
    }

    public static async Task<ErrorOr<TNextValue>> Then<TValue, TNextValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Func<TValue, ErrorOr<TNextValue>> onValue)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return result.Then(onValue);
    }

    public static async Task<ErrorOr<TValue>> ThenDo<TValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Action<TValue> action)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return result.ThenDo(action);
    }

    public static async Task<ErrorOr<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Func<TValue, Task<TNextValue>> onValue)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    public static async Task<ErrorOr<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Func<TValue, Task<ErrorOr<TNextValue>>> onValue)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return await result.ThenAsync(onValue).ConfigureAwait(false);
    }

    public static async Task<ErrorOr<TValue>> ThenDoAsync<TValue>(
        this Task<ErrorOr<TValue>> errorOrTask,
        Func<TValue, Task> action)
    {
        var result = await errorOrTask.ConfigureAwait(false);
        return await result.ThenDoAsync(action).ConfigureAwait(false);
    }
}