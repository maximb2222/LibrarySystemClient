using LibrarySystem.Contracts.DTOs;

namespace LibrarySystem.BlazorClient.Services;

public class SessionService
{
    public AuthTokenDto? CurrentToken { get; private set; }
    public bool IsAuthenticated => CurrentToken != null && CurrentToken.ExpiresAt > DateTime.UtcNow;
    public string? Username => CurrentToken?.Username;
    public string? Role => CurrentToken?.Role;
    public string? Token => CurrentToken?.Token;

    public bool IsAdmin => Role == "Admin";
    public bool IsLibrarian => Role == "Librarian" || IsAdmin;

    public event Action? OnAuthStateChanged;

    public void SetToken(AuthTokenDto token)
    {
        CurrentToken = token;
        OnAuthStateChanged?.Invoke();
    }

    public void Logout()
    {
        CurrentToken = null;
        OnAuthStateChanged?.Invoke();
    }
}
