using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Dominio
{
   public  class Author
    {

        [Key]
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public bool Seleccionado { get; set; }
        public ICollection<AuthorBook> BooksLnk { get; set; }

    }
}
