using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;

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
        private bool IsSystemExist(int id)
        {
            return _systemRepository.GetById(id) != null;
        }

        [NonAction]
        private IActionResult RedirectToSystems()
        {
            return RedirectToAction("Index", "System");
        }

        public IActionResult Index(int id)
        {
            return IsSystemExist(id) ? View(_roleRepository.All.Where(p => p.SystemId == id)) : RedirectToSystems();
        }

        public IActionResult Create(int id)
        {
            if (!IsSystemExist(id))
            {
                return RedirectToSystems();
            }

            var entity = new Role()
            {
                RoleId = 0,
                Name = string.Empty,
                SystemId = id,
                System = _systemRepository.GetById(id)
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

            return RedirectToAction("Index", "Role");
        }

        public IActionResult Edit(int id)
        {
            var entity = _roleRepository.GetById(id);
            return entity != null ? (IActionResult) View(entity) : RedirectToAction("Index", "Role");
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

            return RedirectToAction("Index", "Role");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entity = _roleRepository.GetById(id);
            if (entity == null)
            {
                return RedirectToAction("Index", "Role");
            }

            _roleRepository.Remove(id);
            return RedirectToAction("Index", "Role");
        }
    }
}
