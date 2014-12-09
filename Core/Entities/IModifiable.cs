using System;

namespace MovieService.Core.Entities
{
    public interface IModifiable
    {
        DateTimeOffset ModifiedDate { get; set; }
    }
}
