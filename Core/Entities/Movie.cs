using System;
using System.Collections.Generic;
using MovieService.Core.Entities.Security;
using MovieService.Core.Exceptions;
using MovieService.Core.Helpers;
using MovieService.Core.Logging;

namespace MovieService.Core.Entities
{
    public class Movie : Entity, IModifiable
    {
        public string Name { get; set; }

        public long? TmdbMovieId { get; set; }
        public virtual TmdbMovie TmdbMovie { get; set; }

        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = NormalizeUrl(value);
            }
        }

        public string GetImageImageUrl(string baseTmdbUrl)
        {
            return CustomImageUrl ?? TmdbMovie.Get(m => m.GetImageUrl(baseTmdbUrl));
        }

        private string customImageUrl;
        public string CustomImageUrl
        {
            get
            {
                return customImageUrl;
            }
            set
            {
                customImageUrl = NormalizeUrl(value);
            }
        }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        [Log(Verbosity.Full)]
        public virtual User User { get; set; }

        [Log(Verbosity.Full)]
        public virtual ICollection<Vote> Votes { get; set; }

        public Movie()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new EntityInvalidException("Movie name is required.");
            if (TmdbMovie == null)
            {
                if (string.IsNullOrEmpty(Url))
                    throw new EntityInvalidException("Movie URL is required.");
                if (string.IsNullOrEmpty(CustomImageUrl))
                    throw new EntityInvalidException("Movie image URL is required.");
                if (!Validation.Url.IsValid(Url))
                    throw new EntityInvalidException("Movie URL is invalid.");
                if (!Validation.Url.IsValid(CustomImageUrl))
                    throw new EntityInvalidException("Movie image URL is invalid.");
            }
        }

        public override void Map(object source)
        {
            Map(source as Movie);
        }

        public void Map(Movie source)
        {
            if (source != null)
            {
                TmdbMovie = source.TmdbMovie;
            }
        }

        public bool CanBeEditedBy(User user)
        {
            return user.HasPermission(Permissions.EditOthersMovies) || user == User;
        }

        private static string NormalizeUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && !url.ToLower().Contains("://"))
            {
                url = "http://" + url;
            }
            return url;
        }
    }
}
