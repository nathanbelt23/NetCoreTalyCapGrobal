using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using Dominio;
using FluentValidation;

namespace Aplicacion.AppBooks
{
    public   class DeleteBook
    {
        public class ParametersDeleteBook : IRequest<ResponseOperations>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<ParametersDeleteBook, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }



            public async Task<ResponseOperations> Handle(ParametersDeleteBook request, CancellationToken cancellationToken)
            {
                var book = await _context.Book.FindAsync(request.Id);
                if (book == null)
                {
                    return new ResponseOperations() { Ok = false, Message = "We have not found the book ", Id = 0 };
                }
                else
                {

                    _context.Book.Remove(book);

                    var item = await _context.SaveChangesAsync();
                    if (item == 0)
                    {
                        return new ResponseOperations() { Ok = false, Message = "We have not found the book ", Id = 0 };
                    }
                    else
                    {
                        return new ResponseOperations() { Ok = true, Message = "The book was deleted successfully  ", Id = 0 };
                    }

                }
            }
        }

    }
}
