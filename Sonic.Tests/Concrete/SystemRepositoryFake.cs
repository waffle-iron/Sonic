using Sonic.Domain.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Sonic.Tests.Concrete
{
    public class SystemRepositoryFake : ICrudRepository<Domain.Entities.System>
    {
        private readonly List<Domain.Entities.System> _storage = new List<Domain.Entities.System>();

        public IEnumerable<Domain.Entities.System> All => _storage.AsEnumerable();

        public Domain.Entities.System GetById(int id)
        {
            return _storage.FirstOrDefault(p => p.SystemId == id);
        }

        public bool Add(Domain.Entities.System item)
        {
            foreach (var role in item.Roles)
            {
                if (role.System == null)
                {
                    role.System = item;
                }
            }
            _storage.Add(item);

            return true;
        }

        public bool Update(Domain.Entities.System item)
        {
            foreach (var role in item.Roles)
            {
                if (role.System == null)
                {
                    role.System = item;
                }
            }

            var entity = _storage.FirstOrDefault(p => p.SystemId == item.SystemId);
            entity.Name = item.Name;

            return true;
        }

        public bool Remove(int id)
        {
            var entity = _storage.FirstOrDefault(p => p.SystemId == id);
            _storage.Remove(entity);

            return true;
        }
    }
}
