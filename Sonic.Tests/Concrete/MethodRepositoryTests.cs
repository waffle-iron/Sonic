using FluentAssertions;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Xunit;

namespace Sonic.Tests.Concrete
{    
    public class MethodRepositoryTests
    {
        public ICrudRepository<Method> Repository { get; }

        public MethodRepositoryTests()
        {
            Repository = new MethodRepositoryFake();
        }

        [Fact]
        public void get_all()
        {
            var system = new Domain.Entities.System {SystemId = 1, Name = "App 1"};
            Repository.Add(new Method {MethodId = 1, Name = "MethodOne", SystemId = 1, System = system});
            Repository.Add(new Method {MethodId = 2, Name = "MethodTwo", SystemId = 1, System = system});
                        
            Repository.All.Should().HaveCount(2);
        }

        [Fact]
        public void get_by_id()
        {
            var system = new Domain.Entities.System { SystemId = 1, Name = "App 1" };
            Repository.Add(new Method { MethodId = 1, Name = "MethodOne", SystemId = 1, System = system });
            Repository.Add(new Method { MethodId = 2, Name = "MethodTwo", SystemId = 1, System = system });

            Repository.All.Should().HaveCount(2);
            Repository.GetById(2).Name.Should().Be("MethodTwo");
        }

        [Fact]
        public void add_method()
        {
            var system = new Domain.Entities.System { SystemId = 1, Name = "App 1" };
            Repository.Add(new Method { MethodId = 1, Name = "MethodOne", SystemId = 1, System = system });
            Repository.Add(new Method { MethodId = 2, Name = "MethodTwo", SystemId = 1, System = system });

            Repository.All.Should().HaveCount(2);
            Repository.GetById(2).Name.Should().Be("MethodTwo");
        }

        [Fact]
        public void update_method()
        {
            var system = new Domain.Entities.System { SystemId = 1, Name = "App 1" };
            Repository.Add(new Method { MethodId = 1, Name = "MethodOne", SystemId = 1, System = system });
            Repository.Add(new Method { MethodId = 2, Name = "MethodTwo", SystemId = 1, System = system });

            var method = Repository.GetById(2);
            method.Name = "Test";
            Repository.Update(method);

            Repository.GetById(2).Name.Should().Be("Test");
        }

        [Fact]
        public void remove_role()
        {
            var system = new Domain.Entities.System { SystemId = 1, Name = "App 1" };
            Repository.Add(new Method { MethodId = 1, Name = "MethodOne", SystemId = 1, System = system });
            Repository.Add(new Method { MethodId = 2, Name = "MethodTwo", SystemId = 1, System = system });

            Repository.Remove(1);

            Repository.All.Should().HaveCount(1);
        }
    }
}
