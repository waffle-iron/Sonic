using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class RoleControllerTests
    {
        private readonly ICrudRepository<Role> repository = null;
        private readonly ICrudRepository<Domain.Entities.System> systemRepository = null;

        public RoleControllerTests()
        {
            repository = new RoleRepositoryFake();
            systemRepository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_system_roles()
        {
            var systemApp1 = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            var systemApp2 = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };
            systemRepository.Add(systemApp1);
            systemRepository.Add(systemApp2);

            var controller = new RoleController(repository, systemRepository);            
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = systemApp1 });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = systemApp1 });
            repository.Add(new Role() { RoleId = 3, Name = "adminTest", SystemId = 2, System = systemApp2 });
            repository.Add(new Role() { RoleId = 4, Name = "userTest", SystemId = 2, System = systemApp2 });

            IActionResult result = controller.Index(systemApp2.SystemId);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<Role> model = Assert.IsAssignableFrom<IEnumerable<Role>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            Assert.Equal(2, model.First().SystemId);
            Assert.Equal("userTest", model.ToList()[1].Name);
        }

        [Fact]
        public void index_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(repository, systemRepository);
            systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            systemRepository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            IActionResult result = controller.Index(3);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void the_name_field_is_required()
        {
            var entity = new Role { RoleId = 1, SystemId = 1, Name = "" };
            var validationContext = new ValidationContext(entity, null, null);
            var validationResult = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            Assert.False(valid);
            var failure = Assert.Single(validationResult, x => x.ErrorMessage == "The Name field is required.");
            Assert.Single(failure.MemberNames, x => x == "Name");
        }

        [Fact]
        public void create_get_system_by_id()
        {
            var controller = new RoleController(repository, systemRepository);
            systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            IActionResult result = controller.Create(1);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Role model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
            Assert.Equal(1, model.SystemId);
            Assert.Equal("App 1", model.System.Name);
        }

        [Fact]
        public void create_redirect_to_action_if_system_id_does_not_exist()
        {
            var controller = new RoleController(repository, systemRepository);
            systemRepository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            IActionResult result = controller.Create(2);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void create_post()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            var controller = new RoleController(repository, systemRepository);            
            var roleAdmin = new Role() { RoleId = 1, SystemId = 1, Name = "admin", System = system };
            repository.Add(roleAdmin);

            IActionResult result = controller.Create(new Role() { RoleId = 2, SystemId = 1, Name = "user", System = system });

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(2, repository.GetAll().Count());
            Assert.Equal("App 1", repository.GetById(2).System.Name);
            Assert.Equal("user", repository.GetById(2).Name);
        }

        [Fact]
        public void create_post_removing_spaces_for_the_name_field()
        {
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            var controller = new RoleController(repository, systemRepository);
            var roleAdmin = new Role() { RoleId = 1, SystemId = 1, Name = "admin", System = system };
            repository.Add(roleAdmin);

            var entity = new Role() { RoleId = 2, SystemId = 1, Name = "  user  ", System = system };
            IActionResult result = controller.Create(entity);

            Assert.Equal(entity.Name.Trim(), repository.GetById(2).Name);
        }

        [Fact]
        public void edit_get_role_by_id()
        {
            var controller = new RoleController(repository, systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            IActionResult result = controller.Edit(2);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Role model = Assert.IsAssignableFrom<Role>(viewResult.ViewData.Model);
            Assert.Equal(1, model.SystemId);
            Assert.Equal("App 1", model.System.Name);
            Assert.Equal(2, model.RoleId);
            Assert.Equal("user", model.Name);
        }

        [Fact]
        public void edit_redirect_to_action_if_role_id_does_not_exist()
        {
            var controller = new RoleController(repository, systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });

            IActionResult result = controller.Edit(3);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void edit_post()
        {
            var controller = new RoleController(repository, systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });
            Role entity = repository.GetById(2);
            entity.Name = "test";

            IActionResult result = controller.Edit(entity);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(2, repository.GetAll().Count());
            Assert.Equal("test", repository.GetById(2).Name);
        }

        [Fact]
        public void edit_post_removing_spaces_for_the_name_field()
        {
            var controller = new RoleController(repository, systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });
            Role entity = repository.GetById(2);
            entity.Name = "  test  ";

            IActionResult result = controller.Edit(entity);

            Assert.Equal(entity.Name.Trim(), repository.GetById(2).Name);            
        }

        [Fact]
        public void remove_post()
        {
            var controller = new RoleController(repository, systemRepository);
            var system = new Domain.Entities.System() { SystemId = 1, Name = "App 1" };
            systemRepository.Add(system);
            repository.Add(new Role() { RoleId = 1, Name = "admin", SystemId = 1, System = system });
            repository.Add(new Role() { RoleId = 2, Name = "user", SystemId = 1, System = system });            
                        
            IActionResult result = controller.Delete(2);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Role", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, repository.GetAll().Count());
        }        
    }
}
