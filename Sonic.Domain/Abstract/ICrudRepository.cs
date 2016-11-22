using System.Collections.Generic;

namespace Sonic.Domain.Abstract
{
    public interface ICrudRepository<T>
    {
        IEnumerable<T> All { get; }

        T GetById(int id);
        bool Add(T item);
        bool Update(T item);
        bool Remove(int id);
    }
}
