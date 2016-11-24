﻿using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Abstract;

namespace Sonic.WebUI.Controllers
{
    public class SystemController : Controller
    {
        private readonly ICrudRepository<Domain.Entities.System> _systemRepository;

        public SystemController(ICrudRepository<Domain.Entities.System> systemRepository)
        {
            _systemRepository = systemRepository;
        }

        public IActionResult Index()
        {
            return View(_systemRepository.All);
        }

        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            var entity = id != 0 ? _systemRepository.GetById(id) : new Domain.Entities.System() {SystemId = 0, Name = string.Empty};
            if (entity == null)
            {
                return RedirectToAction("Index", "System");
            }

            return View(entity);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Domain.Entities.System system)
        {
            if (!ModelState.IsValid)
                return View(system);

            system.Name = system.Name.Trim();

            if (system.SystemId == 0)
            {
                _systemRepository.Add(system);
            }
            else
            {
                _systemRepository.Update(system);
            }

            return RedirectToAction("Index", "System");
        }

        [ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var entity = _systemRepository.GetById(id);
            if (entity == null)
            {
                return RedirectToAction("Index", "System");
            }

            return View("Delete", entity);
        }

        [ActionName("Delete"), HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var entity = _systemRepository.GetById(id);
            if (entity != null)
            {
                _systemRepository.Remove(id);
            }

            return RedirectToAction("Index", "System");
        }
    }
}
