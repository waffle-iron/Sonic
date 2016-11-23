using FluentAssertions;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.Tests.Concrete.Fakes;
using Xunit;

namespace Sonic.Tests.Concrete.Tests
{
    public class RoleRepositoryTests
    {
        public ICrudRepository<Role> Repository { get; }

        public RoleRepositoryTests()
        {
            Repository = new RoleRepositoryFake();
        }

        [Fact]
        public void get_all()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            Repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            Repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Repository.All.Should().HaveCount(2);
        }

        [Fact]
        public void get_by_id()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            Repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            Repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Repository.All.Should().HaveCount(2);
            Repository.GetById(2).Name.Should().Be("App 1 - user");
        }

        [Fact]
        public void add_role()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            Repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            Repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Repository.All.Should().HaveCount(2);
            Repository.GetById(2).Name.Should().Be("App 1 - user");
        }

        [Fact]
        public void update_role()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            Repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            Repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            var role = Repository.GetById(2);
            role.Name = "Test";
            Repository.Update(role);

            Repository.GetById(2).Name.Should().Be("Test");
        }

        [Fact]
        public void remove_role()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            Repository.Add(new Role() { RoleId = 1, SystemId = 1, Name = "App 1 - admin", System = system });
            Repository.Add(new Role() { RoleId = 2, SystemId = 1, Name = "App 1 - user", System = system });

            Repository.Remove(1);


            Repository.All.Should().HaveCount(1);
        }
    }
}
