using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using Aplicacion.ManejadorError;
using Persistencia;
using Dominio;
using System.Collections.Generic;

namespace Aplicacion.AppBooks
{
    public class CreateBook
    {

        public class CreateBookParameters : IRequest<ResponseOperations>
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int PageCount { get; set; }
            public string Excerpt { get; set; }
            public DateTime PublishDate { get; set; }

            public ICollection<AuthorBook> AuthorLnk { get; set; }
        }

        public class Validations : AbstractValidator<CreateBookParameters>
        {

            public Validations()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter the Description");
                RuleFor(x => x.PageCount).GreaterThan(0).WithMessage("the number of pages must be greater than 0 ");
                RuleFor(x => x.PublishDate).NotEmpty().WithMessage("Please enter the Publish Date");

            }
        }

        public class Manejador : IRequestHandler<CreateBookParameters, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(CreateBookParameters request, CancellationToken cancellationToken)
            {

                var Book = new Book()
                {
                    Title=request.Title,
                    Description= request.Description,
                    PageCount= request.PageCount,
                    Excerpt= request.Excerpt,
                    PublishDate= request.PublishDate
                };

             



                _context.Add(Book);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    if (request.AuthorLnk != null)
                    {
                        foreach (var id in request.AuthorLnk)
                        {
                            var AuthorBook = new AuthorBook
                            {
                                AuthorID = id.AuthorID,
                                BookID = Book.BookID
                            };
                            _context.Add(AuthorBook);
                        }

                        await _context.SaveChangesAsync();
                    }

              

                return new ResponseOperations() { Ok = true, Message = "The Book was created.", Id = Book.BookID };
                }

                else
                {
                    return new ResponseOperations() { Ok = false, Message = "The Book cannot be inserted", Id = Book.BookID };
                }

    
        }


    }
    }
}
