using Xunit;
using System.ServiceModel;
using LibrarySystem.BlazorClient.Services;

namespace LibrarySystem.BlazorClient.Tests;

public class ErrorHandlerTests
{
    [Fact]
    public void FaultException_AccessDenied_ReturnsRussianMessage()
    {
        var ex = new FaultException("Access denied. Required: Admin");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("Доступ запрещён", result);
    }

    [Fact]
    public void FaultException_InvalidCredentials_ReturnsRussianMessage()
    {
        var ex = new FaultException("Invalid credentials.");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("Неверное имя пользователя", result);
    }

    [Fact]
    public void FaultException_NotFound_ReturnsRussianMessage()
    {
        var ex = new FaultException("Book not found.");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("не найден", result);
    }

    [Fact]
    public void FaultException_Expired_ReturnsRussianMessage()
    {
        var ex = new FaultException("Token expired.");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("истёк", result);
    }

    [Fact]
    public void FaultException_NoAvailableCopies_ReturnsMessage()
    {
        var ex = new FaultException("No available copies for this book.");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("Нет доступных экземпляров", result);
    }

    [Fact]
    public void FaultException_UnknownMessage_ReturnsOriginal()
    {
        var ex = new FaultException("Some custom error message");
        var result = ErrorHandler.HandleException(ex);
        Assert.Equal("Some custom error message", result);
    }

    [Fact]
    public void CommunicationException_ReturnsConnectionError()
    {
        var ex = new CommunicationException("Connection refused");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("соединения", result);
    }

    [Fact]
    public void TimeoutException_ReturnsTimeoutMessage()
    {
        var ex = new TimeoutException("Request timed out");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("ожидания", result);
    }

    [Fact]
    public void GenericException_ReturnsGenericMessage()
    {
        var ex = new InvalidOperationException("Something went wrong");
        var result = ErrorHandler.HandleException(ex);
        Assert.Contains("Something went wrong", result);
    }
}
