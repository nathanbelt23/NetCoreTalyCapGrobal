using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.AppBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Aplicacion.AppAuthor;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await Mediator.Send(new SelectBook.ParametersSelectBook());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Book>>> Get(int id)
        {
            return await Mediator.Send(new SelectBookID.ParametersSelectBookID { id = id });
        }


        [HttpGet("Author/{id}")]
        public async Task<ActionResult<List<Author>>> GetAuthor(int id)
        {
            return await Mediator.Send(new SelectAuthorBook.ParametersSelectAuthorID { id = id });
        }


        [HttpPost]
        public async Task<ActionResult<ResponseOperations>> Insert(CreateBook.CreateBookParameters data)
        {
            return await Mediator.Send(data);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseOperations>> Update(int id, UpdateBook.UpdateBookParametros data)
        {

            data.BookID = id;
            return await Mediator.Send(data);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseOperations>> Delete(int Id)
        {
            return await Mediator.Send(new DeleteBook.ParametersDeleteBook() { Id = Id });
        }

        [HttpGet("Report")]
        public async Task<ActionResult<List<ReportesExcel>>> GetReport()
        {
            return await Mediator.Send(new Aplicacion.AppReports.ReportAllDataBase.ParametersReports());
        }
    }
}