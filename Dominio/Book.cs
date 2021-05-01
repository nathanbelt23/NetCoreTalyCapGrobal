using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Book
    {

        public int BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string Excerpt { get; set; }
        public DateTime PublishDate { get; set; }
        public ICollection<AuthorBook> AuthorLnk   { get; set; }
        public ICollection<CoverPhoto>   CoverPhoto{ get; set; }


    }
}
