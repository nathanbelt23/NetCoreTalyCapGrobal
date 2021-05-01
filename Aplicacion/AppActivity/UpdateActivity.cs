using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.AppActivity
{
    public class UpdateActivity
    {

        public class UpdateActivityParameters : IRequest<ResponseOperations>
        {
            public string Title { get; set; }

            public DateTime DueDate { get; set; }

            public bool Completed { get; set; }

            public int ActividadID { get; set; }
        }
        public class Validations : AbstractValidator<UpdateActivityParameters>
        {

            public Validations()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
                RuleFor(x => x.DueDate).NotEmpty().WithMessage("Please enter the DueDate");
               
            }
        }
        public class Manejador : IRequestHandler<UpdateActivityParameters, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(UpdateActivityParameters request, CancellationToken cancellationToken)
            {


                Activity activityEnt = _context.Activity.Where(p=> p.ActividadID== request.ActividadID).FirstOrDefault();

                if (activityEnt == null)
                {
                    return new ResponseOperations() { Ok = false, Message = "We have not found the activity ", Id = 0 };
                }
                else
                {
                    activityEnt.Completed = request.Completed;
                    activityEnt.DueDate = request.DueDate;
                    activityEnt.Title = request.Title;
           

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    return new ResponseOperations() { Ok = true, Message = "The activity was updated.", Id = activityEnt.ActividadID };
                }

                else
                {
                    return new ResponseOperations() { Ok = false, Message = "activity cannot be updated", Id = activityEnt.ActividadID };
                }

                }

            }

      
        }

    }

    
}
