namespace Workhub.Contracts.Chat;

public record ChatResponse(string SenderId,
 string Id,
DateTime CreatedOn,
string ReceiverId,
string ReceiverName,
string SenderName,
 IList<ReplyDto> Replys);

public record ReplyDto(string Id, DateTime CreatedOn, string Message, string FromId);
