using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sonic.Domain.Entities;
using Sonic.Domain.Abstract;

namespace Sonic.WebUI.Controllers
{
    public class MethodController : Controller
    {
        private readonly ICrudRepository<Method> _methodRepository;
        private readonly ICrudRepository<Domain.Entities.System> _systemRepository;

        public MethodController(ICrudRepository<Method> methodRepository, ICrudRepository<Domain.Entities.System> systemRepository)
        {
            _methodRepository = methodRepository;
            _systemRepository = systemRepository;
        }

        public IActionResult Index(int id)
        {
            return View(_methodRepository.All.Where(p => p.SystemId == id));            
        }
    }
}
