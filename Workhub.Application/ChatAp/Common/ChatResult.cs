using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Common;

public record ChatResult(string SenderId,
 string Id,
DateTime CreatedOn,
string ReceiverId,
string ReceiverName,
string SenderName,
 ICollection<Replyy> Replys);


public record Replyy(string Id, DateTime CreatedOn, string Message, string FromId);