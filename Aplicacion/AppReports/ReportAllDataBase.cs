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

namespace Aplicacion.AppReports
{
    public class ReportAllDataBase
    {

        public class ParametersReports : IRequest<List<ReportesExcel>>
        {

        }


        public class Validations : AbstractValidator<ParametersReports>
        {
            public Validations()
            {

            }
        }


        public class Manejador : IRequestHandler<ParametersReports, List<ReportesExcel>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }

            public async Task<List<ReportesExcel>> Handle(ParametersReports request, CancellationToken cancellationToken)
            {

                var result = await (from a in _booksOnlineContext.AuthorBook
                                    join b in _booksOnlineContext.Author on a.AuthorID equals b.AuthorID
                                    join c in _booksOnlineContext.Book on a.BookID equals c.BookID

                                    select new ReportesExcel
                                    {
                                        FirstName = b.FirstName,
                                        LastName = b.LastName,
                                        BookID = c.BookID,
                                        Title = c.Title,
                                        Description = c.Description,
                                        PageCount = c.PageCount,
                                        Excerpt = c.Excerpt,
                                        PublishDate = c.PublishDate

                                    }).ToListAsync();

                return result;


            }
        }


    }
}
