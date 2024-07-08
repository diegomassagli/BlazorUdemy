﻿using APIAlumnos.Repositorio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ModeloClasesAlumnos;


namespace APIAlumnos.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly IRepositorioCursos cursosRepositorio;

        public CursosController(IRepositorioCursos cursosRepositorio)
        {
            this.cursosRepositorio = cursosRepositorio;
        }

        //[HttpGet]
        //public async Task<ActionResult> ObtenerCursos()
        //{
        //    try
        //    {
        //        return Ok(await cursosRepositorio.ObtenerCursos());
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo los datos");
        //    };
        //}


        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerCurso(int id)
        {
            try
            {
                var resultado = await cursosRepositorio.ObtenerCurso(id);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo el dato");
            };
        }

        [HttpGet("{id:int}/{idprecio:int}")]
        public async Task<ActionResult<Curso>>ObtenerCurso(int id, int idprecio)
        {
            try
            {
                var resultado = await cursosRepositorio.ObtenerCurso(id, idprecio);
                if(resultado == null)
                {
                    return NotFound();
                }
                return resultado;
            }
            catch
            {

            }
        }

        [HttpGet("{nombreCurso}")]
        public async Task<ActionResult> ObtenerCurso(string nombreCurso)
        {
            try
            {
                var resultado = await cursosRepositorio.ObtenerCurso(nombreCurso);
                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo el dato");
            };
        }


        [Microsoft.AspNetCore.Mvc.Route("~/API/AlumnosCursos/{idAlumno}")]
        public async Task<ActionResult> ObtenerCursos(int idAlumno)
        {
            try
            {
                return Ok(await cursosRepositorio.ObtenerCursos(idAlumno));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error obteniendo los datos");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Curso>> CrearCurso(Curso curso)
        {
            try
            {
                if (curso == null || curso.ListaPrecios == null)  // como minimo al dar de alta un curso tiene que tener por lo menos 1 precio
                    return BadRequest();

                var cursoAux = await cursosRepositorio.ObtenerCurso(curso.NombreCurso);
                if (cursoAux != null)
                {
                    ModelState.AddModelError("NombreCurso", "El curso ya esta dado de alta");
                    return BadRequest(ModelState);
                }

                var nuevoCurso = await cursosRepositorio.AltaCurso(curso);
                return nuevoCurso;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error dando de alta nuevo curso");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<Curso>> ModificarCurso(int id, Curso curso)
        {
            try
            {
                if (id != curso.Id)            // controla que coincida el id de la url con el que viene en el cuerpo
                    return BadRequest("Curso Id no coincide");

                var cursoModificar = await cursosRepositorio.ObtenerCurso(id);
                if (cursoModificar == null)
                    return NotFound($"Curso con = {id} no encontrado");

                return await cursosRepositorio.ModificarCurso(curso);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error actualizando datos");
            };
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> BorrarCurso(int id)
        {
            try
            {
                var cursoABorrar = await cursosRepositorio.ObtenerCurso(id);
                if (cursoABorrar == null)
                    return NotFound($"Curso con = {id} no encontrado");

                return await cursosRepositorio.BorrarCurso(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error eliminando datos");
            };
        }


    }
}
