using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.AppActivity
{
    public class SelectActivity
    {
        public class ParametersSelectActivity : IRequest<List<Activity>>
        {

        }

        public class Manejador : IRequestHandler<ParametersSelectActivity, List<Activity>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }
            public async Task<List<Activity>> Handle(ParametersSelectActivity request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Activity.ToListAsync();
            }

        }


    }
}
