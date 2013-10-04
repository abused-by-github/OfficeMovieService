using System;
using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class Movie : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("Name is required field");
        }
    }
}
