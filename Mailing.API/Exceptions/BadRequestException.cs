using System.Net;
using Auth.API;

namespace Mailing.API.Exceptions;

public class BadRequestException<T> : DomainException
{
    public BadRequestException() : base(
        ErrorCodes.BadRequestError, (int)HttpStatusCode.BadRequest)
    {
        PlaceholderData.Add("EntityName", typeof(T).Name);
    }
}
public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(
        message, (int)HttpStatusCode.BadRequest)
    {
    }
}