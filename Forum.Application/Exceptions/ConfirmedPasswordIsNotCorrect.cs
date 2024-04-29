namespace Forum.Application.Exceptions
{
    public class ConfirmedPasswordIsNotCorrect : Exception
    {
        public ConfirmedPasswordIsNotCorrect(string? message) : base("confirmed password is not correct,try again "+message)
        {
        }
    }
}
