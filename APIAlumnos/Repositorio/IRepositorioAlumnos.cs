using Microsoft.AspNetCore.Mvc;
using ModeloClasesAlumnos;

namespace APIAlumnos.Repositorio
{
    public interface IRepositorioAlumnos
    {
        Task<Alumno> AltaAlumno(Alumno Alumno);

        Task<IEnumerable<Alumno>> ObtenerAlumnos();

        Task<Alumno> ObtenerAlumno(int id);

        Task<Alumno> ObtenerAlumno(string email);

        Task<Alumno> ModificarAlumno(Alumno Alumno);

        Task<Alumno> BorrarAlumno(int id);

        Task<IEnumerable<Alumno>> BuscarAlumnos(string texto);

        // Inscribir Alumnos en un curso
        Task<Alumno> InscribirAlumnoCurso(Alumno alumno, int idCurso, int idPrecio);

        // Devuelve los datos de un alumno y todos sus cursos
        Task<Alumno> AlumnoCursos(int idAlumno);

    }
}
