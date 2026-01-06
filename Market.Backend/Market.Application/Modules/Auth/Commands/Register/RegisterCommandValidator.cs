// Market.Application/Modules/Auth/Commands/Register/RegisterCommandValidator.cs
using FluentValidation;

namespace Market.Application.Modules.Auth.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno.")
            .MaximumLength(50).WithMessage("Ime može imati najviše 50 karaktera.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Prezime je obavezno.")
            .MaximumLength(50).WithMessage("Prezime može imati najviše 50 karaktera.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email je obavezan.")
            .EmailAddress().WithMessage("Unesite validnu email adresu.")
            .MaximumLength(256).WithMessage("Email može imati najviše 256 karaktera.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Lozinka je obavezna.")
            .MinimumLength(6).WithMessage("Lozinka mora imati najmanje 6 karaktera.")
            .Matches(@"[A-Z]+").WithMessage("Lozinka mora sadržati barem jedno veliko slovo.")
            .Matches(@"[a-z]+").WithMessage("Lozinka mora sadržati barem jedno malo slovo.")
            .Matches(@"[0-9]+").WithMessage("Lozinka mora sadržati barem jednu cifru.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Lozinke se moraju podudarati.");
    }
}