using Forum.Domain.Accounts;

namespace Forum.Domain.Bans
{
    public class AccountBan
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public string AccountID { get; set; }
        
    }
}
