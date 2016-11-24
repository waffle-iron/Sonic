using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Sonic.Domain.Concrete
{
    public class RoleRepository : ICrudRepository<Role>
    {
        private readonly SonicDbContext _dbContext;

        public IEnumerable<Role> All => _dbContext.Roles;

        public RoleRepository(IServiceProvider serviceProvider)
        {
            _dbContext = new SonicDbContext(serviceProvider.GetRequiredService<DbContextOptions<SonicDbContext>>());
        }

        public Role GetById(int id)
        {
            return _dbContext.Roles.FirstOrDefault(p => p.RoleId == id);
        }

        public bool Add(Role item)
        {
            _dbContext.Roles.Add(item);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(Role item)
        {
            var entity = _dbContext.Roles.FirstOrDefault(p => p.RoleId == item.RoleId);
            if (entity == null)
                return false;

            entity.Name = item.Name;
            _dbContext.SaveChanges();
            return true;
        }

        public bool Remove(int id)
        {
            var entity = _dbContext.Roles.FirstOrDefault(p => p.RoleId == id);

            if (entity == null)
                return false;

            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
