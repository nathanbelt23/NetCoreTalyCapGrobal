using Aplicacion.Contratos;
using Dominio;
using System;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Aplicacion.Seguridad
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };


            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes("Mi palabra secreta"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion= new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(claims),
                Expires= DateTime.Now.AddDays(30),
                SigningCredentials= credentials

            };


            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);
            return tokenManejador.WriteToken(token);


        }
    }
}
