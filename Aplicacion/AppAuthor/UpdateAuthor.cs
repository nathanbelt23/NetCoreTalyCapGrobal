using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using Dominio;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.AppAuthor
{
    public class UpdateAuthor
    {
        public class UpdateAuthorParametros : IRequest<ResponseOperations>
        {

            public int AuthorID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

        }

        public class Validacion : AbstractValidator<UpdateAuthorParametros>
        {

            public Validacion()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter the FirstName");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter the LastName");
            }
        }

        public class Manejador : IRequestHandler<UpdateAuthorParametros, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(UpdateAuthorParametros request, CancellationToken cancellationToken)
            {

                var author = await _context.Author.FindAsync(request.AuthorID);
                if (author == null)
                {
                    return new ResponseOperations() { Ok = false, Message = "We have not found the author ", Id = 0};
                }
                else
                {
                    author.FirstName = request.FirstName;
                    author.LastName = request.LastName;
               



                    var valor = await _context.SaveChangesAsync();
                    if (valor > 0)
                    {
                        return new ResponseOperations() { Ok = true, Message = "The author was updated.", Id = author.AuthorID };
                    }
                    else
                    {
                        return new ResponseOperations() { Ok = false, Message = "Author cannot be saved. ", Id = author.AuthorID };
                    }

                }
            }
        }
    }
}
