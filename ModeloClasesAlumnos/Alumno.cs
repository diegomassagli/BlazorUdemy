using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloClasesAlumnos
{
    public class Alumno
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "* El campo nombre es obligatorio")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "* El campo email es obligatorio")]
        [EmailAddress(ErrorMessage = "* El campo email es incorrecto")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "* El campo foto es obligatorio")]
        public string Foto { get; set; }
        public List<Curso>? ListaCursos { get; set; }

        [Required(ErrorMessage = "* El campo FechaAlta es obligatorio")]
        public DateTime? FechaAlta { get; set; }
        
        public DateTime? FechaBaja { get; set; }
    }
}
