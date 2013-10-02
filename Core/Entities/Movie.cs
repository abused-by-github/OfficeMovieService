namespace Svitla.MovieService.Core.Entities
{
    public class Movie : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public User User { get; set; }
    }
}
