
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
        var chat = DbSet
    .OrderBy(o => o.CreatedOn)
    .Where(r => r.SenderId == senderId && r.ReceiverId == receiverId)
    .Include(r => r.Replys.OrderByDescending(reply => reply.CreatedOn))
    .FirstOrDefault();
        // Retrieves the first matching entity or null if no matches exist
        return chat;
        // await DbSet
        //     .Include(r => r.Replys)
        //     .SingleOrDefaultAsync(r => r.SenderId == senderId && r.ReceiverId == receiverId)
    }

    public IEnumerable<ChatPost> GetByUser(string userId)
    {
        return DbSet
    .Where(c => c.SenderId == userId || c.ReceiverId == userId)
    .Include(c => c.Replys.OrderByDescending(reply => reply.CreatedOn))
    .OrderByDescending(c => c.Replys.FirstOrDefault().CreatedOn); // Assuming you want to order by the latest reply

    }
}