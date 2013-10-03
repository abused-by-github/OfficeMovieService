using System;

namespace Svitla.MovieService.Core.Entities
{
    /// <summary>
    /// Entity with Id = 0 doesn't equal anything except itself.
    /// </summary>
    public abstract class Entity
    {
        public long Id { get; set; }

        public override int GetHashCode()
        {
            return Id > 0 ? Id.GetHashCode() : Guid.NewGuid().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as Entity);
        }

        public bool Equals(Entity other)
        {
            bool result = false;
            if (!ReferenceEquals(null, other))
            {
                result = GetType() == other.GetType() && Id > 0 && Id == other.Id;
            }
            return result;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
