using BaseAzureStorage.Services.BlobStorage;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BaseAzureStorage.Controllers.Test.api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpPost]
        public void ListBlobContainers()
        {
            AzureStorageService azureStorageService = new AzureStorageService();

            azureStorageService.ListBlobContainersAsync().GetAwaiter().GetResult();

        }


        [HttpPost]
        public IActionResult DownloadFileAsync([FromForm] string Carpeta, [FromForm] string Archivo)
        {
            AzureStorageService azureStorageService = new AzureStorageService();

            var result = azureStorageService.DownloadFileAsync(Carpeta, Archivo).GetAwaiter().GetResult();

            return File(result.content, result.contentType, result.name);

        }

        [HttpPost]
        public void DeleteFileAsync([FromForm] string Carpeta, [FromForm] string Archivo)
        {
            AzureStorageService azureStorageService = new AzureStorageService();

            azureStorageService.DeleteFileAsync(Carpeta, Archivo).GetAwaiter().GetResult();

        }

        [HttpPost]
        public List<string> ListFileAsync([FromForm] string? Carpeta = null)
        {
            AzureStorageService azureStorageService = new AzureStorageService();

            return azureStorageService.ListFileAsync(Carpeta).GetAwaiter().GetResult();

        }

        [HttpPost]
        public void UploadFileAsync([FromForm] string Carpeta, IFormFile fileDetails)
        {

            AzureStorageService azureStorageService = new AzureStorageService();

            azureStorageService.UploadFileAsync(fileDetails.OpenReadStream(), Carpeta, fileDetails.FileName).GetAwaiter().GetResult();

        }
    }
}
