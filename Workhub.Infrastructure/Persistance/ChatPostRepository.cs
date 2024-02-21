using System.Data.Entity;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ChatPostRepository : GenericRepository<ChatPost>, IChatPostRepository
{
    public ChatPostRepository(AppDataContext context, CancellationToken token) : base(context, token)
    {
    }

    public async Task<ChatPost> GetbySenderAndReciverId(string senderId, string receiverId, CancellationToken token)
    {
        return await DbSet.FirstOrDefaultAsync(r => r.SenderId == senderId && r.ReceiverId == receiverId, token);
    }

    public IEnumerable<ChatPost> GetByUser(string userId, CancellationToken token)
    {
        return DbSet.Where(c => c.SenderId == userId || c.ReceiverId == userId);
    }
}