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

namespace Aplicacion.AppActivity
{
    public class SelectActivityID
    {
        public class ParametersSelectActivityID : IRequest<List<Activity>>
        {
            public int id { get; set; }
        }

        public class Validations : AbstractValidator<ParametersSelectActivityID>
        {
            public Validations()
            {
                RuleFor(x => x.id).GreaterThan(-1).WithMessage("The id must be greater than or equal to zero");
            }
        }


        public class Manejador : IRequestHandler<ParametersSelectActivityID, List<Activity>>
        {

            BooksOnlineContext _booksOnlineContext;
            public Manejador(BooksOnlineContext booksOnlineContext)
            {
                _booksOnlineContext = booksOnlineContext;
            }

            public async Task<List<Activity>> Handle(ParametersSelectActivityID request, CancellationToken cancellationToken)
            {
                return await _booksOnlineContext.Activity.Where(p => p.ActividadID == request.id).ToListAsync();
            }
        }

    }
}
