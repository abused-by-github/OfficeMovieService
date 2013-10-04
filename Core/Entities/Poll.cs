using System;
using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class Poll : Entity
    {
        public string Name { get; set; }

        /// <summary>
        /// Date when poll was created.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Date when poll will be closed.
        /// </summary>
        public DateTimeOffset ExpirationDate { get; set; }

        /// <summary>
        /// Date when movie which has won will be viewed.
        /// </summary>
        public DateTimeOffset ViewDate { get; set; }

        public virtual User Owner { get; set; }

        public bool HasViewed
        {
            get
            {
                return ViewDate < DateTime.Now;
            }
        }

        public bool IsVoteable
        {
            get
            {
                return ExpirationDate > DateTime.Now;
            }
        }

        public Poll()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
