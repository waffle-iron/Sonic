using FluentAssertions;
using FluentAssertions.Mvc;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Tests.Concrete;
using Sonic.WebUI.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sonic.Tests.Concrete.Fakes;
using Xunit;

namespace Sonic.Tests.Controllers
{
    public class SystemControllerTests
    {
        private readonly ICrudRepository<Domain.Entities.System> _repository;

        public SystemControllerTests()
        {
            _repository = new SystemRepositoryFake();
        }

        [Fact]
        public void index_get_all()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            var result = controller.Index();

            result.Should().BeViewResult().ModelAs<IEnumerable<Domain.Entities.System>>().Should().HaveCount(2);
        }

        [Fact]
        public void edit_redirect_to_action_if_id_does_not_exist()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            var result = controller.Edit(3);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("System");
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public void edit_get_by_id()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });

            var result = controller.Edit(1);

            result.Should().BeViewResult().ModelAs<Domain.Entities.System>().Name.Should().Be("App 1");
        }

        [Fact]
        public void edit_add_id_is_zero()
        {
            var controller = new SystemController(_repository);

            var result = controller.Edit(0);

            result.Should().BeViewResult().ModelAs<Domain.Entities.System>().Name.Should().NotBeNull();
            result.Should().BeViewResult().ModelAs<Domain.Entities.System>().SystemId.Should().Be(0);
        }

        [Fact]
        public void edit_post_add()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "App 2" };

            var result = controller.Edit(entity);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("System");
            redirectToActionResult.ActionName.Should().Be("Index");
            _repository.All.Should().HaveCount(2);
            _repository.GetById(2).Name.Should().Be("App 2");
        }

        [Fact]
        public void edit_post_update()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "Test" };

            IActionResult result = controller.Edit(entity);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("System");
            redirectToActionResult.ActionName.Should().Be("Index");
            _repository.GetById(2).Name.Should().Be("Test");
        }

        [Fact]
        public void edit_post_removing_spaces_for_the_name_field()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            var entity = new Domain.Entities.System() { SystemId = 0, Name = "  App 2  " };

            controller.Edit(entity);

            entity.Name.Trim().Should().Be(_repository.GetById(0).Name);
        }

        [Fact]
        public void the_name_field_is_required()
        {
            var entity = new Domain.Entities.System() { SystemId = 2, Name = "" };
            var validationContext = new ValidationContext(entity, null, null);
            var validationResult = new List<ValidationResult>();

            var valid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            valid.Should().BeFalse();
            var failure = Assert.Single(validationResult, x => x.ErrorMessage == "The Name field is required.");
            Assert.Single(failure.MemberNames, x => x == "Name");
        }

        [Fact]
        public void remove_post()
        {
            var controller = new SystemController(_repository);
            _repository.Add(new Domain.Entities.System() { SystemId = 1, Name = "App 1" });
            _repository.Add(new Domain.Entities.System() { SystemId = 2, Name = "App 2" });

            var result = controller.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be("System");
            redirectToActionResult.ActionName.Should().Be("Index");
            _repository.All.Should().HaveCount(1);
        }
    }
}
