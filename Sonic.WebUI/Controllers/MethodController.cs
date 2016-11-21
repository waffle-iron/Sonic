using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Entities;
using Sonic.Domain.Abstract;

namespace Sonic.WebUI.Controllers
{
    public class MethodController : Controller
    {
        private readonly ICrudRepository<Method> methodRepository = null;
        private readonly ICrudRepository<Domain.Entities.System> systemRepository = null;

        public MethodController(ICrudRepository<Method> methodRepository, ICrudRepository<Domain.Entities.System> systemRepository)
        {
            this.methodRepository = methodRepository;
            this.systemRepository = systemRepository;
        }

        public IActionResult Index(int id)
        {
            return View(methodRepository.GetAll().Where(p => p.SystemId == id));            
        }
    }
}
