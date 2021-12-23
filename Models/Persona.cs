using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SolicitudesAppV2.Models
{
    public class Persona: BaseModelo
    {   [MaxLength(50)]
        [Required]
        public string Nombre{ get; set; }

        [MaxLength(50)]
        [Required]
        public string Apellido { get; set; }

        [MaxLength(20)]
        [Required]
        public string Pasaporte { get; set; }

        [MaxLength(100)]
        [Required]
        public string Direccion { get; set; }


        [MaxLength(1)]
        [Required]
        public string Sexo { get; set; }

        public string NombreCompleto { get { return Nombre + " " + Apellido; } }

        public ICollection<Solicitud> Solicitudes { get; set; }



    }
}
