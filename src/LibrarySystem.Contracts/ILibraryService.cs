using System.ServiceModel;
using LibrarySystem.Contracts.DTOs;

namespace LibrarySystem.Contracts;

[ServiceContract(Namespace = "http://library.example.com/services")]
public interface ILibraryService
{
    [OperationContract]
    AuthTokenDto Authenticate(string username, string password);

    [OperationContract]
    List<BookDto> GetAllBooks(string token);

    [OperationContract]
    BookDto GetBookById(string token, int id);

    [OperationContract]
    List<BookDto> SearchBooks(string token, SearchCriteriaDto criteria);

    [OperationContract]
    BookDto AddBook(string token, BookDto book);

    [OperationContract]
    void UpdateBook(string token, BookDto book);

    [OperationContract]
    void DeleteBook(string token, int id);

    [OperationContract]
    List<ReaderDto> GetAllReaders(string token);

    [OperationContract]
    ReaderDto GetReaderById(string token, int id);

    [OperationContract]
    ReaderDto RegisterReader(string token, ReaderDto reader);

    [OperationContract]
    LoanDto LendBook(string token, int bookId, int readerId);

    [OperationContract]
    LoanDto ReturnBook(string token, int loanId);

    [OperationContract]
    List<LoanDto> GetLoansByReader(string token, int readerId);

    [OperationContract]
    List<LoanDto> GetOverdueLoans(string token);

    [OperationContract]
    LibraryStatsDto GetStatistics(string token);
}
