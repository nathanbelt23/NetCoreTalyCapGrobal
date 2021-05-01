using System;
using System.Text;
using System.Collections.Generic;
using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Aplicacion.ManejadorError;

namespace Aplicacion.AppAuthor
{
    public class SelectAuthor
    {
        public class ParametersSelectAuthor : IRequest<List<Author>>
        {

        }

        public class Manejador : IRequestHandler<ParametersSelectAuthor, List<Author>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }
            public async Task<List<Author>> Handle(ParametersSelectAuthor request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Author
                    .Include(P => P.BooksLnk)
                    .ThenInclude(p => p.Book)
                    .ToListAsync();
            }

        }

    }
}
