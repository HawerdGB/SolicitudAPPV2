using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SolicitudesAppV2.Models
{
    public class Estado: BaseModelo
    {   [MaxLength(50)]
        [Required]
        public string NombEstado{ get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }

        public ICollection<Solicitud> Solicitudes { get; set; }
    }
}
