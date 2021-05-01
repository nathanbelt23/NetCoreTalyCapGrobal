using Dominio;
using Aplicacion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {


        [HttpPost("login")]

        [HttpPost]
        public async  Task<ActionResult<UsuarioData>> login(Login.Ejecuta  login)
        {
            return await Mediator.Send(login);

        }

    }
}
