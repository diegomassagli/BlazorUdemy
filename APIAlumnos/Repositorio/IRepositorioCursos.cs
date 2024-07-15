using ModeloClasesAlumnos;

public interface IRepositorioCursos

{   // da de alta un curso y su precio
    Task<Curso> AltaCurso(Curso curso);

    // devuelve todos los cursos y sus precios.
    // si el parametro idalumno viene en -1 devolvemos todos los cursos, sino solo los que no esta inscripto
    Task<IEnumerable<Curso>> ObtenerCursos(int idAlumno);

    // devuelve los datos de un curso por id 
    Task<Curso> ObtenerCurso(int id);

    // devuelve los datos de un curso con su precio buscado por Id
    Task<Curso> ObtenerCurso(int id, int idPrecio);

    // devuelve los datos de un curso por nombre
    Task<Curso> ObtenerCurso(string nombreCurso);
    
    Task<Curso> ModificarCurso(Curso curso);

    Task<bool> BorrarCurso(int id);
    
}

