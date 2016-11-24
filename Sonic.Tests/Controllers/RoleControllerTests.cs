using FluentAssertions;
using FluentAssertions.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.WebUI.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sonic.Tests.Concrete.Fakes;
using Sonic.WebUI.Models;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class RoleControllerTests
    {
        private readonly ICrudRepository<Role> _repository;
        private readonly ICrudRepository<Domain.Entities.System> _systemRepository;

        public RoleControllerTests()
        {
            _repository = new RoleRepositoryFake();
            _systemRepository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_system_roles()
        {
            var systemApp1 = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            var systemApp2 = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };
            _systemRepository.Add(systemApp1);
            _systemRepository.Add(systemApp2);

            var controller = new RoleController(_repository, _systemRepository);
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = systemApp1 });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = systemApp1 });
            _repository.Add(new Role() { RoleId = 3, Name = "adminTest", SystemId = 2, System = systemApp2 });
            _repository.Add(new Role() { RoleId = 4, Name = "userTest", SystemId = 2, System = systemApp2 });

            var result = controller.Index(2);

            result.Should().BeViewResult().ModelAs<RoleModel>().Roles.Should().HaveCount(2);
            result.Should().BeViewResult().ModelAs<RoleModel>().System.Name.Should().Be("App 2");
            result.Should().BeViewResult().ModelAs<RoleModel>().Roles.First(p => p.RoleId == 4).Name.Should().Be("userTest");
        }

        [Fact]
        public void index_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            var result = controller.Index(3);

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("System");
        }

        [Fact]
        public void the_name_field_is_required()
        {
            var entity = new Role { RoleId = 1, SystemId = 1, Name = "" };
            var validationContext = new ValidationContext(entity, null, null);
            var validationResult = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            Assert.False(valid);
            var failure = Assert.Single(validationResult, x => x.ErrorMessage == "The Name field is required.");
            Assert.Single(failure.MemberNames, x => x == "Name");
        }

        [Fact]
        public void create_get_system_by_id()
        {
            var controller = new RoleController(_repository, _systemRepository);
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            var result = controller.Create(1);

            result.Should().BeViewResult().ModelAs<Role>().System.Name.Should().Be("App 1");
        }

        [Fact]
        public void create_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            var result = controller.Create(2);

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("System");
        }

        [Fact]
        public void create_post()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            var controller = new RoleController(_repository, _systemRepository);
            var roleAdmin = new Role() { RoleId = 1, SystemId = 1, Name = "admin", System = system };
            _repository.Add(roleAdmin);

            var result = controller.Create(new Role() { RoleId = 2, SystemId = 1, Name = "user", System = system });

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("Role");
            _repository.All.Count().Should().Be(2);
            _repository.GetById(2).System.Name.Should().Be("App 1");
            _repository.GetById(2).Name.Should().Be("user");
        }

        [Fact]
        public void create_post_removing_spaces_for_the_name_field()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            var controller = new RoleController(_repository, _systemRepository);
            var roleAdmin = new Role() { RoleId = 1, SystemId = 1, Name = "admin", System = system };
            _repository.Add(roleAdmin);

            var entity = new Role() { RoleId = 2, SystemId = 1, Name = "  user  ", System = system };
            controller.Create(entity);

            _repository.GetById(2).Name.Should().Be("user");
        }

        [Fact]
        public void edit_get_role_by_id()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            var result = controller.Edit(2);

            result.Should().BeViewResult().ModelAs<Role>().System.Name.Should().Be("App 1");
            result.Should().BeViewResult().ModelAs<Role>().Name.Should().Be("user");
        }

        [Fact]
        public void edit_redirect_to_action_if_role_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            var result = controller.Edit(3);

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("System");
        }

        [Fact]
        public void edit_post()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });
            var entity = _repository.GetById(2);
            entity.Name = "test";

            var result = controller.Edit(entity);

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("Role");
            _repository.All.Count().Should().Be(2);
            _repository.GetById(2).Name.Should().Be("test");
        }

        [Fact]
        public void edit_post_removing_spaces_for_the_name_field()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });
            var entity = _repository.GetById(2);
            entity.Name = "  test  ";

            controller.Edit(entity);

            _repository.GetById(2).Name.Should().Be("test");
        }

        [Fact]
        public void remove_post()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _systemRepository.Add(system);
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            var result = controller.Delete(2);

            result.Should().BeRedirectToRouteResult().WithAction("Index").WithController("Role");
            _repository.All.Should().HaveCount(1);
        }
    }
}
