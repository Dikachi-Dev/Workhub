using Workhub.Domain.Entities;

namespace Workhub.Contracts.Chat;

public record ChatResponse(string SenderId,
 string Id,
DateTime CreatedOn,
string ReceiverId,
 ICollection<Replyyy> Replys);

 public record Replyyy( string Id, DateTime CreatedOn, string Message, string FromId);
