using ModeloClasesAlumnos;

namespace BlazorServer.Servicios
{
    public interface IServicioAlumnos
    {
        Task<IEnumerable<Alumno>> ObtenerAlumnos();

        Task<Alumno> ObtenerAlumno(int id);

        Task<Alumno> AltaAlumno(Alumno alumno);

        Task<Alumno> ModificarAlumno(int id, Alumno alumno);

        Task BorrarAlumno(int id);

        Task<Alumno> CursosInscriptosAlumno(int id);

        Task<Alumno> CursosInscribirAlumno(Alumno alumno, int idCurso, int idPrecio);
    }
}
