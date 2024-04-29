namespace Forum.Application.Exceptions
{
    public class UserIsBannedException : Exception
    {
        public UserIsBannedException(string? message) : base("user is banned "+message)
        {
        }
    }
}
