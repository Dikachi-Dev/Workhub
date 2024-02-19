namespace Workhub.Contracts.Chat;

public record ChatRequest(string SenderId, string ReceiverId, string Message);
