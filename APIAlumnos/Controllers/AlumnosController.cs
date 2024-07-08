using APIAlumnos.Repositorio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ModeloClasesAlumnos;


namespace APIAlumnos.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly IRepositorioAlumnos alumnosRepositorio;

        public AlumnosController(IRepositorioAlumnos alumnosRepositorio)
        {
            this.alumnosRepositorio = alumnosRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerAlumnos()
        {
            try
            {
                return Ok(await alumnosRepositorio.ObtenerAlumnos());
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo los datos");
            };
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerAlumno(int id)
        {
            try
            {
                var resultado = await alumnosRepositorio.ObtenerAlumno(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo el dato");
            };
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> CrearAlumno(Alumno alumno)
        {
            try
            {
                if (alumno == null)
                    return BadRequest();

                var alumnoAux = await alumnosRepositorio.ObtenerAlumno(alumno.Email);
                if (alumnoAux != null)
                {
                    ModelState.AddModelError("email", "El email ya esta en uso");
                    return BadRequest(ModelState);
                }                    

                var nuevoAlumno = await alumnosRepositorio.AltaAlumno(alumno);
                return nuevoAlumno;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error dando de alta nuevo alumno");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<Alumno>> ModificarAlumno(int id, Alumno alumno)
        {
            try
            {
                if (id != alumno.Id)            // controla que coincida el id de la url con el que viene en el cuerpo
                    return BadRequest("Alumno Id no coincide");

                var alumnoModificar = await alumnosRepositorio.ObtenerAlumno(id);
                if (alumnoModificar == null)
                    return NotFound($"Alumno con = {id} no encontrado");

                return await alumnosRepositorio.ModificarAlumno(alumno);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            };
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Alumno>> BorrarAlumno(int id)
        {
            try
            {
                var alumnoABorrar = await alumnosRepositorio.ObtenerAlumno(id);
                if (alumnoABorrar == null)
                    return NotFound($"Alumno con = {id} no encontrado");

                return await alumnosRepositorio.BorrarAlumno(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error eliminando datos");
            };
        }


        [HttpGet("{BuscarAlumnos}")]
        public async Task<ActionResult> BuscarAlumnos(string texto)
        {
            try
            {
                return Ok(await alumnosRepositorio.BuscarAlumnos(texto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo los datos");
            };
        }


        [Microsoft.AspNetCore.Mvc.Route("InscribirAlumno/{idCurso}/{idPrecio}")]
        public async Task<ActionResult<Alumno>> InscribirAlumnoCurso([FromBody]Alumno alumno, int idCurso, int idPrecio)
        {
            try
            {
                // verificamos que el alumno existe sino sale por exception
                var alumnoValidar = await alumnosRepositorio.ObtenerAlumno(alumno.Id);

                if (alumnoValidar == null)
                    return NotFound($"Alumno no Encontrado");

                return await alumnosRepositorio.InscribirAlumnoCurso(alumno, idCurso, idPrecio);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inscribiendo alumno en curso");
            }

        }



        [Microsoft.AspNetCore.Mvc.Route("CursosAlumno/{idAlumno}")]
        public async Task<ActionResult<Alumno>> AlumnoCursos(int idAlumno)
        {
            try
            {
                Alumno AlumnoRespuesta = null;
                Alumno alumnoValidar = await alumnosRepositorio.ObtenerAlumno(idAlumno);

                if (alumnoValidar == null)
                    return NotFound($"Alumno no encontrado");

                AlumnoRespuesta = await alumnosRepositorio.AlumnoCursos(idAlumno);

                if (AlumnoRespuesta == null)
                    AlumnoRespuesta = alumnoValidar;

                return AlumnoRespuesta;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo cursos alumno");
            }
        }



    }
}
