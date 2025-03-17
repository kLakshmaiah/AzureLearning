using AzureLearning.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AzureLearning.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService) 
        { 
            _containerService = containerService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<string> containers =await _containerService.GetContainers();
            return View(containers);
        }
        public IActionResult AddContainer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddContainer(string containerName)
        {
            await _containerService.AddContainer(containerName);
            return View();
        }
    }
}
