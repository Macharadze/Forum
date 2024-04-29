using FluentValidation;
using Forum.Application.CommentModel;

namespace Forum.Api.Infrastructure.Validations
{
    public class CommentValidation: AbstractValidator<CommentRequestModel>
    {
        public CommentValidation()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        }
    }
}
