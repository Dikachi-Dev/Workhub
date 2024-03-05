
using Microsoft.EntityFrameworkCore;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Infrastructure.Persistance;

public class ChatPostRepository : GenericRepository<ChatPost>, IChatPostRepository
{
    public ChatPostRepository(AppDataContext context) : base(context)
    {
    }

    public async Task<ChatPost> GetbySenderAndReciverId(string senderId, string receiverId)
    {
        return await DbSet
            .Include(r => r.Replys)
            .SingleOrDefaultAsync(r => r.SenderId == senderId && r.ReceiverId == receiverId);
    }

    public IEnumerable<ChatPost> GetByUser(string userId)
    {
        return DbSet.Where(c => c.SenderId == userId || c.ReceiverId == userId);
    }
}