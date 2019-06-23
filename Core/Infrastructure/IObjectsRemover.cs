using System;

namespace Core.Infrastructure
{
    public interface IObjectsRemover<out T>
    {
        void Remove(Predicate<T> removePredicate);
    }
}