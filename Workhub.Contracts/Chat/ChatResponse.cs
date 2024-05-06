namespace Workhub.Contracts.Chat;

public record ChatResponse(string SenderId,
 string Id,
DateTime CreatedOn,
string ReceiverId,
string ReceiverName,
string SenderName,
 IList<Replyyy> Replys);

public record Replyyy(string Id, DateTime CreatedOn, string Message, string FromId);
