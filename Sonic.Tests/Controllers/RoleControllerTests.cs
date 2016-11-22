using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

            var result = controller.Index(systemApp2.SystemId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Role>>(viewResult.ViewData.Model);
            model.Should().HaveCount(2);
            model.First().SystemId.Should().Be(2);
            model.ToList()[1].Name.Should().Be("userTest");
        }

        [Fact]
        public void index_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            var result = controller.Index(3);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("System");
            redirectToActionResult.ActionName.Should().Be("Index");
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

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
            Assert.Equal(1, model.SystemId);
            Assert.Equal("App 1", model.System.Name);
        }

        [Fact]
        public void create_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            _systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            var result = controller.Create(2);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
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

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(2, _repository.All.Count());
            Assert.Equal("App 1", _repository.GetById(2).System.Name);
            Assert.Equal("user", _repository.GetById(2).Name);
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

            Assert.Equal(entity.Name.Trim(), _repository.GetById(2).Name);
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

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
            Assert.Equal(1, model.SystemId);
            Assert.Equal("App 1", model.System.Name);
            Assert.Equal(2, model.RoleId);
            Assert.Equal("user", model.Name);
        }

        [Fact]
        public void edit_redirect_to_action_if_role_id_does_not_exist()
        {
            var controller = new RoleController(_repository, _systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            _repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            _repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            var result = controller.Edit(3);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
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

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(2, _repository.All.Count());
            Assert.Equal("test", _repository.GetById(2).Name);
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

            _repository.GetById(2).Name.Should().Be(entity.Name.Trim());
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

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("Role");
            redirectToActionResult.ActionName.Should().Be("Index");
            _repository.All.Should().HaveCount(1);
        }
    }
}
