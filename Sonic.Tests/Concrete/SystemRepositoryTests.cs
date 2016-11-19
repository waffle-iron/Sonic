using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sonic.Tests.Concrete
{
    public class SystemRepositoryTests
    {
        private readonly ICrudRepository<Domain.Entities.System> repository = null;

        public SystemRepositoryTests()
        {
            repository = new SystemRepositoryFake();
        }

        [Fact]
        public void get_all()
        {
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public void get_roles_from_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            system.Roles.Add(new Role() { RoleId = 1, Name = "Admin", SystemId = 1 });
            system.Roles.Add(new Role() { RoleId = 2, Name = "User", SystemId = 1 });

            repository.Add(system);

            Assert.Equal(2, system.Roles.Count);
        }

        [Fact]
        public void get_by_id()
        {
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            Assert.Equal("App 2", repository.GetById(2).Name);
        }

        [Fact]
        public void get_role_from_system_by_id()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            system.Roles.Add(new Role() { RoleId = 1, Name = "Admin", SystemId = 1 });
            system.Roles.Add(new Role() { RoleId = 2, Name = "User", SystemId = 1 });

            repository.Add(system);

            Assert.Equal("User", system.Roles.FirstOrDefault(p => p.RoleId == 2).Name);
        }

        [Fact]
        public void add()
        {
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });            

            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void add_roles_to_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            repository.Add(system);
            var adminRole = new Role() { RoleId = 1, Name = "Admin", SystemId = 1 };
            var userRole = new Role() { RoleId = 2, Name = "User", SystemId = 1 };

            repository.GetById(1).Roles.Add(adminRole);
            repository.GetById(1).Roles.Add(userRole);

            Assert.Equal("User", system.Roles.FirstOrDefault(p => p.RoleId == 2).Name);
            Assert.Equal(2, repository.GetById(1).Roles.Count);
        }

        [Fact]
        public void update()
        {
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            Domain.Entities.System system = repository.GetById(2);
            system.Name = "Test";
            repository.Update(system);

            Assert.Equal("Test", repository.GetById(2).Name);
        }

        [Fact]
        public void update_role_in_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            repository.Add(system);
            var adminRole = new Role() { RoleId = 1, Name = "Admin", SystemId = 1 };
            var userRole = new Role() { RoleId = 2, Name = "User", SystemId = 1 };

            Domain.Entities.System systemUpdate = repository.GetById(1);
            systemUpdate.Roles.Add(adminRole);
            systemUpdate.Roles.Add(userRole);
            repository.Update(systemUpdate);

            var testRole = repository.GetById(1).Roles.FirstOrDefault(p => p.RoleId == 2);
            Assert.Equal(1, testRole.SystemId);
            Assert.Equal("App", testRole.System.Name);
        }

        [Fact]
        public void remove()
        {
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            repository.Remove(2);

            Assert.Equal(2, repository.GetAll().Count());
        }
    }
}
