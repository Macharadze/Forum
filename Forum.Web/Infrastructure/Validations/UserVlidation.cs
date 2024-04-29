using FluentValidation;
using Forum.Application.AccountModel;

namespace Forum.Web.Infrastructure.Validations
{
    public class UserVlidation : AbstractValidator<RegisterModel>
    {
        public UserVlidation()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.")
                .Matches("[0-9]{9}").WithMessage("phone must contain a fixed-length number with 9 digits.");
            ;
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
        }

      
    }
}
