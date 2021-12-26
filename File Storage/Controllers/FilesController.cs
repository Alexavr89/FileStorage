using FileStorageDAL;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace File_Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public FilesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> DownloadFile()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> UploadFile()
        {
            throw new NotImplementedException();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void DeleteFile(int id)
        {

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<StorageFile> GetFileByName(int id)
        {
            return _unitOfWork.StorageFiles.GetPrivateFileByIdAsync(id);
        }
    }
}
