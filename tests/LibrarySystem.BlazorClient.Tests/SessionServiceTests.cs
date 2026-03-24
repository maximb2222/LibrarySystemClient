using Xunit;
using LibrarySystem.BlazorClient.Services;
using LibrarySystem.Contracts.DTOs;

namespace LibrarySystem.BlazorClient.Tests;

public class SessionServiceTests
{
    [Fact]
    public void NewSession_IsNotAuthenticated()
    {
        var session = new SessionService();

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.Username);
        Assert.Null(session.Role);
        Assert.Null(session.Token);
    }

    [Fact]
    public void SetToken_MakesSessionAuthenticated()
    {
        var session = new SessionService();
        var token = new AuthTokenDto
        {
            Token = "test-token",
            Username = "admin",
            Role = "Admin",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };

        session.SetToken(token);

        Assert.True(session.IsAuthenticated);
        Assert.Equal("admin", session.Username);
        Assert.Equal("Admin", session.Role);
        Assert.Equal("test-token", session.Token);
    }

    [Fact]
    public void ExpiredToken_IsNotAuthenticated()
    {
        var session = new SessionService();
        var token = new AuthTokenDto
        {
            Token = "expired-token",
            Username = "user",
            Role = "Reader",
            ExpiresAt = DateTime.UtcNow.AddHours(-1)
        };

        session.SetToken(token);

        Assert.False(session.IsAuthenticated);
    }

    [Fact]
    public void Logout_ClearsSession()
    {
        var session = new SessionService();
        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "u", Role = "Admin",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        session.Logout();

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.Token);
    }

    [Fact]
    public void IsAdmin_ReturnsTrueForAdmin()
    {
        var session = new SessionService();
        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "admin", Role = "Admin",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        Assert.True(session.IsAdmin);
        Assert.True(session.IsLibrarian);
    }

    [Fact]
    public void IsLibrarian_ReturnsTrueForLibrarian()
    {
        var session = new SessionService();
        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "lib", Role = "Librarian",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        Assert.False(session.IsAdmin);
        Assert.True(session.IsLibrarian);
    }

    [Fact]
    public void Reader_HasNoLibrarianAccess()
    {
        var session = new SessionService();
        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "reader", Role = "Reader",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        Assert.False(session.IsAdmin);
        Assert.False(session.IsLibrarian);
    }

    [Fact]
    public void SetToken_RaisesAuthStateChanged()
    {
        var session = new SessionService();
        bool eventFired = false;
        session.OnAuthStateChanged += () => eventFired = true;

        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "u", Role = "Admin",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        Assert.True(eventFired);
    }

    [Fact]
    public void Logout_RaisesAuthStateChanged()
    {
        var session = new SessionService();
        session.SetToken(new AuthTokenDto
        {
            Token = "t", Username = "u", Role = "Admin",
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        });

        bool eventFired = false;
        session.OnAuthStateChanged += () => eventFired = true;

        session.Logout();

        Assert.True(eventFired);
    }
}
