﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Core.SharedKernel.Domain
{
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public bool Equals(ValueObject<T> other)
        {
            return EqualsCore(other);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is ValueObject<T> valueObject))
                return false;

            return EqualsCore(valueObject);
        }

        private bool EqualsCore(ValueObject<T> other)
        {
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
