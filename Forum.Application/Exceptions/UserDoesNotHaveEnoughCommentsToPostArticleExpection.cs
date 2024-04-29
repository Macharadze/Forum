namespace Forum.Application.Exceptions
{
    public class UserDoesNotHaveEnoughCommentsToPostArticleExpection : Exception
    {
        public UserDoesNotHaveEnoughCommentsToPostArticleExpection(string? message) : base("User does not have enough comments to post article " + message)
        {
        }
    }
}
