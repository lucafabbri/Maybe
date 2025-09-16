using System;
using System.Threading.Tasks;
using FluentAssertions;
using Maybe;
using Xunit;

namespace Maybe.UnitTest;

public class ElseErrorForwardingTests
{
    // Dummy error type used in tests. Must satisfy the "new()" constraint.
    private sealed class DummyError : BaseError
    {
        public string? Info { get; }
        public DummyError() { }
        public DummyError(string? info) => Info = info;
    }

    [Fact]
    public void Else_WithForwardedError_ReturnsSuccess_WhenSuccess()
    {
        var success = Maybe<int, DummyError>.Success(42);

        var result = success.Else(new DummyError("fwd"));

        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(42);
    }

    [Fact]
    public void Else_WithForwardedError_ReplacesError_WhenError()
    {
        var original = Maybe<int, DummyError>.Error(new DummyError("orig"));

        var fwd = new DummyError("fwd");
        var result = original.Else(fwd);

        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().BeSameAs(fwd);
    }

    [Fact]
    public void Else_WithValueFallback_ReturnsFallback_WhenError()
    {
        var original = Maybe<string, DummyError>.Error(new DummyError("orig"));

        var result = original.Else("fallback");

        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("fallback");
    }

    [Fact]
    public void Else_WithValueFunc_UsesFunc_WhenError_And_NotInvoked_WhenSuccess()
    {
        var invoked = false;
        Func<DummyError, string> func = e =>
        {
            invoked = true;
            return $"wrapped:" + e.Info;
        };

        var success = Maybe<string, DummyError>.Success("ok");
        success.Else(func).IsSuccess.Should().BeTrue();
        invoked.Should().BeFalse();

        invoked = false;
        var error = Maybe<string, DummyError>.Error(new DummyError("orig"));
        var result = error.Else(func);
        invoked.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("wrapped:orig");
    }

    [Fact]
    public void Else_WithErrorFunc_TransformsError_WhenError_And_NotInvoked_WhenSuccess()
    {
        var invoked = false;
        Func<DummyError, DummyError> func = e =>
        {
            invoked = true;
            return new DummyError($"x:" + e.Info);
        };

        var success = Maybe<int, DummyError>.Success(1);
        var s = success.Else(func);
        s.IsSuccess.Should().BeTrue();
        invoked.Should().BeFalse();

        invoked = false;
        var err = Maybe<int, DummyError>.Error(new DummyError("orig"));
        var r = err.Else(func);
        invoked.Should().BeTrue();
        r.IsError.Should().BeTrue();
        r.ErrorOrThrow().Info.Should().Be("x:orig");
    }

    [Fact]
    public void Else_ValueFunc_Null_Throws()
    {
        var err = Maybe<int, DummyError>.Error(new DummyError("e"));
        Action act = () => err.Else((Func<DummyError, int>)null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("fallbackFunc");
    }

    [Fact]
    public void Else_ErrorFunc_Null_Throws()
    {
        var err = Maybe<int, DummyError>.Error(new DummyError("e"));
        Action act = () => err.Else((Func<DummyError, DummyError>)null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("fallbackErrorFunc");
    }

    [Fact]
    public async Task Else_Task_WithForwardedError_ReplacesError_WhenError()
    {
        Task<Maybe<int, DummyError>> task = Task.FromResult(Maybe<int, DummyError>.Error(new DummyError("orig")));

        var forwarded = new DummyError("fwd");
        var result = await task.Else(forwarded);

        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().BeSameAs(forwarded);
    }

    [Fact]
    public async Task Else_Task_WithValueFunc_Works_WhenError_And_NotInvoked_WhenSuccess()
    {
        var invoked = false;
        Func<DummyError, string> func = e =>
        {
            invoked = true;
            return "val";
        };

        var sTask = Task.FromResult(Maybe<string, DummyError>.Success("ok"));
        (await sTask.Else(func)).IsSuccess.Should().BeTrue();
        invoked.Should().BeFalse();

        invoked = false;
        var eTask = Task.FromResult(Maybe<string, DummyError>.Error(new DummyError("e")));
        var r = await eTask.Else(func);
        invoked.Should().BeTrue();
        r.IsSuccess.Should().BeTrue();
        r.ValueOrThrow().Should().Be("val");
    }

    [Fact]
    public async Task ElseAsync_WithAsyncValueFunc_UsesTransformedValue_WhenError()
    {
        var maybe = Maybe<int, DummyError>.Error(new DummyError("orig"));

        var result = await maybe.ElseAsync(async e =>
        {
            await Task.Delay(1);
            return 99;
        });

        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(99);
    }

    [Fact]
    public async Task ElseAsync_WithAsyncErrorFunc_UsesTransformedError_WhenError()
    {
        var maybe = Maybe<int, DummyError>.Error(new DummyError("orig"));

        var result = await maybe.ElseAsync(async e =>
        {
            await Task.Delay(1);
            return new DummyError($"async:" + e.Info);
        });

        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Info.Should().Be("async:orig");
    }

    [Fact]
    public async Task ElseAsync_Task_WithAsyncErrorFunc_UsesTransformedError_WhenError()
    {
        var task = Task.FromResult(Maybe<int, DummyError>.Error(new DummyError("orig")));

        var result = await task.ElseAsync(async e =>
        {
            await Task.Delay(1);
            return new DummyError($"async:" + e.Info);
        });

        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Info.Should().Be("async:orig");
    }

    [Fact]
    public async Task ElseAsync_ValueFunc_Null_Throws()
    {
        var err = Maybe<int, DummyError>.Error(new DummyError("e"));
        Func<DummyError, Task<int>> f = null!;
        var act = async () => await err.ElseAsync(f);
        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("fallbackAsync");
    }

    [Fact]
    public async Task ElseAsync_ErrorFunc_Null_Throws()
    {
        var err = Maybe<int, DummyError>.Error(new DummyError("e"));
        Func<DummyError, Task<DummyError>> f = null!;
        var act = async () => await err.ElseAsync(f);
        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("forwardedErrorAsync");
    }
}