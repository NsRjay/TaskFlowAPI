using System.Data;
using FluentValidation;
using TaskFlowAPI.DTOs;
namespace TaskFlowAPI.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x=>x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Username must be atleast 3 characters");
            RuleFor(x=>x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 charcaters");
        }
    }
}