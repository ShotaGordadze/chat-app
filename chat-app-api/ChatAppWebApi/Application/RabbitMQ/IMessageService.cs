using Infrastructure.Database.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Application.RabbitMQ;

public interface IMessageService
{
    void SendMessage(string message, string username, string queueName, out bool sentSuccessfully);
    Message ConsumeMessage(string queueName);
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

    public void SendMessage(string message, string username, string queueName, out bool sentSuccessfully)
    {
        _channel.ConfirmSelect();

        var properties = _channel.CreateBasicProperties();
        properties.Headers = new Dictionary<string, object>()
        {
            { "username", username }
        };

        byte[] body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: properties,
                             body: body);

        if (!_channel.WaitForConfirms())
            sentSuccessfully = false;

        else
            sentSuccessfully = true;
    }

    public Message ConsumeMessage(string queueName)
    {
        var consumer = new EventingBasicConsumer(_channel);
        string message = string.Empty;
        string username = string.Empty;
        var messageReceivedEvent = new AutoResetEvent(false);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
            if (ea.BasicProperties.Headers.TryGetValue("username", out object usernameObj))
            {
                byte[] usernameBytes = (byte[])usernameObj;
                username = Encoding.UTF8.GetString(usernameBytes);
            }
            else
            {
                username = "unknown";
            }
            messageReceivedEvent.Set();
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        messageReceivedEvent.WaitOne();

        return new Message
        {
            Context = message,
            Username = username,
        };
    }
}
