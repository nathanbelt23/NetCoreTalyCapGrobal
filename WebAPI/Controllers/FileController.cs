
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistencia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/file")]
    [ApiController]
    [AllowAnonymous]
    public class FileController : ControllerBase
    {
        public class FileModel
        {
            public string FileName { get; set; }
            public IFormFile FormFile { get; set; }
            //public List<IFormFile> FormFiles { get; set; }
        }

        BooksOnlineContext _booksOnlineContext;
        public FileController(BooksOnlineContext booksOnlineContext)
        {
            _booksOnlineContext = booksOnlineContext;
        }

        public class FileInputModel
        {
            public IFormFile FileToUpload { get; set; }
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {

            var cover =  _booksOnlineContext.CoverPhoto.Where(p => p.CoverID == id).FirstOrDefault();
            string imagen = "noImage.png";

            if (cover != null)
            {
                imagen = cover.Url;
            }

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", imagen);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }



        [HttpPut("{id}")]
        public  ActionResult PutImage(int id, IFormFile file)

        {
            try
            {
                string FileName = Guid.NewGuid() + ".jpg";

                var cover =  _booksOnlineContext.CoverPhoto.Where(p=> p.BookID==id).FirstOrDefault();



                if (cover == null)
                {
                    cover = new Dominio.CoverPhoto();
                    cover.BookID = id;
                    cover.Url = FileName;
                    _booksOnlineContext.Add(cover);
                     _booksOnlineContext.SaveChanges();
                }
                else
                {

                    _booksOnlineContext.CoverPhoto.Remove(cover);
                    _booksOnlineContext.SaveChanges();

                    var coverins = new Dominio.CoverPhoto();
                    coverins.BookID = id;
                    coverins.Url = FileName;
                    _booksOnlineContext.Add(coverins);
                    _booksOnlineContext.SaveChanges();
                }
    
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats" +
                "officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

    }
}