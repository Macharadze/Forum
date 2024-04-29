using Forum.Application.AccountModel;

namespace Forum.Web.Models
{
    public class ProfileViewModel
    {
        public bool IsOwner { get; set; }
        public AccountResponseModel AccountResponseModel { get; set; }
    }
}
