﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class MethodControllerTests
    {
        private readonly ICrudRepository<Method> _repository;
        private readonly ICrudRepository<Domain.Entities.System> _systemRepository;

        public MethodControllerTests()
        {
            _repository = new MethodRepositoryFake();
            _systemRepository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_system_methods()
        {
            var systemApp1 = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            var systemApp2 = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };
            _systemRepository.Add(systemApp1);
            _systemRepository.Add(systemApp2);

            var controller = new MethodController(_repository, _systemRepository);
            _repository.Add(new Method() { MethodId = 1, Name = "MethodOneInApp1", SystemId = 1 });
            _repository.Add(new Method() { MethodId = 2, Name = "MethodOne", SystemId = 2 });
            _repository.Add(new Method() { MethodId = 3, Name = "MethodTwo", SystemId = 2 });
            _repository.Add(new Method() { MethodId = 4, Name = "MethodThree", SystemId = 2 });

            var result = controller.Index(systemApp2.SystemId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Method>>(viewResult.ViewData.Model);
            model.Should().HaveCount(3);
            model.FirstOrDefault().SystemId.Should().Be(2);
            model.FirstOrDefault(p => p.MethodId == 3).Name.Should().Be("MethodTwo");
        }
    }
}
