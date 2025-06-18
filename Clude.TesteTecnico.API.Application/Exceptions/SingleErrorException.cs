using FluentValidation;
using FluentValidation.Results;

namespace Clude.TesteTecnico.API.Application.Exceptions
{
    public class SingleErrorException : ValidationException
    {
        public SingleErrorException(string message) 
            : base(new[] { new ValidationFailure("", message) })
        {
        }

        public SingleErrorException(string propertyName, string message) 
            : base(new[] { new ValidationFailure(propertyName, message) })
        {
        }
    }
} 