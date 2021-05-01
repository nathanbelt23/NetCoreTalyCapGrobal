using Dominio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
   public class DataPrueba
    {

        public static async Task InsertarData(BooksOnlineContext cursosOnlineContext,UserManager<Usuario> usuarioManager)
        {

            if (usuarioManager.Users.Count()==1)
            {
                var usuario = new Usuario() {
                    NombreCompleto="Yonathan Beltran Romero",
                    Email="desarrollador@gmail.com" , 
                    UserName= "desarrollador@gmail.com"
                };
                
                await usuarioManager.CreateAsync(usuario, "Colombia2021.");
            }

        }
    }
}
