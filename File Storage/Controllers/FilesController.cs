using FileStorageBLL.Interfaces;
using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace File_Storage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesController"/>.
        /// </summary>
        /// <param name="appEnvironment"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="fileService"></param>
        public FilesController(IWebHostEnvironment appEnvironment,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _appEnvironment = appEnvironment;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        /// <summary>
        /// Download specific file from the server
        /// </summary>
        /// <param name="fileId">Id of expected file to download</param>
        /// <returns>Downloading confirmation message</returns>
        [HttpGet("download/{fileId}")]
        public IActionResult ShareFile(int fileId)
        {
            try
            {
                var file = _unitOfWork.StorageFiles.GetByIdAsync(fileId).Result;
                if (file == null)
                    return NotFound("There is no file in the system with id :" + $"{fileId}");
                var result = File(System.IO.File.ReadAllBytes(file.RelativePath), $"{file.Extension}", $"{file.Name}");
                return result;
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Adds file to the database
        /// </summary>
        /// <param name="uploadedFile">File to be uploaded to the server</param>
        /// <returns>Uploding confirmation message</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null)
                {
                    string path = "../FileStorageDAL/Files/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    _fileService.AddFile(uploadedFile);
                }
                return Ok("File Uploaded");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Gets files for specified user
        /// </summary>
        /// <param name="userId">Id of specified user</param>
        /// <returns>Files, that belongs to specific user</returns>
        [HttpGet("{userId}")]
        public IEnumerable<StorageFile> GetFilesByUser(string userId)
        {
            try
            {
                return _fileService.GetFilesByUser(userId);
            }
            catch (Exception e)
            {
                return (IEnumerable<StorageFile>)NotFound(e.Message);
            }
        }

        /// <summary>
        /// Gets all files from the database
        /// </summary>
        /// <returns>Files from the database</returns>
        [HttpGet("search/{query}")]
        public IEnumerable<StorageFile> GetAllFiles(string query)
        {
            return _fileService.GetAllFiles(query);
        }

        /// <summary>
        /// Delete file by specified file id
        /// </summary>
        /// <param name="id">id of file that expected to be deleted</param>
        /// <returns>Empty result as a confirmation of successful file deletion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var file = await _unitOfWork.StorageFiles.GetByIdAsync(id);
                if (file == null)
                    return BadRequest("File not found");
                System.IO.File.Delete(file.RelativePath);
                _fileService.DeleteFile(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
