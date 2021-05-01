using Dominio;
using MediatR;
using Persistencia;
using System.Collections.Generic;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.AppBooks
{
  public  class SelectBook
    {

        public class ParametersSelectBook : IRequest<List<Book>>
        {

        }

        public class Manejador : IRequestHandler<ParametersSelectBook, List<Book>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }
            public async Task<List<Book>> Handle(ParametersSelectBook request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Book
                    .Include(p=>p.AuthorLnk)
                    .ThenInclude(p=>p.Author)
                    .Include(p=>p.CoverPhoto)
                    .ToListAsync();
            }

        }

    }
}
