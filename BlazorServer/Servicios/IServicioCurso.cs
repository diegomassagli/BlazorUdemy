using ModeloClasesAlumnos;

namespace BlazorServer.Servicios
{
    public interface IServicioCurso
    {
        Task<IEnumerable<Curso>> ObtenerCursos(int id);

        Task<Curso> AltaCurso(Curso curso);
    }
}
