using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Application.RabbitMQ;

public class RabbitMQPersistentConnection
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private readonly object _lock = new object();

    public RabbitMQPersistentConnection(IConfiguration config)
    {
        _connectionFactory = new ConnectionFactory()
        {
            HostName = config["MessageFactory:HostName"],
            Port = int.Parse(config["MessageFactory:Port"]!),
            UserName = config["MessageFactory:UserName"],
            Password = config["MessageFactory:Password"]
        };
    }

    public IConnection GetConnection()
    {
        lock (_lock)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
            }
        }

        return _connection;
    }
}