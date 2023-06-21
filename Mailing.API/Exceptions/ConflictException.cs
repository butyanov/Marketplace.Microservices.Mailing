using System.Net;

namespace Mailing.API.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}