using System.Runtime.Serialization;

namespace AltenBlog.Common.Infrastructore.Exceptions;

public class DatabaseValidationException : Exception
{
    public DatabaseValidationException()
    {
    }

    public DatabaseValidationException(string? message) : base(message)
    {
    }

    public DatabaseValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DatabaseValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}