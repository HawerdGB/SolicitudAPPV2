using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SolicitudesAppV2.Models
{
    public class Solicitud: BaseModelo
    {
        public int PersonaID { get; set; }

        public int EstadoID { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }

        public Estado Estado { get; set; }

        public Persona Persona { get; set; }
    }
}
