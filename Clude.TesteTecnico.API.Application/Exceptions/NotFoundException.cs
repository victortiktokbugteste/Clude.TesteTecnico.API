using FluentValidation;
using FluentValidation.Results;

namespace Clude.TesteTecnico.API.Application.Exceptions
{
    public class NotFoundException : ValidationException
    {
        public NotFoundException(string message) 
            : base(new[] { new ValidationFailure("", message) })
        {
        }

        public NotFoundException(string propertyName, string message) 
            : base(new[] { new ValidationFailure(propertyName, message) })
        {
        }
    }
} 