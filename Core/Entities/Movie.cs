using System;
using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class Movie : Entity, IModifiable
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

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
                throw new ArgumentException("Name is required field");
        }
    }
}
