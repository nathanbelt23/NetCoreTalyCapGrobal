using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
   public class AuthorBook
    {

        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }

    }
}
