namespace Forum.Application.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string? message) : base("User Already Exist " + message)
        {
        }
    }
}
