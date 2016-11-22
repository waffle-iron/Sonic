using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sonic.Tests.Concrete
{
    public class SystemRepositoryTests
    {
        private readonly ICrudRepository<Domain.Entities.System> _repository;

        public SystemRepositoryTests()
        {
            _repository = new SystemRepositoryFake();
        }

        [Fact]
        public void get_all()
        {
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            _repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            _repository.All.Should().HaveCount(3);
        }

        [Fact]
        public void get_roles_from_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            system.Roles.Add(new Role() { RoleId = 1, Name = "Admin", SystemId = 1 });
            system.Roles.Add(new Role() { RoleId = 2, Name = "User", SystemId = 1 });

            _repository.Add(system);

            _repository.GetById(1).Roles.Should().HaveCount(2);
        }

        [Fact]
        public void get_by_id()
        {
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            _repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            _repository.GetById(2).Name.Should().Be("App 2");
        }

        [Fact]
        public void get_role_from_system_by_id()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            system.Roles.Add(new Role() { RoleId = 1, Name = "Admin", SystemId = 1 });
            system.Roles.Add(new Role() { RoleId = 2, Name = "User", SystemId = 1 });

            _repository.Add(system);

            system.Roles.FirstOrDefault(p => p.RoleId == 2).Name.Should().Be("User");
        }

        [Fact]
        public void add_system()
        {
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            _repository.All.Should().HaveCount(1);
        }

        [Fact]
        public void add_roles_to_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            _repository.Add(system);
            var adminRole = new Role() { RoleId = 1, Name = "Admin", SystemId = 1 };
            var userRole = new Role() { RoleId = 2, Name = "User", SystemId = 1 };

            _repository.GetById(1).Roles.Add(adminRole);
            _repository.GetById(1).Roles.Add(userRole);

            system.Roles.FirstOrDefault(p => p.RoleId == 2).Name.Should().Be("User");
            _repository.GetById(1).Roles.Should().HaveCount(2);
        }

        [Fact]
        public void update_system()
        {
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            _repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            var system = _repository.GetById(2);
            system.Name = "Test";
            _repository.Update(system);

            _repository.GetById(2).Name.Should().Be("Test");
        }

        [Fact]
        public void update_role_in_system()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App" };
            _repository.Add(system);
            var adminRole = new Role() { RoleId = 1, Name = "Admin", SystemId = 1 };
            var userRole = new Role() { RoleId = 2, Name = "User", SystemId = 1 };

            var systemUpdate = _repository.GetById(1);
            systemUpdate.Roles.Add(adminRole);
            systemUpdate.Roles.Add(userRole);
            _repository.Update(systemUpdate);

            var testRole = _repository.GetById(1).Roles.FirstOrDefault(p => p.RoleId == 2);
            testRole.SystemId.Should().Be(1);
            testRole.System.Name.Should().Be("App");
        }

        [Fact]
        public void remove_system()
        {
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            _repository.Add(new Domain.Entities.System() { SystemId = 3, Name = "App 3" });

            _repository.Remove(2);

            _repository.All.Should().HaveCount(2);
        }
    }
}
