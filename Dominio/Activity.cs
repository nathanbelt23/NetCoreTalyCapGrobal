using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio
{
    public class Activity
    {
        [Key]
        public int ActividadID { get; set; }

        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool Completed { get; set; }

    }



}
