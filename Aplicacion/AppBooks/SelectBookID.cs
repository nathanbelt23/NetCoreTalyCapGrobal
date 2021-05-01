using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.AppBooks
{
    public  class SelectBookID
    {

        public class ParametersSelectBookID : IRequest<List<Book>>
        {
            public int id { get; set; }
        }


        public class Validations : AbstractValidator<ParametersSelectBookID>
        {
            public Validations()
            {
                RuleFor(x => x.id).GreaterThan(-1).WithMessage("The id must be greater than or equal to zero");
            }

            private object RuleFor(Func<object, object> p)
            {
                throw new NotImplementedException();
            }
        }

        public class Manejador : IRequestHandler<ParametersSelectBookID, List<Book>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }
            public async Task<List<Book>> Handle(ParametersSelectBookID request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Book.Where(p=> p.BookID==request.id).ToListAsync();
            }

        }


    }
}
