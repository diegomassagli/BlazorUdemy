using ModeloClasesAlumnos;

namespace BlazorServer.Servicios
{
    public interface IServicioCurso
    {
        Task<IEnumerable<Curso>> ObtenerCursos(int id);

        Task<Curso> AltaCurso(Curso curso);

        Task<Curso> ObtenerCurso(int Id, int idPrecio);

        Task<Curso> ModificarCurso(int Id, Curso curso);

        Task BorrarCurso(int id);
    }
}