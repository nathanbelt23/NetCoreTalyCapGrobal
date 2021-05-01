using Dominio;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Persistencia;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aplicacion.AppAuthor
{
    public class SelectAuthorBook
    {
        public class ParametersSelectAuthorID : IRequest<List<Author>>
        {
            public int id { get; set; }
        }


        public class Validations : AbstractValidator<ParametersSelectAuthorID>
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
                if (request.id == 0)
                {
                    return await _booksOnlineContext.Author.ToListAsync();
                }
                else
                {

                        var resultadoPP = await _booksOnlineContext.AuthorBook.Where(p => p.BookID == request.id).ToListAsync();
                      var  resultadosP = await _booksOnlineContext.Author.ToListAsync();

                    var result = (from p in resultadosP
                                  join pp in resultadoPP on p.AuthorID equals pp.AuthorID
                                  into ps
                                  from pp in ps.DefaultIfEmpty()
                                  select new Author { AuthorID = p.AuthorID, FirstName = p.FirstName,  LastName=p.LastName , Seleccionado = pp == null ? false : true }
                    ).ToList()
                    ;

                    return result;
                }
            }
        }


    }
}
