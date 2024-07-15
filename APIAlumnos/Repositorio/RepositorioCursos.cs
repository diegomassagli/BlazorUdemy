using APIAlumnos.Datos;
using ModeloClasesAlumnos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Update;
using System.Data;


namespace APIAlumnos.Repositorio
{
    public class RepositorioCursos : IRepositorioCursos // aca decimos que esta clase implementa esta interfaz
    {
        private string CadenaConexion;

        public RepositorioCursos(AccesoDatos cadenaConexion)  // aca paso un objeto de tipo AccesoDatos y paso la cadena conexion.

        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }

        public async Task<Curso> AltaCurso(Curso curso)
        {
            Curso cursoCreado = null;
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            int idCursoCreado = -1;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoAltaCurso";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;

                int cont = 0;
                while (cont < curso.ListaPrecios.Count)
                {
                    if (curso.ListaPrecios == null || curso.ListaPrecios[cont] == null || curso.ListaPrecios[cont].FechaInicio == null || curso.ListaPrecios[cont].FechaFin == null)
                        throw new Exception("Para dar de alta un curso debes enviar precio, fecha inicio y fecha fin");

                    Comm.Parameters.Clear();
                    Comm.Parameters.Add("@NombreCurso", System.Data.SqlDbType.VarChar, 500).Value = curso.NombreCurso;
                    Comm.Parameters.Add("@Coste", System.Data.SqlDbType.Float).Value = curso.ListaPrecios[cont].Coste;
                    Comm.Parameters.Add("@FechaInicio", System.Data.SqlDbType.DateTime).Value = curso.ListaPrecios[cont].FechaInicio;
                    Comm.Parameters.Add("@FechaFin", System.Data.SqlDbType.DateTime).Value = curso.ListaPrecios[cont].FechaFin;
                    // reader = await Comm.ExecuteReaderAsync(); al hacerlo para varios registros, no se comporta bien, por eso uso un
                    idCursoCreado = Convert.ToInt32(await Comm.ExecuteScalarAsync());
                    cont++;
                }
                if (idCursoCreado != -1)
                {
                    cursoCreado = await ObtenerCurso(idCursoCreado);

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creado los datos del Curso" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return cursoCreado;
        }

        public async Task<bool> BorrarCurso(int id)
        {
            Curso cursoBorrado = null;                                    // lo hago asi para instanciarlo solo si existe el curso
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoBorrarCurso";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@IdCurso", System.Data.SqlDbType.Int).Value = id;
                reader = await Comm.ExecuteReaderAsync();
                return true;
            }
            // saca el catch para que se maneje en el controlador
            //catch (SqlException ex)
            //{
            //    throw new Exception("Error borrando Curso " + ex.Message);
            //}
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }

        public async Task<Curso> ObtenerCurso(int id)
        {
            Curso curso = null;

            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoObtenerCursos";
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (curso == null)
                    {
                        curso = new Curso();
                        curso.Id = Convert.ToInt32(reader["idCurso"]);
                        curso.NombreCurso = reader["NombreCurso"].ToString();
                        curso.ListaPrecios = new List<Precio>();
                    }

                    //Añadimos los posibles precios del curso
                    Precio aux = new Precio();
                    aux.Id = Convert.ToInt32(reader["idPrecio"]);
                    aux.Coste = Convert.ToDouble(reader["Coste"]);
                    aux.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                    aux.FechaFin = Convert.ToDateTime(reader["FechaFin"]);

                    curso.ListaPrecios.Add(aux);

                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos de nuestro curso " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return curso;
        }

        public async Task<Curso> ObtenerCurso(int id, int idprecio)
        {
            Curso curso = null;

            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoObtenerCursos";
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Parameters.Add("@id", SqlDbType.Int).Value = id;
                Comm.Parameters.Add("@idPrecio", SqlDbType.Int).Value = idprecio;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (curso == null)
                    {
                        curso = new Curso();
                        curso.Id = Convert.ToInt32(reader["idCurso"]);
                        curso.NombreCurso = reader["NombreCurso"].ToString();
                        curso.ListaPrecios = new List<Precio>();
                    }

                    //Añadimos los posibles precios del curso
                    Precio aux = new Precio();
                    aux.Id = Convert.ToInt32(reader["idPrecio"]);
                    aux.Coste = Convert.ToDouble(reader["Coste"]);
                    aux.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                    aux.FechaFin = Convert.ToDateTime(reader["FechaFin"]);

                    curso.ListaPrecios.Add(aux);

                }

            }
            catch (SqlException ex)
            {
                throw new Exception("Error cargando los datos de nuestro curso " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return curso;
        }

        public async Task<Curso> ObtenerCurso(string nombreCurso)
        {            
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;            
            Boolean hayRegistros;
            Curso curso = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoObtenerCursos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@NombreCurso", System.Data.SqlDbType.VarChar, 500).Value = nombreCurso;
                reader = await Comm.ExecuteReaderAsync();
                hayRegistros = reader.Read();
                if (hayRegistros)
                {
                    curso = new Curso();
                    curso.Id = Convert.ToInt32(reader["idCurso"]);
                    curso.NombreCurso = reader["NombreCurso"].ToString();
                    curso.ListaPrecios = new List<Precio>();                

                    while (hayRegistros)
                    {
                        Precio aux = new Precio();
                        aux.Id = Convert.ToInt32(reader["idPrecio"]);
                        aux.Coste = Convert.ToDouble(reader["Coste"]);
                        aux.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                        aux.FechaFin = Convert.ToDateTime(reader["FechaFin"]);
                        curso.ListaPrecios.Add(aux);
                        hayRegistros = reader.Read();                        
                    }                    
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Cargando los datos de nuestros Cursos " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return curso;

        }

        public async Task<IEnumerable<Curso>> ObtenerCursos(int idAlumno)
        {
            List<Curso> listaCursos = new List<Curso>();
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            int cursoAnt = 0;
            Boolean hayRegistros;
            Curso curso = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoObtenerCursos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                if (idAlumno != -1)
                    Comm.Parameters.Add("@idalumno", System.Data.SqlDbType.Int).Value = idAlumno;

                reader = await Comm.ExecuteReaderAsync();
                hayRegistros = reader.Read();       
                while (hayRegistros)
                {
                    if (cursoAnt != Convert.ToInt32(reader["idCurso"]))
                    {
                        cursoAnt = Convert.ToInt32(reader["idCurso"]);
                        curso = new Curso();
                        curso.Id = Convert.ToInt32(reader["idCurso"]);
                        curso.NombreCurso = reader["NombreCurso"].ToString();
                        curso.ListaPrecios = new List<Precio>();
                    }

                    while ((hayRegistros) && (cursoAnt == Convert.ToInt32(reader["idCurso"])) )
                    {
                        Precio aux = new Precio();
                        aux.Id = Convert.ToInt32(reader["idPrecio"]);
                        aux.Coste = Convert.ToDouble(reader["Coste"]);
                        aux.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                        aux.FechaFin = Convert.ToDateTime(reader["FechaFin"]);
                        curso.ListaPrecios.Add(aux);
                        hayRegistros = reader.Read();
                        if (!hayRegistros) break;
                    }

                    listaCursos.Add(curso);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Cargando los datos de nuestros Cursos " + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return listaCursos;
        }

        public async Task<Curso> ModificarCurso(Curso curso)
        {
            Curso cursoModificado = null;
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                int cont = 0;
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.CursoModificarCurso";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;

                while (cont < curso.ListaPrecios.Count) 
                {
                    Comm.Parameters.Clear();
                    Comm.Parameters.Add("@idCurso", System.Data.SqlDbType.Int).Value = curso.Id;
                    if (curso.ListaPrecios[cont].Id>0)  // esto va como nulo cuando estoy creando un precio nuevo
                        Comm.Parameters.Add("@idPrecio", System.Data.SqlDbType.Int).Value = curso.ListaPrecios[cont].Id;
                    Comm.Parameters.Add("@NombreCurso", System.Data.SqlDbType.VarChar, 500).Value = curso.NombreCurso;                    
                    Comm.Parameters.Add("@Coste", System.Data.SqlDbType.Float).Value = curso.ListaPrecios[cont].Coste;
                    Comm.Parameters.Add("@FechaInicio", System.Data.SqlDbType.DateTime).Value = curso.ListaPrecios[cont].FechaInicio;
                    Comm.Parameters.Add("@FechaFin", System.Data.SqlDbType.DateTime).Value = curso.ListaPrecios[cont].FechaFin;
                    await Comm.ExecuteNonQueryAsync();
                    cont++;
                }
                cursoModificado = await ObtenerCurso(curso.Id);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando los datos de nuestro Curso" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (Comm != null)
                    Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return cursoModificado;

        }     
    }
}
