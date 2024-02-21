using Workhub.Domain.Entities;

namespace Workhub.Contracts.Chat;

public record ChatByUserIdResponse(IEnumerable<ChatPost> chatPosts);
