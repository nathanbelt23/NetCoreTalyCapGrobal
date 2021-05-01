using Dominio;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Aplicacion.ManejadorError;
using System.Net;
using FluentValidation;
using Aplicacion.Contratos;

namespace Aplicacion.Seguridad
{
    public class Login
    {

        public class Ejecuta : IRequest<UsuarioData>
        {

            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(z => z.Email).NotEmpty();
                RuleFor(z => z.Email).EmailAddress();
                RuleFor(z => z.Password).NotEmpty();
                RuleFor(z => z.Password).MinimumLength(3);
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerator _jwtGenerador;
            //IJwtGenerator _jwtGenerator; 
            public Manejador(UserManager<Usuario> userManager, 
                SignInManager<Usuario> signInManager,
                IJwtGenerator jwtGenerator
                )
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerator;
            }



            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(
                           HttpStatusCode.NotFound,
                            new { Login = "No se encontro el usuario" }
                            );
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync
                    (
                    usuario,
                    request.Password, false
                    );
                if (resultado.Succeeded)
                {

                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                         Token = _jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email  =  usuario.Email

                    };

                   // return usuario;
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);


            }
        }

    }
}
