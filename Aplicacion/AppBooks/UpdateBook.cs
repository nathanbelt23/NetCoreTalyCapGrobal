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
    public class UpdateBook
    {

        public class UpdateBookParametros : IRequest<ResponseOperations>
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int PageCount { get; set; }
            public string Excerpt { get; set; }
            public DateTime PublishDate { get; set; }
            public ICollection<AuthorBook> AuthorLnk { get; set; }
            public ICollection<CoverPhoto> CoverPhoto { get; set; }

        }

        public class Validacion : AbstractValidator<UpdateBookParametros>
        {

            public Validacion()
            {
                RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter the Description");
                RuleFor(x => x.PageCount).GreaterThan(0).WithMessage("Please enter the PageCount greater than zero");
                RuleFor(x => x.Excerpt).NotEmpty().WithMessage("Please enter the Excerpt");
              
            }
        }

        public class Manejador : IRequestHandler<UpdateBookParametros, ResponseOperations>
        {
            BooksOnlineContext _context;
            public Manejador(BooksOnlineContext context)
            {
                _context = context;
            }


            public async Task<ResponseOperations> Handle(UpdateBookParametros request, CancellationToken cancellationToken)
            {
                try
                {
                    var book = await _context.Book.FindAsync(request.BookID);
                    if (book == null)
                    {
                        return new ResponseOperations() { Ok = false, Message = "We have not found the book ", Id = 0 };
                    }
                    else
                    {
                        book.Title = request.Title;
                        book.Description = request.Description+".";
                        book.PageCount = request.PageCount;
                        book.Excerpt = request.Excerpt;
                        book.PublishDate = request.PublishDate;
                        var valor =  _context.SaveChanges();
                        var booklnk = _context.AuthorBook.Where(prop => prop.BookID == request.BookID);

                        foreach (var rem in booklnk)
                        {
                           

                            _context.Entry(rem).State = EntityState.Deleted; // added row
                            _context.AuthorBook.Remove(rem);
                        }
                         _context.SaveChanges();
                        if (request.AuthorLnk != null)
                        {

                            foreach (var id in request.AuthorLnk)
                            {
                                var authorBook = new AuthorBook
                                {
                                    AuthorID = id.AuthorID,
                                    BookID = id.BookID
                                };

                               

                                _context.Entry(authorBook).State = EntityState.Added; // added row
                                _context.AuthorBook.Add(authorBook);
                            }
                            await _context.SaveChangesAsync();
                    
                        }
                  

                        if (valor > 0)
                        {
                            return new ResponseOperations() { Ok = true, Message = "The Book was updated.", Id = book.BookID };
                        }
                        else
                        {
                            return new ResponseOperations() { Ok = false, Message = "Book cannot be saved. ", Id = book.BookID };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ResponseOperations() { Ok = false, Message = e.Message, Id =0 };
                }
            }
        }
    }
}
