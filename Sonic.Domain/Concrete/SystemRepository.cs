using System;
using System.Collections.Generic;
using Sonic.Domain.Abstract;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sonic.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Sonic.Domain.Concrete
{
    public class SystemRepository : ICrudRepository<Entities.System>
    {
        private readonly SonicDbContext _dbContext;

        public IEnumerable<Entities.System> All => _dbContext.Systems;

        public SystemRepository(IServiceProvider serviceProvider)
        {
            _dbContext = new SonicDbContext(serviceProvider.GetRequiredService<DbContextOptions<SonicDbContext>>());
        }

        public Entities.System GetById(int id)
        {
            return _dbContext.Systems.FirstOrDefault(p => p.SystemId == id);
        }

        public bool Add(Entities.System item)
        {
            _dbContext.Systems.Add(item);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(Entities.System item)
        {
            var entity = _dbContext.Systems.FirstOrDefault(p => p.SystemId == item.SystemId);
            if (entity == null)
                return false;

            entity.Name = item.Name;
            _dbContext.SaveChanges();
            return true;
        }

        public bool Remove(int id)
        {
            var entity = _dbContext.Systems.FirstOrDefault(p => p.SystemId == id);

            if (entity == null)
                return false;

            _dbContext.Remove(entity);
            return true;
        }
    }
}