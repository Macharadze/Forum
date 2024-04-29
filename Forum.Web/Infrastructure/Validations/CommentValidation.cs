using FluentValidation;
using Forum.Application.CommentModel;

namespace Forum.Web.Infrastructure.Validations
{
    public class CommentValidation: AbstractValidator<CommentRequestModel>
    {
        public CommentValidation()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        }
    }
}
