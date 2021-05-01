using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio
{

    [NotMapped]
    public class ReportesExcel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string Excerpt { get; set; }
        public DateTime PublishDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
