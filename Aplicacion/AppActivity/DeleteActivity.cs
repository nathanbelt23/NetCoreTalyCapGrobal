using Dominio;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.AppActivity
{
    public  class DeleteActivity
    {

        public class ParametersDeleteActivity : IRequest<ResponseOperations>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<ParametersDeleteActivity, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }



            public async Task<ResponseOperations> Handle(ParametersDeleteActivity request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activity.FindAsync(request.Id);
                if (activity == null)
                {
                    return new ResponseOperations() { Ok = false, Message = "We have not found the Activity ", Id = 0 };
                }
                else
                {

                    _context.Activity.Remove(activity);

                    var item = await _context.SaveChangesAsync();
                    if (item == 0)
                    {
                        return new ResponseOperations() { Ok = false, Message = "We have not found the Activity ", Id = 0 };
                    }
                    else
                    {
                        return new ResponseOperations() { Ok = true, Message = "The Activity was deleted successfully  ", Id = 0 };
                    }

                }
            }
        }


    }
}
