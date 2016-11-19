using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class SystemControllerTests
    {
        private readonly ICrudRepository<Domain.Entities.System> repository = null;

        public SystemControllerTests()
        {
            repository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_all()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            IActionResult result = controller.Index();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            IEnumerable<Domain.Entities.System> model =
                Assert.IsAssignableFrom<IEnumerable<Domain.Entities.System>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void edit_redirect_to_action_if_id_does_not_exist()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            IActionResult result = controller.Edit(3);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void edit_get_by_id()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            IActionResult result = controller.Edit(1);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Domain.Entities.System model =
                Assert.IsAssignableFrom<Domain.Entities.System>(viewResult.ViewData.Model);
            Assert.Equal("App 1", model.Name);
        }

        [Fact]
        public void edit_add_id_is_zero()
        {
            var controller = new SystemController(repository);

            IActionResult result = controller.Edit(0);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Domain.Entities.System model =
                Assert.IsAssignableFrom<Domain.Entities.System>(viewResult.ViewData.Model);
            Assert.NotNull(model.Name);
            Assert.Equal(0, model.SystemId);
        }

        [Fact]
        public void edit_post_add()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };

            IActionResult result = controller.Edit(entity);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(2, repository.GetAll().Count());
            Assert.Equal("App 2", repository.GetById(2).Name);
        }

        [Fact]
        public void edit_post_update()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "Test" };

            IActionResult result = controller.Edit(entity);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Test", repository.GetById(2).Name);
        }

        [Fact]
        public void edit_post_removing_spaces_for_the_name_field()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            var entity = new Domain.Entities.System() { SystemId = 0, Name = "  App 2  " };

            IActionResult result = controller.Edit(entity);

            Assert.Equal(entity.Name.Trim(), repository.GetById(0).Name);
        }

        [Fact]
        public void the_name_field_is_required()
        {
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "" };
            var validationContext = new ValidationContext(entity, null, null);
            var validationResult = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            Assert.False(valid);
            var failure = Assert.Single(validationResult, x => x.ErrorMessage == "The Name field is required.");
            Assert.Single(failure.MemberNames, x => x == "Name");
        }

        [Fact]
        public void remove_post()
        {
            var controller = new SystemController(repository);
            repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            IActionResult result = controller.Delete(1);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("System", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, repository.GetAll().Count());
        }
    }
}
