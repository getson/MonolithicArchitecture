using System;
using System.Text;

namespace MyApp.SharedKernel.Domain
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity : IEquatable<BaseEntity>, IProjectableEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Is transient
        /// </summary>
        /// <returns>Result</returns>
        public bool IsTransient()
        {
            return Equals(Id, default(int));
        }

        /// <summary>
        /// Equals
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


        protected string GetAllProperties<T>(T @object)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var strLine = new StringBuilder();
            strLine.Append($"{type.FullName}=>{{");
            foreach (var property in props)
            {
                strLine.Append($"'{property.Name}' : '{property.GetValue(@object)}'");
            }

            strLine.Append("}");
            return strLine.ToString();
        }

        public override string ToString()
        {
            return GetAllProperties(this);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
