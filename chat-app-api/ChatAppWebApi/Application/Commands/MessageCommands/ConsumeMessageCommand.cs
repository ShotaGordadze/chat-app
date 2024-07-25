using Application.RabbitMQ;
using Infrastructure;
using Infrastructure.Database.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Commands;

public record ConsumeMessageCommand() : IRequest<Message>;

public class ConsumeMessageCommandHandler : IRequestHandler<ConsumeMessageCommand, Message>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageService _messageService;

    public ConsumeMessageCommandHandler(IUnitOfWork unitOfWork, IMessageService messageService, IMessageRepository messageRepository)
    {
        _unitOfWork = unitOfWork;
        _messageService = messageService;
        _messageRepository = messageRepository;
    }

    public async Task<Message> Handle(ConsumeMessageCommand request, CancellationToken cancellationToken)
    {
        string queueName = "chat-queue";

        _messageService.DeclareQueue(queueName);
        var message = _messageService.ConsumeMessage(queueName);

        await _messageRepository.Store(message);

        await _unitOfWork.SaveAsync(cancellationToken);

        return message;
    }
}
