using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

/// <summary>
/// Provides a set of extension methods for the <see cref="Maybe{TValue, TError}"/> type,
/// enabling a fluent and functional Domain-Specific Language (DSL).
/// </summary>
public static partial class MaybeExtensions
{
	/// <summary>
	/// Transforms the Maybe by applying one of two functions, depending on the outcome's state.
	/// This is the primary method for safely exiting the Maybe context.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult Match<TValue, TError, TResult>(
		this in Maybe<TValue, TError> maybe,
		Func<TValue, TResult> onSome,
		Func<TError, TResult> onNone)
		where TError : IError
	{
		return maybe.IsSuccess
			? onSome(maybe.ValueOrThrow())
			: onNone(maybe.ErrorOrThrow());
	}

	/// <summary>
	/// Asynchronously transforms the Maybe by applying one of two functions, depending on the outcome's state.
	/// </summary>
	public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
		this Task<Maybe<TValue, TError>> maybeTask,
		Func<TValue, Task<TResult>> onSome,
		Func<TError, Task<TResult>> onNone)
		where TError : IError
	{
		var maybe = await maybeTask.ConfigureAwait(false);
		if (maybe.IsSuccess)
		{
			return await onSome(maybe.ValueOrThrow()).ConfigureAwait(false);
		}

		return await onNone(maybe.ErrorOrThrow()).ConfigureAwait(false);
	}
}
