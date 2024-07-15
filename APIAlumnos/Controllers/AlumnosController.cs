using APIAlumnos.Repositorio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ModeloClasesAlumnos;


namespace APIAlumnos.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly IRepositorioAlumnos alumnosRepositorio;
        private readonly ILogger<AlumnosController> log;

        public AlumnosController(IRepositorioAlumnos alumnosRepositorio, ILogger<AlumnosController> l)
        {
            this.alumnosRepositorio = alumnosRepositorio;
            this.log = l;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerAlumnos()
        {
            try
            {
                return Ok(await alumnosRepositorio.ObtenerAlumnos());
            }
            catch (Exception ex) 
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo ObtenerAlumnos: " + ex.ToString());
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
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo ObtenerAlumno por Id: " + ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo el dato");
            };
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> CrearAlumno(Alumno alumno)
        {
            Alumno nuevoAlumno = new Alumno();
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

                nuevoAlumno = await alumnosRepositorio.AltaAlumno(alumno);
            }
            catch (SqlException ex)
            {
                nuevoAlumno.error = new Error();
                log.LogError("Se produjo un error en el controlador de Alumnos en el método AltaAlumno:" + ex.ToString());
                nuevoAlumno.error.mensaje = "Error creando alumno " + ex.Message;
                nuevoAlumno.error.mostrarUsuario = true;

            }
            catch (Exception ex)
            {
                nuevoAlumno.error = new Error();
                log.LogError("Se produjo un error en el controlador de Alumnos en el método AltaAlumno:" + ex.ToString());
                nuevoAlumno.error.mensaje = ex.ToString();
                nuevoAlumno.error.mostrarUsuario = false;
            }
            return nuevoAlumno;
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
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo ObtenerAlumno por Id: " + ex.ToString());
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
            catch (SqlException ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo BorrarAlumno: " + ex.ToString());
                return StatusCode(StatusCodes.Status303SeeOther, ex.Message);
            }
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo BorrarAlumno: " + ex.ToString());
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
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo BuscarAlumnos por Nombre: " + ex.ToString());
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
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo ObtenerAlumno por Id: " + ex.ToString());
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
            catch (Exception ex)
            {
                log.LogError("Se produjo un error en el controlador de Alumnos, en el metodo CursoAlumno/IdAlumno: " + ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo cursos alumno");
            }
        }



    }
}
