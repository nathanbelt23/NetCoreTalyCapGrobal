using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.AppActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ActivityController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> Get()
        {
            return await Mediator.Send(new SelectActivity.ParametersSelectActivity());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Activity>>> Get(int id)
        {
            return await Mediator.Send(new SelectActivityID.ParametersSelectActivityID { id = id });
        }


        [HttpPost]
        public async Task<ActionResult<ResponseOperations>> Insert(CreateActivity.CreateActivityParameters data)
        {
            return await Mediator.Send(data);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseOperations>> Update(int id, UpdateActivity.UpdateActivityParameters data)
        {

            data.ActividadID = id;
            return await Mediator.Send(data);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseOperations>> Delete(int Id)
        {
            return await Mediator.Send(new DeleteActivity.ParametersDeleteActivity() { Id = Id });
        }

    }
}
