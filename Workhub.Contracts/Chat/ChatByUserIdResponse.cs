using Workhub.Domain.Entities;

namespace Workhub.Contracts.Chat;

public record ChatByUserIdResponse(IList<ChatPost> chatPosts);
