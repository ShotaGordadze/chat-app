using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace Application.RabbitMQ;

public interface IMessageService
{
    void SendMessage(string message, string queueName, out bool sentSuccessfully);
    void DeclareQueue(string queueName);
}

public class MessageService : IMessageService
{
    private readonly RabbitMQPersistentConnection _persistentConnection;
    private readonly IModel _channel;

    public MessageService(RabbitMQPersistentConnection persistentConnection)
    {
        _persistentConnection = persistentConnection;
        _channel = _persistentConnection.GetConnection().CreateModel();
    }

    public void DeclareQueue(string queueName)
    {
        _channel.QueueDeclare(queue: queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void SendMessage(string message, string queueName, out bool sentSuccessfully)
    {
        _channel.ConfirmSelect();

        byte[] body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        if (!_channel.WaitForConfirms())
            sentSuccessfully = false;
        
        else
            sentSuccessfully = true;
    }
}
