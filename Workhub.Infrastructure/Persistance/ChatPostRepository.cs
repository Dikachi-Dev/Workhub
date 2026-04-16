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

    public async Task<ChatPost?> GetbySenderAndReciverId(string senderId, string receiverId)
    {
        var chat = await DbSet
            .Include(r => r.Replys.OrderByDescending(reply => reply.CreatedOn))
            .Where(r => (r.SenderId == senderId && r.ReceiverId == receiverId) || 
                       (r.SenderId == receiverId && r.ReceiverId == senderId))
            .FirstOrDefaultAsync();
        
        return chat;
    }

    public async Task<IList<ChatPost>> GetByUser(string userId)
    {
        var chats = await DbSet
            .Include(c => c.Replys)
            .Where(c => c.SenderId == userId || c.ReceiverId == userId)
            .OrderByDescending(c => c.UpdatedOn)
            .ToListAsync();
        
        return chats;
    }
}