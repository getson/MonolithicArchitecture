using System;

namespace BinaryOrigin.SeedWork.Core.Domain
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity : IEquatable<BaseEntity>, IProjectable
    {
        protected BaseEntity()
                    : this(Guid.NewGuid())
        {
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// get or set the identifier
        /// </summary>
        public Guid Id { get; set; }

        public bool IsTransient()
        {
            return Id == default(Guid);
        }

        /// <summary>
        /// check for object equality
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Result</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">other entity</param>
        /// <returns>Result</returns>
        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (IsTransient() || other.IsTransient() || !Equals(Id, other.Id))
                return false;

            var otherType = other.GetType();
            var thisType = GetType();

            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}