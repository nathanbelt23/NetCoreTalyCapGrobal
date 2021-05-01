using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio
{
    public  class CoverPhoto
    {
        [Key]
        public int CoverID { get; set; }
        public int BookID { get; set; }
        public  string Url { get; set; }
        public Book  Book { get; set; }
    }
}
