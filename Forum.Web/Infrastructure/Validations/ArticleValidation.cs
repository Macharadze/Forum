using FluentValidation;
using Forum.Application.ArticleModel;

namespace Forum.Web.Infrastructure.Validations
{
    public class ArticleValidation : AbstractValidator<ArticleRequestModel>
    {
        public ArticleValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.");
        }  
    }
}

