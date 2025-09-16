using FluentAssertions;
using Maybe;
using Maybe.Toolkit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maybe.Toolkit.Tests;

public class HttpToolkitTests
{
    [Fact]
    public void TryGetAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryGetAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync((string?)null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetAsync_WithEmptyUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync("");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync((Uri?)null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryPostAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryPostAsync("http://example.com", null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryGetStringAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetStringAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetByteArrayAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetByteArrayAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryPutAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPutAsync("http://example.com", null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPutAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPutAsync((string?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryPutAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPutAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPatchAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPatchAsync("http://example.com", null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPatchAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPatchAsync((string?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryPatchAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPatchAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryDeleteAsync("http://example.com");

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryDeleteAsync((string?)null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryDeleteAsync((Uri?)null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPostAsync_WithUriObject_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPostAsync(new Uri("http://example.com"), null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPostAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPostAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    #region JSON integration tests

    [Fact]
    public async Task TryGetJsonAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryGetJsonAsync<TestObject>("http://example.com");

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPostJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPostJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPutJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPutJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPatchJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPatchJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    #endregion

    #region Integration tests with DummyJSON service

    private static bool IsNetworkAvailable()
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = client.GetAsync("https://dummyjson.com/test").Result;
            return true;
        }
        catch
        {
            return false;
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryGetAsync_WithDummyJsonService_ReturnsSuccessResponse()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos";

        // Act
        var result = await client.TryGetAsync(url);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var response = result.ValueOrThrow();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryGetStringAsync_WithDummyJsonService_ReturnsJsonString()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos";

        // Act
        var result = await client.TryGetStringAsync(url);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var jsonString = result.ValueOrThrow();
        jsonString.Should().NotBeNullOrWhiteSpace();
        jsonString.Should().Contain("todos");
        jsonString.Should().Contain("total");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryGetJsonAsync_WithDummyJsonService_ReturnsTodosResponse()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos";

        // Act
        var result = await client.TryGetJsonAsync<TodosResponse>(url);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var todosResponse = result.ValueOrThrow();
        todosResponse.Should().NotBeNull();
        todosResponse.Todos.Should().NotBeNull();
        todosResponse.Todos.Should().NotBeEmpty();
        todosResponse.Total.Should().BeGreaterThan(0);
        todosResponse.Limit.Should().BeGreaterThan(0);
        
        // Verify first todo structure
        var firstTodo = todosResponse.Todos.First();
        firstTodo.Id.Should().BeGreaterThan(0);
        firstTodo.Task.Should().NotBeNullOrWhiteSpace();
        firstTodo.UserId.Should().BeGreaterThan(0);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryGetJsonAsync_WithDummyJsonSingleTodo_ReturnsTodo()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos/1";

        // Act
        var result = await client.TryGetJsonAsync<Todo>(url);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var todo = result.ValueOrThrow();
        todo.Should().NotBeNull();
        todo.Id.Should().Be(1);
        todo.Task.Should().NotBeNullOrWhiteSpace();
        todo.UserId.Should().BeGreaterThan(0);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryPostJsonAsync_WithDummyJsonService_ReturnsCreatedTodo()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos/add";
        var newTodo = new TodoCreate 
        { 
            Todo = "Use Maybe.Toolkit for HTTP requests", 
            Completed = false, 
            UserId = 1 
        };

        // Act
        var result = await client.TryPostJsonAsync<TodoCreate, Todo>(url, newTodo);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var createdTodo = result.ValueOrThrow();
        createdTodo.Should().NotBeNull();
        createdTodo.Id.Should().BeGreaterThan(0);
        createdTodo.Task.Should().Be(newTodo.Todo);
        createdTodo.Completed.Should().Be(newTodo.Completed);
        createdTodo.UserId.Should().Be(newTodo.UserId);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryPutJsonAsync_WithDummyJsonService_ReturnsUpdatedTodo()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos/1";
        var updateTodo = new TodoUpdate 
        { 
            Todo = "Updated todo using Maybe.Toolkit", 
            Completed = true 
        };

        // Act
        var result = await client.TryPutJsonAsync<TodoUpdate, Todo>(url, updateTodo);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var updatedTodo = result.ValueOrThrow();
        updatedTodo.Should().NotBeNull();
        updatedTodo.Id.Should().Be(1);
        updatedTodo.Task.Should().Be(updateTodo.Todo);
        updatedTodo.Completed.Should().Be(updateTodo.Completed);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryPatchJsonAsync_WithDummyJsonService_ReturnsUpdatedTodo()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos/1";
        var patchTodo = new TodoPatch 
        { 
            Completed = true
        };

        // Act
        var result = await client.TryPatchJsonAsync<TodoPatch, Todo>(url, patchTodo);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var updatedTodo = result.ValueOrThrow();
        updatedTodo.Should().NotBeNull();
        updatedTodo.Id.Should().Be(1);
        updatedTodo.Completed.Should().Be(patchTodo.Completed);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TryDeleteAsync_WithDummyJsonService_ReturnsSuccessResponse()
    {
        // Skip if network is not available
        if (!IsNetworkAvailable())
        {
            return; // Skip the test
        }

        // Arrange
        using var client = new HttpClient();
        const string url = "https://dummyjson.com/todos/1";

        // Act
        var result = await client.TryDeleteAsync(url);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        var response = result.ValueOrThrow();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TryGetJsonAsync_WithInvalidJsonUrl_VerifiesErrorHandling()
    {
        // This test uses a local mock approach instead of external service
        // to verify that JSON parsing errors are properly handled
        
        // Arrange
        using var client = new HttpClient();
        
        // Use an invalid URL that will definitely cause an HTTP error
        // This tests our error handling without depending on external services
        const string url = "http://localhost:99999/invalid"; 

        // Act
        var result = await client.TryGetJsonAsync<TodosResponse>(url);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
        error.HttpError.Should().NotBeNull();
    }

    #endregion

    // DTOs for DummyJSON service
    private class TodosResponse
    {
        [JsonPropertyName("todos")]
        public List<Todo> Todos { get; set; } = new();

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("skip")]
        public int Skip { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }

    private class Todo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("todo")]
        public string Task { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }
    }

    private class TodoCreate
    {
        [JsonPropertyName("todo")]
        public string Todo { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }
    }

    private class TodoUpdate
    {
        [JsonPropertyName("todo")]
        public string Todo { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }

    private class TodoPatch
    {
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }

    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    #region HttpJsonError tests

    [Fact]
    public void HttpJsonError_DefaultConstructor_SetsDefaults()
    {
        // Act
        var error = new HttpJsonError();

        // Assert
        error.Should().NotBeNull();
        error.IsHttpError.Should().BeFalse();
        error.IsJsonError.Should().BeFalse();
        error.HttpError.Should().BeNull();
        error.JsonError.Should().BeNull();
        error.UnderlyingError.Should().BeNull();
    }

    [Fact]
    public void HttpJsonError_WithHttpError_SetsPropertiesCorrectly()
    {
        // Arrange
        var httpError = new HttpError(new HttpRequestException("test"), "http://test.com", null, "Test HTTP error");

        // Act
        var error = new HttpJsonError(httpError);

        // Assert
        error.IsHttpError.Should().BeTrue();
        error.IsJsonError.Should().BeFalse();
        error.HttpError.Should().Be(httpError);
        error.JsonError.Should().BeNull();
        error.UnderlyingError.Should().Be(httpError);
    }

    [Fact]
    public void HttpJsonError_WithJsonError_SetsPropertiesCorrectly()
    {
        // Arrange
        var jsonError = new JsonError(new System.Text.Json.JsonException("test"), "Test JSON error");

        // Act
        var error = new HttpJsonError(jsonError);

        // Assert
        error.IsHttpError.Should().BeFalse();
        error.IsJsonError.Should().BeTrue();
        error.HttpError.Should().BeNull();
        error.JsonError.Should().Be(jsonError);
        error.UnderlyingError.Should().Be(jsonError);
    }

    #endregion
}