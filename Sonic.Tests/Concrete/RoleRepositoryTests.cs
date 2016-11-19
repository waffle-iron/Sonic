using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sonic.Tests.Concrete
{    
    public class RoleRepositoryTests
    {
        private readonly ICrudRepository<Role> repository = null;

        public RoleRepositoryTests()
        {
            repository = new RoleRepositoryFake();
        }

        [Fact]
        public void get_all()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Assert.Equal(2, repository.GetAll().Count());
        }

        [Fact]
        public void get_by_id()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Assert.Equal(2, repository.GetAll().Count());
            Assert.Equal("App 1 - user", repository.GetById(2).Name);
        }

        public void add()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Assert.Equal(2, repository.GetAll().Count());
            Assert.Equal("App 1 - user", repository.GetById(2).Name);
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void update()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Role role = repository.GetById(2);
            role.Name = "Test";
            repository.Update(role);

            Assert.Equal("Test", repository.GetById(2).Name);
        }

        [Fact]
        public void remove()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            repository.Remove(1);

            Assert.Equal(1, repository.GetAll().Count());
        }
    }
}
