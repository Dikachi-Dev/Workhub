using Workhub.Domain.Entities;

namespace Workhub.Application.Interfaces.Persistance;

public interface IChatPostRepository : IGenericRepository<ChatPost>
{
    IEnumerable<ChatPost> GetByUser(string userId);

}
