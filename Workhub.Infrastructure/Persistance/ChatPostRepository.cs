using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ChatPostRepository : GenericRepository<ChatPost>, IChatPostRepository
{
    public ChatPostRepository(AppDataContext context) : base(context)
    {
    }

    public IEnumerable<ChatPost> GetByUser(string userId)
    {
        return GetAll().Where(c => c.SenderId == userId || c.ReceiverId == userId);
    }
}