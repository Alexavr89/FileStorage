using FileStorageDAL;
using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace File_Storage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileStorageDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesController"/>.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="context"></param>
        /// <param name="appEnvironment"></param>
        /// <param name="userManager"></param>
        public FilesController(IUnitOfWork unitOfWork, FileStorageDbContext context,
            IWebHostEnvironment appEnvironment, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        /// <summary>
        /// Download specific file from the server
        /// </summary>
        /// <param name="fileId">Id of expected file to download</param>
        /// <returns>Downloading confirmation message</returns>
        [HttpGet("download/{fileId}")]
        public IActionResult DownloadFile(int fileId)
        {
            try
            {
                var file = _context.StorageFiles.Where(x => x.Id == fileId).FirstOrDefault();
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
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
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
                    StorageFile file = new StorageFile
                    {
                        Name = uploadedFile.FileName,
                        RelativePath = path,
                        Created = DateTime.Now,
                        Size = uploadedFile.Length,
                        Extension = uploadedFile.ContentType,
                        //ApplicationUser = _userManager.Users.FirstOrDefault(),
                        IsPublic = true,
                        Id = _context.StorageFiles.Count() + 1
                    };
                    await _context.StorageFiles.AddAsync(file);
                    await _context.SaveChangesAsync();
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
                var user = _userManager.FindByIdAsync(userId).Result;
                var files = _context.StorageFiles.Where(file => file.ApplicationUser == user);
                return files;
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
            var files = _context.StorageFiles.ToList();
            if (query == null)
            {
                return files;
            }
            return files.Where(x => x.Name.ToLower().Contains(query.ToLower()));
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
                var file = await _context.StorageFiles.FindAsync(id);
                if (file == null)
                    return BadRequest("File not found");
                System.IO.File.Delete(file.RelativePath);
                _context.StorageFiles.Remove(file);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
