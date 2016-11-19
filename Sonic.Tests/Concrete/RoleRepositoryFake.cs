using System;
using System.Collections.Generic;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System.Linq;

namespace Sonic.Tests.Concrete
{
    public class RoleRepositoryFake : ICrudRepository<Role>
    {
        private List<Role> storage = new List<Role>();

        public IEnumerable<Role> GetAll()
        {
            return storage.AsEnumerable();
        }

        public Role GetById(int id)
        {
            return storage.FirstOrDefault(p => p.RoleId == id);
        }

        public bool Add(Role item)
        {
            storage.Add(item);

            return true;
        }
                
        public bool Update(Role item)
        {
            Role entity = storage.FirstOrDefault(p => p.RoleId == item.RoleId);
            entity.Name = item.Name;
            
            return true;
        }

        public bool Remove(int id)
        {
            Role entity = storage.FirstOrDefault(p => p.RoleId == id);
            storage.Remove(entity);

            return true;
        }
    }
}