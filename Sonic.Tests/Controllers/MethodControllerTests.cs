using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class MethodControllerTests
    {
        private readonly ICrudRepository<Method> repository = null;
        private readonly ICrudRepository<Domain.Entities.System> systemRepository = null;

        public MethodControllerTests()
        {
            repository = new MethodRepositoryFake();
            systemRepository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_system_methods()
        {
            var systemApp1 = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            var systemApp2 = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };
            systemRepository.Add(systemApp1);
            systemRepository.Add(systemApp2);

            var controller = new MethodController(repository, systemRepository);
            repository.Add(new Method() { MethodId = 1, Name = "MethodOneInApp1", SystemId = 1 });
            repository.Add(new Method() { MethodId = 2, Name = "MethodOne", SystemId = 2 });
            repository.Add(new Method() { MethodId = 3, Name = "MethodTwo", SystemId = 2 });
            repository.Add(new Method() { MethodId = 4, Name = "MethodThree", SystemId = 2 });

            IActionResult result = controller.Index(systemApp2.SystemId);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<Method> model = Assert.IsAssignableFrom<IEnumerable<Method>>(viewResult.ViewData.Model);
            model.Should().HaveCount(3);
            model.FirstOrDefault().SystemId.Should().Be(2);
            model.FirstOrDefault(p => p.MethodId == 3).Name.Should().Be("MethodTwo");
        }
    }
}
