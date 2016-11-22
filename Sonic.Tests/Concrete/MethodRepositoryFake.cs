using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System;
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
            throw new NotImplementedException();
            //return storage.FirstOrDefault(p => p.MethodId == id);
        }

        public bool Add(Method item)
        {
            _storage.Add(item);

            return true;
        }

        public bool Update(Method item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }        
    }
}
