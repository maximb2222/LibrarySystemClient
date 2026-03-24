using System.ServiceModel;

namespace LibrarySystem.BlazorClient.Services;

public static class ErrorHandler
{
    public static string HandleException(Exception ex)
    {
        return ex switch
        {
            FaultException fault => ParseFaultMessage(fault.Message),
            CommunicationException => "Ошибка соединения с сервером. Убедитесь, что сервис запущен.",
            TimeoutException => "Превышено время ожидания ответа от сервера.",
            _ => $"Непредвиденная ошибка: {ex.Message}"
        };
    }

    private static string ParseFaultMessage(string message)
    {
        if (message.Contains("Access denied", StringComparison.OrdinalIgnoreCase))
            return "Доступ запрещён. Недостаточно прав для выполнения операции.";
        if (message.Contains("Invalid credentials", StringComparison.OrdinalIgnoreCase))
            return "Неверное имя пользователя или пароль.";
        if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            return "Запрашиваемый ресурс не найден.";
        if (message.Contains("expired", StringComparison.OrdinalIgnoreCase))
            return "Срок действия токена истёк. Пожалуйста, войдите заново.";
        if (message.Contains("No available copies", StringComparison.OrdinalIgnoreCase))
            return "Нет доступных экземпляров книги.";
        return message;
    }
}
