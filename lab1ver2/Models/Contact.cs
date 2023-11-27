using FluentValidation;

namespace lab1ver2.Models
{
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(4).MaximumLength(20).WithMessage("Valid name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is not valid.");
            RuleFor(x => x.Subject).NotNull().MinimumLength(4).MaximumLength(20).WithMessage("Valid subject is required.");
            RuleFor(x => x.Message).NotEmpty().MinimumLength(10).MaximumLength(150).WithMessage("Valid message is required.");
        }
    }
}
