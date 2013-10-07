using System;
using System.Collections.Generic;
using Svitla.MovieService.Core.Exceptions;

namespace Svitla.MovieService.Core.Entities
{
    public class Movie : Entity, IModifiable
    {
        public string Name { get; set; }

        private string url;
        public string Url {
            get
            {
                return url;
            }
            set
            {
                url = NormalizeUrl(value);
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = NormalizeUrl(value);
            }
        }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public virtual User User { get; set; }
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
            if (string.IsNullOrEmpty(Url))
                throw new EntityInvalidException("Movie URL is required.");
            if (!Validation.Url.IsValid(Url))
                throw new EntityInvalidException("Movie URL is invalid.");
            if (string.IsNullOrEmpty(ImageUrl))
                throw new EntityInvalidException("Movie image URL is required.");
            if (!Validation.Url.IsValid(ImageUrl))
                throw new EntityInvalidException("Movie image URL is invalid.");
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
