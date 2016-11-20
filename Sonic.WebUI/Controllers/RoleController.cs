using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;
using Sonic.Domain.Entities;

namespace Sonic.WebUI.Controllers
{
    public class RoleController : Controller
    {
        private readonly ICrudRepository<Role> roleRepository = null;
        private readonly ICrudRepository<Domain.Entities.System> systemRepository = null;

        public RoleController(ICrudRepository<Role> roleRepository, ICrudRepository<Domain.Entities.System> systemRepository)
        {
            this.roleRepository = roleRepository;
            this.systemRepository = systemRepository;
        }

        [NonAction]
        private bool IsSystemExist(int id)
        {
            Domain.Entities.System entity = systemRepository.GetById(id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [NonAction]
        private IActionResult RedirectToSystems()
        {
            return RedirectToAction("Index", "System");
        }

        public IActionResult Index(int id)
        {
            if (IsSystemExist(id))
            {
                return View(roleRepository.GetAll().Where(p => p.SystemId == id));
            }
            else
            {
                return RedirectToSystems();
            }
        }

        public IActionResult Create(int id)
        {
            if (IsSystemExist(id))
            {
                var entity = new Role()
                {
                    RoleId = 0,
                    Name = string.Empty,
                    SystemId = id,
                    System = systemRepository.GetById(id)
                };
                return View(entity);
            }
            else
            {
                return RedirectToSystems();
            }
        }

        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                role.Name = role.Name.Trim();
                roleRepository.Add(role);

                return RedirectToAction("Index", "Role");
            }

            return View(role);
        }

        public IActionResult Edit(int id)
        {
            Role entity = roleRepository.GetById(id);
            if (entity == null)
            {
                return RedirectToAction("Index", "Role");
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                role.Name = role.Name.Trim();
                roleRepository.Update(role);
                return RedirectToAction("Index", "Role");
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Role entity = roleRepository.GetById(id);
            if(entity != null)
            {
                roleRepository.Remove(id);
            }

            return RedirectToAction("Index", "Role");
        }
    }
}
