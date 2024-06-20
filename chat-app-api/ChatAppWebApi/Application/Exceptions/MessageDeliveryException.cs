namespace Application.Exceptions;

public class MessageDeliveryException : Exception
{
    public MessageDeliveryException()
    {
    }

    public MessageDeliveryException(string message)
        : base(message)
    {
    }

    public MessageDeliveryException(string message, Exception inner)
        : base(message, inner)
    {
    }
}