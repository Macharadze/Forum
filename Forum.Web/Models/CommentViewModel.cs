namespace Forum.Web.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
    }
}
