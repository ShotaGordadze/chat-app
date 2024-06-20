using Infrastructure.Database.Entities;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Application.RabbitMQ;

public interface IMessageService
{
    void SendMessage(string message, string queueName, out bool sentSuccessfully);
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

    public Message ConsumeMessage(string queueName)
    {
        var consumer = new EventingBasicConsumer(_channel);
        string message = string.Empty;
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
        };

        _channel.BasicConsume(queue: queueName,
                              autoAck: true,
                              consumer: consumer);

        return new Message
        {
            Context = message,
            Username = "Not set yet"
        };
    }
}
