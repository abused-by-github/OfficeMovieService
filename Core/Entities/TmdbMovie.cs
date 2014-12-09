namespace MovieService.Core.Entities
{
    /// <summary>
    /// An entry from http://www.themoviedb.org DB.
    /// </summary>
    public class TmdbMovie : Entity
    {
        private const string ImageUrlFormat = "{0}w500{1}";

        public long TmdbId { get; set; }

        public string PosterPath { get; set; }

        public string GetImageUrl(string baseUrl)
        {
            return string.Format(ImageUrlFormat, baseUrl, PosterPath);
        }
    }
}
