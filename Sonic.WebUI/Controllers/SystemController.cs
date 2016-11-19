using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;

namespace Sonic.WebUI.Controllers
{
    public class SystemController : Controller
    {
        private readonly ICrudRepository<Domain.Entities.System> systemRepository = null;

        public SystemController(ICrudRepository<Domain.Entities.System> systemRepository)
        {
            this.systemRepository = systemRepository;
        }

        public IActionResult Index()
        {
            return View(systemRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return View(new Domain.Entities.System() { Id = 0, Name = string.Empty });
            }

            Domain.Entities.System entity = systemRepository.GetById(id);
            if (entity == null)
            {
                return RedirectToAction("Index", "System");
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Domain.Entities.System system)
        {
            if (ModelState.IsValid)
            {
                Domain.Entities.System entity = systemRepository.GetById(system.Id);
                system.Name = system.Name.Trim();
                if (entity == null)
                {
                    systemRepository.Add(system);
                }
                else
                {
                    systemRepository.Update(system);
                }

                return RedirectToAction("Index", "System");
            }

            return View(system);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Domain.Entities.System entity = systemRepository.GetById(id);
            if (entity != null)
            {
                systemRepository.Remove(id);
            }

            return RedirectToAction("Index", "System");
        }
    }
}
