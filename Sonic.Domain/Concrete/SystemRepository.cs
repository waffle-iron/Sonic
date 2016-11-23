using System;
using System.Collections.Generic;
using Sonic.Domain.Abstract;
using System.Linq;
using Sonic.Domain.Entities;

namespace Sonic.Domain.Concrete
{
    public class SystemRepository : ICrudRepository<Entities.System>
    {
        private readonly List<Entities.System> _testStorageToRemove = new List<Domain.Entities.System>();

        public IEnumerable<Entities.System> All => _testStorageToRemove.AsEnumerable();

        public SystemRepository()
        {
            _testStorageToRemove.Add(new Entities.System() {SystemId = 1, Name = "Test", Roles = new List<Role>()});
        }
        
        public Entities.System GetById(int id)
        {
            return _testStorageToRemove.FirstOrDefault(p => p.SystemId == id);
        }

        public bool Add(Entities.System item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Entities.System item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}