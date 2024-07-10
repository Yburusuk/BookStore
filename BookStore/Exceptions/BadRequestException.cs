using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStore.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(ModelStateDictionary modelstate)
    {
    }
}