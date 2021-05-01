using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.AppAuthor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await Mediator.Send(new SelectAuthor.ParametersSelectAuthor());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Author>>> Get(int id)
        {
            return await Mediator.Send(new SelectAuthorID.ParametersSelectAuthorID { id = id });
        }


        [HttpPost]
        public async Task<ActionResult<ResponseOperations>> Insert(CreateAuthor.CreateAuthorParameters data)
        {
            return await Mediator.Send(data);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseOperations>> Update(int id, UpdateAuthor.UpdateAuthorParametros data)
        {

            data.AuthorID = id;
            return await Mediator.Send(data);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseOperations>> Delete(int Id)
        {
            return await Mediator.Send(new DeleteAuthor.ParametersDeleteAuthor() { Id = Id });
        }

    }
}
