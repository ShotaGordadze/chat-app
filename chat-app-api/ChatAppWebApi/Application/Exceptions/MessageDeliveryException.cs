using Microsoft.EntityFrameworkCore.Query.Internal;

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

public class UserAlreadyExistsException : Exception
{
    public string Email { get; }

    public UserAlreadyExistsException() { }

    public UserAlreadyExistsException(string message)
        : base(message) { }

    public UserAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }


    public UserAlreadyExistsException(string message, string email)
        : this(message)
    {
        Email = email;
    }
}