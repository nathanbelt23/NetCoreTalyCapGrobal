using Dominio;
using MediatR;
using System.Collections.Generic;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;



namespace Aplicacion.AppAuthor
{
    public  class SelectAuthorID
    {

        public class  ParametersSelectAuthorID:IRequest<List<Author>>
        {
            public int id { get; set; }
        }


        public class Validations:AbstractValidator<ParametersSelectAuthorID>
        {
            public Validations()
            {
                RuleFor(x => x.id).GreaterThan(-1).WithMessage("The id must be greater than or equal to zero");
            }
        }


        public class Manejador : IRequestHandler<ParametersSelectAuthorID, List<Author>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }

            public async Task<List<Author>> Handle(ParametersSelectAuthorID request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Author
                    .Include(P=>P.BooksLnk)
                    .ThenInclude(p=>p.Book)
                    .Where(p => p.AuthorID == request.id)
                    .ToListAsync();
            }
        }

    }
}
