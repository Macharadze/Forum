namespace Forum.Application.Exceptions
{

    public class UserDoesnotExistException : Exception
    {
        public UserDoesnotExistException(string? message) : base("User does not exist "+message)
        {
        }
    }
}
