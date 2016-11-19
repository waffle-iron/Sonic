using Sonic.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sonic.Domain.Entities;

namespace Sonic.Tests.Concrete
{
    public class SystemRepositoryFake : ICrudRepository<Domain.Entities.System>
    {
        private List<Domain.Entities.System> storage = new List<Domain.Entities.System>();

        public IEnumerable<Domain.Entities.System> GetAll()
        {
            return storage.AsEnumerable();
        }

        public Domain.Entities.System GetById(int id)
        {
            return storage.FirstOrDefault(p => p.SystemId == id);
        }

        public bool Add(Domain.Entities.System item)
        {
            foreach (Role role in item.Roles)
            {
                if (role.System == null)
                {
                    role.System = item;
                }
            }
            storage.Add(item);

            return true;
        }

        public bool Update(Domain.Entities.System item)
        {
            foreach (Role role in item.Roles)
            {
                if (role.System == null)
                {
                    role.System = item;
                }
            }
            Domain.Entities.System entity = storage.FirstOrDefault(p => p.SystemId == item.SystemId);
            entity.Name = item.Name;

            return true;
        }

        public bool Remove(int id)
        {
            Domain.Entities.System entity = storage.FirstOrDefault(p => p.SystemId == id);
            storage.Remove(entity);

            return true;
        }
    }
}
