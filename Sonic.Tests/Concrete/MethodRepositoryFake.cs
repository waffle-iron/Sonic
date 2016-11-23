using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Sonic.Tests.Concrete
{
    public class MethodRepositoryFake : ICrudRepository<Method>
    {
        private readonly List<Method> _storage = new List<Method>();

        public IEnumerable<Method> All => _storage.AsEnumerable();

        public Method GetById(int id)
        {
            return _storage.FirstOrDefault(p => p.MethodId == id);
        }

        public bool Add(Method item)
        {
            _storage.Add(item);

            return true;
        }

        public bool Update(Method item)
        {
            var entity = _storage.FirstOrDefault(p => p.MethodId == item.MethodId);
            entity.Name = item.Name;

            return true;
        }

        public bool Remove(int id)
        {
            var entity = _storage.FirstOrDefault(p => p.MethodId == id);
            _storage.Remove(entity);

            return true;
        }        
    }
}
