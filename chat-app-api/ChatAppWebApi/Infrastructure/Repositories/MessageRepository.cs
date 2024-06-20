using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using SHG.Infrastructure.Repositories.Abstraction;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface IMessageRepository : IRepository<Message>;

public class MessageRepository : IMessageRepository
{
    private readonly MessagesDbContext _messagesDbContext;

    public MessageRepository(MessagesDbContext dbContext)
    {
        _messagesDbContext = dbContext;
    }

    public void Delete(Message document)
    {
       _messagesDbContext.Remove(document);
    }

    public async Task<Message> Find(int id)
    {
        return await _messagesDbContext.Messages.FirstAsync(m => m.Id == id);
    }

    public IQueryable<Message> Query(Expression<Func<Message, bool>>? expression = null)
    {
        return expression != null ?
            _messagesDbContext.Messages.Where(expression) :
            _messagesDbContext.Messages.AsQueryable();
    }

    public async Task Store(Message document)
    {
        await _messagesDbContext.Messages.AddAsync(document);
    }
}
