using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sonic.Domain.Abstract
{
    public interface ICrudRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Add(T item);
        bool Update(T item);
        bool Remove(int id);
    }
}
