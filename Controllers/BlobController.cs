using AzureLearning.IServices;
using AzureLearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureLearning.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService) 
        { 
            _blobService=blobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            List<string> blobNames =await  _blobService.GetAllBlobsAsync(name);
            return View(blobNames);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlobsWithUrl(string name)
        {
            List<BlobDetails> blobDetails =await _blobService.GetBlobsWithFullUrlAsync(name);
            return View(blobDetails);
        }
        public IActionResult BlobWithTheContainerUrl()
        {
            return View();
        }
        public IActionResult ViewBlobItem(string containerName, string blobName)
        {
            BlobDetails blobDetails= _blobService.GetABlobAsync(containerName, blobName);
            return View(blobDetails);
        }
    }
}
