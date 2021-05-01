using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using FluentValidation;
using Persistencia;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Aplicacion.AppAuthor
{
    public class CreateAuthor
    {
        public class CreateAuthorParameters : IRequest<ResponseOperations>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

        }

        public class Validations : AbstractValidator<CreateAuthorParameters>
        {

            public Validations()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter the FirstName");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter the LastName");
            }
        }

        public class Manejador : IRequestHandler<CreateAuthorParameters, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(CreateAuthorParameters request, CancellationToken cancellationToken)
            {

                var author = new Author()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                _context.Add(author);

        
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    return new ResponseOperations() { Ok = true, Message = "The author was created.", Id = author.AuthorID };
                }

                else
                {
                    return new ResponseOperations() { Ok = false, Message = "Author cannot be inserted", Id = author.AuthorID };
                }

            }
        }
    }
}
