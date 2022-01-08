using FileStorageBLL.Interfaces;
using FileStorageDAL;
using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;

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
                if (file.IsPublic)
                {
                    var result = File(System.IO.File.ReadAllBytes(file.RelativePath), $"{file.Extension}", $"{file.Name}");
                    return result;
                }
                return Unauthorized("Please ask owner for access to this file.");
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
        [HttpPost("upload/{userName}")]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile, string userName)
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
                    _fileService.AddFile(uploadedFile, userName);
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
        /// <returns>Files, that belongs to a specific user</returns>
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
        [HttpGet("search/{query?}")]
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
        [HttpGet("private/{userId}")]
        public IEnumerable<StorageFile> GetPrivateFilesByUser(string userId)
        {
            return _fileService.GetPrivateFilesByUser(userId);
        }

        /// <summary>
        /// Gets all public files by user
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Returns all public files for a particular user</returns>
        [HttpGet("public/{userId}")]
        public IEnumerable<StorageFile> GetPublicFilesByUser(string userId)
        {
            return _fileService.GetPublicFilesByUser(userId);
        }

        /// <summary>
        /// Set file status as public
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="IsPublic">Bool result of file status</param>
        /// <returns>File status update confirmation</returns>
        [HttpPost("setpublic/{fileId}")]
        public async Task<IActionResult> SetFilePublic(int fileId, [FromBody]bool IsPublic)
        {
            try
            {
                await _fileService.SetFilePublic(fileId, IsPublic);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Set file status as private
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="IsPrivate">Bool result of file status</param>
        /// <returns>Change file status to private</returns>
        [HttpPost("setprivate/{fileId}")]
        public async Task<IActionResult> SetFilePrivate(int fileId, [FromBody]bool IsPrivate)
        {
            try
            {
                await _fileService.SetFilePrivate(fileId, IsPrivate);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
