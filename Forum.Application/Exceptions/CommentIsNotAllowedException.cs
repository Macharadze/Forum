namespace Forum.Application.Exceptions
{
    public class CommentIsNotAllowedException : Exception
    {
        public CommentIsNotAllowedException(string? message) : base("Comment section is disabled "+message)
        {
        }
    }
}
