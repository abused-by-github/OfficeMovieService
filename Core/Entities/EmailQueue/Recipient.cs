namespace MovieService.Core.Entities.EmailQueue
{
    public class Recipient : Entity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public RecipientRole Role { get; set; }
    }
}
