using System;

namespace Svitla.MovieService.Core.Entities
{
    public interface IModifiable
    {
        DateTimeOffset ModifiedDate { get; set; }
    }
}
