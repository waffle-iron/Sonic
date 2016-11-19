using Sonic.Domain.Abstract;
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
            repository.Add(new Domain.Entities.System() { Id = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { Id = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { Id = 3, Name = "App 3" });

            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public void get_by_id()
        {
            repository.Add(new Domain.Entities.System() { Id = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { Id = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { Id = 3, Name = "App 3" });

            Assert.Equal("App 2", repository.GetById(2).Name);
        }

        [Fact]
        public void add()
        {
            repository.Add(new Domain.Entities.System() { Id = 1, Name = "App 1" });            

            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void update()
        {
            repository.Add(new Domain.Entities.System() { Id = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { Id = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { Id = 3, Name = "App 3" });

            Domain.Entities.System system = repository.GetById(2);
            system.Name = "Test";
            repository.Update(system);

            Assert.Equal("Test", repository.GetById(2).Name);
        }

        [Fact]
        public void remove()
        {
            repository.Add(new Domain.Entities.System() { Id = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { Id = 2, Name = "App 2" });
            repository.Add(new Domain.Entities.System() { Id = 3, Name = "App 3" });

            repository.Remove(2);

            Assert.Equal(2, repository.GetAll().Count());
        }
    }
}
