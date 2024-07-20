using Application.Exceptions;
using Application.RabbitMQ;
using Infrastructure;
using Infrastructure.Database.Entities;
using Infrastructure.Repositories;
using MediatR;

namespace Application.NewFolder
{
    public record SendMessageCommand(string Message) : IRequest<Message>;

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Message>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageService _messageService;

        public SendMessageCommandHandler(IUnitOfWork unitOfWork, IMessageRepository messageRepository, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageRepository = messageRepository;
            _messageService = messageService;
        }

        public async Task<Message> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            string queueName = "chat-queue";

            _messageService.DeclareQueue(queueName);
            _messageService.SendMessage(request.Message, queueName, out bool sentSuccessfully);

            if (sentSuccessfully)
            {
                Message message = new Message
                {
                    Context = request.Message,
                    Username = "Not Set Yet"
                };

                await _messageRepository.Store(message);

                await _unitOfWork.SaveAsync(cancellationToken);

                return message;
            }

            throw new MessageDeliveryException("Couldn't send a message");
        }
    }
}
