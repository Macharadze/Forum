namespace Forum.Api.Models
{
    public class ChangePasswordModel
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
