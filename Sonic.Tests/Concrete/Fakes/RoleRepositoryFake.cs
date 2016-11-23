using System.Collections.Generic;
using System.Linq;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;

namespace Sonic.Tests.Concrete.Fakes
{
    public class RoleRepositoryFake : ICrudRepository<Role>
    {
        private readonly List<Role> _storage = new List<Role>();

        public IEnumerable<Role> All => _storage.AsEnumerable();

        public Role GetById(int id)
        {
            return _storage.FirstOrDefault(p => p.RoleId == id);
        }

        public bool Add(Role item)
        {
            _storage.Add(item);

            return true;
        }

        public bool Update(Role item)
        {
            var entity = _storage.FirstOrDefault(p => p.RoleId == item.RoleId);
            entity.Name = item.Name;

            return true;
        }

        public bool Remove(int id)
        {
            var entity = _storage.FirstOrDefault(p => p.RoleId == id);
            _storage.Remove(entity);

            return true;
        }
    }
}