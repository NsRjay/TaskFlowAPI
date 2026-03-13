using System.Data;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using TaskFlowAPI.DTOs;
namespace TaskFlowAPI.Validators
{
    public class TaskCreateDTOValidator : AbstractValidator<TaskCreateDTO>
    {
        public TaskCreateDTOValidator()
        {
            RuleFor(x=>x.Title)
            .NotEmpty()
            .WithMessage("Task title is required")
            .MaximumLength(500);
            RuleFor(x=>x.Description).MaximumLength(500);
            
        }
    }
}