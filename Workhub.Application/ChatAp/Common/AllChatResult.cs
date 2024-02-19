using Workhub.Domain.Entities;

namespace Workhub.Application.ChatAp.Common;

public record AllChatResult(IEnumerable<ChatPost> ChatPosts);

