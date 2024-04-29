using Forum.Domain.Enums;

namespace Forum.Domain.Base
{
    public abstract class BaseClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }

    }
}
