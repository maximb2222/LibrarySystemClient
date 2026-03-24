using System.ServiceModel;
using LibrarySystem.Contracts;
using LibrarySystem.Contracts.DTOs;

namespace LibrarySystem.BlazorClient.Services;

public class LibraryApiClient : IDisposable
{
    private readonly ChannelFactory<ILibraryService> _httpFactory;
    private readonly ChannelFactory<ILibraryService> _tcpFactory;
    private ILibraryService? _httpChannel;
    private ILibraryService? _tcpChannel;

    public LibraryApiClient(string httpUrl, string tcpUrl)
    {
        _httpFactory = new ChannelFactory<ILibraryService>(
            new BasicHttpBinding { MaxReceivedMessageSize = 65536000 },
            new EndpointAddress(httpUrl));

        _tcpFactory = new ChannelFactory<ILibraryService>(
            new NetTcpBinding(SecurityMode.None) { MaxReceivedMessageSize = 65536000 },
            new EndpointAddress(tcpUrl));
    }

    private ILibraryService Http => _httpChannel ??= _httpFactory.CreateChannel();
    private ILibraryService Tcp => _tcpChannel ??= _tcpFactory.CreateChannel();

    private ILibraryService GetChannel(bool useTcp) => useTcp ? Tcp : Http;

    // === Authentication ===
    public async Task<AuthTokenDto> AuthenticateAsync(string username, string password, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).Authenticate(username, password));
    }

    // === Books ===
    public async Task<List<BookDto>> GetAllBooksAsync(string token, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetAllBooks(token));
    }

    public async Task<BookDto> GetBookByIdAsync(string token, int id, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetBookById(token, id));
    }

    public async Task<List<BookDto>> SearchBooksAsync(string token, SearchCriteriaDto criteria, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).SearchBooks(token, criteria));
    }

    public async Task<BookDto> AddBookAsync(string token, BookDto book, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).AddBook(token, book));
    }

    public async Task UpdateBookAsync(string token, BookDto book, bool useTcp = false)
    {
        await Task.Run(() => GetChannel(useTcp).UpdateBook(token, book));
    }

    public async Task DeleteBookAsync(string token, int id, bool useTcp = false)
    {
        await Task.Run(() => GetChannel(useTcp).DeleteBook(token, id));
    }

    // === Readers ===
    public async Task<List<ReaderDto>> GetAllReadersAsync(string token, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetAllReaders(token));
    }

    public async Task<ReaderDto> GetReaderByIdAsync(string token, int id, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetReaderById(token, id));
    }

    public async Task<ReaderDto> RegisterReaderAsync(string token, ReaderDto reader, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).RegisterReader(token, reader));
    }

    // === Loans ===
    public async Task<LoanDto> LendBookAsync(string token, int bookId, int readerId, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).LendBook(token, bookId, readerId));
    }

    public async Task<LoanDto> ReturnBookAsync(string token, int loanId, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).ReturnBook(token, loanId));
    }

    public async Task<List<LoanDto>> GetLoansByReaderAsync(string token, int readerId, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetLoansByReader(token, readerId));
    }

    public async Task<List<LoanDto>> GetOverdueLoansAsync(string token, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetOverdueLoans(token));
    }

    // === Statistics ===
    public async Task<LibraryStatsDto> GetStatisticsAsync(string token, bool useTcp = false)
    {
        return await Task.Run(() => GetChannel(useTcp).GetStatistics(token));
    }

    public void Dispose()
    {
        try { ((IClientChannel?)_httpChannel)?.Close(); } catch { ((IClientChannel?)_httpChannel)?.Abort(); }
        try { ((IClientChannel?)_tcpChannel)?.Close(); } catch { ((IClientChannel?)_tcpChannel)?.Abort(); }
        try { _httpFactory.Close(); } catch { _httpFactory.Abort(); }
        try { _tcpFactory.Close(); } catch { _tcpFactory.Abort(); }
    }
}
