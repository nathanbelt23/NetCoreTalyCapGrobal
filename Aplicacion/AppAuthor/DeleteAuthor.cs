using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using Dominio;
using FluentValidation;

namespace Aplicacion.AppAuthor
{
    public class DeleteAuthor
    {


        public class ParametersDeleteAuthor : IRequest<ResponseOperations>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<ParametersDeleteAuthor, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }



            public async Task<ResponseOperations> Handle(ParametersDeleteAuthor request, CancellationToken cancellationToken)
            {
                var author = await _context.Author.FindAsync(request.Id);
                if (author == null)
                {
                    return new ResponseOperations() { Ok = false, Message = "We have not found the author ", Id = 0 };
                }
                else
                {

                    _context.Author.Remove(author);

                    var item = await _context.SaveChangesAsync();
                    if (item == 0)
                    {
                        return new ResponseOperations() { Ok = false, Message = "We have not found the author ", Id = 0 };
                    }
                    else
                    {
                        return new ResponseOperations() { Ok = true, Message = "The author was deleted successfully  ", Id = 0 };
                    }

                }
            }
        }


    }

}

