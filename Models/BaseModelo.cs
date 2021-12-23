using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SolicitudesAppV2.Models
{
    public class BaseModelo
    {   [Key]
        public int ID { get; set; }
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaUltMode { get; set; }
    }
}
