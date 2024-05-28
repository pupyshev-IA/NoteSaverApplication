using Abdt.Loyal.NoteSaver.Domain;
using FluentValidation;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Validation
{
    public class Validator : AbstractValidator<Note>
    {
        public Validator()
        {
            RuleFor(note => note.Title).NotEmpty().MaximumLength(50);
            RuleFor(note => note.Content).MaximumLength(2000);
        }
    }
}
