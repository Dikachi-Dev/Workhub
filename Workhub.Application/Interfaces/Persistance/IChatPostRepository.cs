using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IChatPostRepository : IGenericRepository<ChatPost>
{
    Task<IList<ChatPost>> GetByUser(string userId);
    Task<ChatPost?> GetbySenderAndReciverId(string senderId, string receiverId);

}
