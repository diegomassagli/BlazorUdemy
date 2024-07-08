 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloClasesAlumnos
{
    public class Precio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* El campo Coste es Obligatorio")]
        public double Coste { get; set; }

        [Required(ErrorMessage = "* El campo FechaInicio es Obligatorio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "* El campo FechaFin es Obligatorio")]
        public DateTime FechaFin { get; set; }

    }
}
