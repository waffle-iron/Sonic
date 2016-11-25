using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;
using Sonic.WebUI.Models;

namespace Sonic.WebUI.Controllers
{
    public class RoleController : Controller
    {
        private readonly ICrudRepository<Role> _roleRepository;
        private readonly ICrudRepository<Domain.Entities.System> _systemRepository;

        public RoleController(ICrudRepository<Role> roleRepository, ICrudRepository<Domain.Entities.System> systemRepository)
        {
            _roleRepository = roleRepository;
            _systemRepository = systemRepository;
        }

        [NonAction]
        private IActionResult RedirectToSystems()
        {
            return RedirectToRoute("default", new { action = "Index", controller = "System" });
        }

        public IActionResult Index(int id)
        {
            var model = new RoleModel {System = _systemRepository.GetById(id)};

            if (model.System == null)
            {
                return RedirectToSystems();
            }

            model.Roles = _roleRepository.All.Where(p => p.SystemId == id).OrderBy(p => p.Name);
            return View(model);
        }

        public IActionResult Create(int id)
        {
            var system = _systemRepository.GetById(id);
            if (system == null)
            {
                return RedirectToSystems();
            }

            var entity = new Role()
            {
                RoleId = 0,
                Name = string.Empty,
                SystemId = id,
                System = system
            };
            return View(entity);
        }

        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            role.Name = role.Name.Trim();
            _roleRepository.Add(role);

            return RedirectToRoute("default", new { controller = "Role", action = "Index", id = role.SystemId });

        }

        public IActionResult Edit(int id)
        {
            var entity = _roleRepository.GetById(id);
            return entity == null ? RedirectToSystems() : View(entity);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            role.Name = role.Name.Trim();
            _roleRepository.Update(role);

            return RedirectToRoute("default", new { controller = "Role", action = "Index", id = role.SystemId });

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entity = _roleRepository.GetById(id);
            if (entity == null)
            {
                return RedirectToSystems();
            }

            _roleRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "Role", action = "Index", id = entity.SystemId });

        }
    }
}
