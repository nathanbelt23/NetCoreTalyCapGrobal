using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.AppActivity
{
    public class CreateActivity
    {

        public class CreateActivityParameters : IRequest<ResponseOperations>
        {
            public string Title { get; set; }

            public DateTime DueDate { get; set; }

            public bool Completed { get; set; }
        }
        public class Validations : AbstractValidator<CreateActivityParameters>
        {

            public Validations()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
                RuleFor(x => x.DueDate).NotEmpty().WithMessage("Please enter the DueDate");
                RuleFor(x => x.Completed).NotEmpty().WithMessage("Please enter the Completed");
            }
        }
        public class Manejador : IRequestHandler<CreateActivityParameters, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(CreateActivityParameters request, CancellationToken cancellationToken)
            {

                var activity = new Activity()
                {


                Completed = request.Completed,
                DueDate = request.DueDate,
                Title = request.Title
                };

                _context.Add(activity);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    return new ResponseOperations() { Ok = true, Message = "The activity was created.", Id = activity.ActividadID };
                }

                else
                {
                    return new ResponseOperations() { Ok = false, Message = "activity cannot be inserted", Id = activity.ActividadID };
                }

            }
        }

    }
}
