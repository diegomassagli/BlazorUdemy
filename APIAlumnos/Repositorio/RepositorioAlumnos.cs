using APIAlumnos.Datos;
using ModeloClasesAlumnos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.AspNetCore.Mvc;


namespace APIAlumnos.Repositorio
{
    public class RepositorioAlumnos : IRepositorioAlumnos // aca decimos que esta clase implementa esta interfaz
    {
        private string CadenaConexion;

        public RepositorioAlumnos(AccesoDatos cadenaConexion)  // aca paso un objeto de tipo AccesoDatos y paso la cadena conexion.

        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }

        public async Task<Alumno> AltaAlumno(Alumno alumno)
        {
            Alumno alumnoCreado = null;
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioAltaAlumno";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 500).Value = alumno.Nombre;
                Comm.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 500).Value = alumno.Email;
                Comm.Parameters.Add("@Foto", System.Data.SqlDbType.VarChar, 500).Value = alumno.Foto;
                Comm.Parameters.Add("@FechaAlta", System.Data.SqlDbType.DateTime).Value = alumno.FechaAlta;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                {
                    alumnoCreado = await ObtenerAlumno(Convert.ToInt32(reader["idAlumno"]));

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creado los datos de nuestro Alumno" + ex.Message);
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

            return alumnoCreado;
        }

        public async Task<Alumno> BorrarAlumno(int id)
        {
            Alumno alumnoBorrado = null;                                    // lo hago asi para instanciarlo solo si existe el alumno
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioMarcarBaja";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@IdAlumno", System.Data.SqlDbType.Int).Value = id;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                {
                    alumnoBorrado = await ObtenerAlumno(Convert.ToInt32(reader["idalumno"]));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error borrando Alumno " + ex.Message);
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

            return alumnoBorrado;


        }

        public async Task<Alumno> ObtenerAlumno(int id)
        {
            Alumno alumno = null;                                    // lo hago asi para instanciarlo solo si existe el alumno
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioObtenerAlumnos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                {
                    alumno = new Alumno();
                    alumno.Id = Convert.ToInt32(reader["id"]);
                    alumno.Nombre = reader["Nombre"].ToString();
                    alumno.Email = reader["Email"].ToString();
                    alumno.Foto = reader["Foto"].ToString();
                    alumno.FechaAlta = Convert.ToDateTime(reader["FechaAlta"].ToString());
                    if (reader["FechaBaja"] != System.DBNull.Value)
                        alumno.FechaBaja = Convert.ToDateTime(reader["FechaBaja"].ToString());
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Cargando los datos de nuestro Alumno " + ex.Message);
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

            return alumno;

        }

        public async Task<Alumno> ObtenerAlumno(string email)   // se usa solo internamente para verificar que el email no exista, pero no tiene un endpoint externo
        {
            Alumno alumno = null;                                    // lo hago asi para instanciarlo solo si existe el alumno
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioObtenerAlumnos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 500).Value = email;
                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                {
                    alumno = new Alumno();
                    alumno.Id = Convert.ToInt32(reader["id"]);
                    alumno.Nombre = reader["Nombre"].ToString();
                    alumno.Email = reader["Email"].ToString();
                    alumno.Foto = reader["Foto"].ToString();
                    alumno.FechaAlta = Convert.ToDateTime(reader["FechaAlta"].ToString());
                    if (reader["FechaBaja"] != System.DBNull.Value)
                        alumno.FechaBaja = Convert.ToDateTime(reader["FechaBaja"].ToString());
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Cargando los datos de nuestro Alumno " + ex.Message);
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

            return alumno;

        }

        public async Task<IEnumerable<Alumno>> ObtenerAlumnos()
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioObtenerAlumnos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Alumno alu = new Alumno();
                    alu.Id = Convert.ToInt32(reader["id"]);
                    alu.Nombre = reader["Nombre"].ToString();
                    alu.Email = reader["Email"].ToString();
                    alu.Foto = reader["Foto"].ToString();
                    alu.FechaAlta = Convert.ToDateTime(reader["FechaAlta"].ToString());
                    if (reader["FechaBaja"] != System.DBNull.Value)
                        alu.FechaBaja = Convert.ToDateTime(reader["FechaBaja"].ToString());
                    lista.Add(alu);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Cargando los datos de nuestros Alumnos " + ex.Message);
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

            return lista;
        }

        public async Task<Alumno> ModificarAlumno(Alumno alumno)
        {
            Alumno alumnoModificado = null;
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioAltaAlumno";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@IdAlumno", System.Data.SqlDbType.Int).Value = alumno.Id;
                Comm.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 500).Value = alumno.Nombre;
                Comm.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 500).Value = alumno.Email;
                Comm.Parameters.Add("@Foto", System.Data.SqlDbType.VarChar, 500).Value = alumno.Foto;
                if (alumno.FechaBaja != null)
                    Comm.Parameters.Add("@FechaBaja", System.Data.SqlDbType.DateTime).Value = alumno.FechaBaja;

                reader = await Comm.ExecuteReaderAsync();
                if (reader.Read())
                    alumnoModificado = await ObtenerAlumno(Convert.ToInt32(reader["idAlumno"])); // aca, con el Id que devolvio Sql a traves del SP UsuarioAltaAlumno, recupero los datos completos del alumno modificado
            }
            catch (SqlException ex)
            {
                throw new Exception("Error modificando los datos de nuestro Alumno" + ex.Message);
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

            return alumnoModificado;

        }

        public async Task<IEnumerable<Alumno>> BuscarAlumnos(string texto)
        {
            List<Alumno> AlumnosEncontrados = new List<Alumno>();
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            SqlDataReader reader = null;
            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioBuscarAlumnos";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@texto", System.Data.SqlDbType.VarChar, 500).Value = texto;
                reader = await Comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Alumno alu = new Alumno();
                    alu.Id = Convert.ToInt32(reader["id"]);
                    alu.Nombre = reader["Nombre"].ToString();
                    alu.Email = reader["Email"].ToString();
                    alu.Foto = reader["Foto"].ToString();
                    alu.FechaAlta = Convert.ToDateTime(reader["FechaAlta"].ToString());
                    if (reader["FechaBaja"] != System.DBNull.Value)
                        alu.FechaBaja = Convert.ToDateTime(reader["FechaBaja"].ToString());
                    AlumnosEncontrados.Add(alu);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error buscando los datos de nuestros Alumnos " + ex.Message);
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

            return AlumnosEncontrados;

        }
             

        // inscribe alumno en curso
        public async Task<Alumno> InscribirAlumnoCurso(Alumno alumno, int idCurso, int idPrecio)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand Comm = null;
            Alumno alumnoInscripto = null;

            try
            {
                sqlConexion.Open();
                Comm = sqlConexion.CreateCommand();
                Comm.CommandText = "dbo.UsuarioInscribirCurso";
                Comm.CommandType = System.Data.CommandType.StoredProcedure;
                Comm.Parameters.Add("@idAlumno", System.Data.SqlDbType.Int).Value = alumno.Id;
                Comm.Parameters.Add("@idCurso", System.Data.SqlDbType.Int).Value = idCurso;
                Comm.Parameters.Add("@idPrecio", System.Data.SqlDbType.Int).Value = idPrecio;

                await Comm.ExecuteNonQueryAsync();  // se usa cuando no devuelve valores
                alumnoInscripto = await ObtenerAlumno(alumno.Id);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creado los datos de nuestro Alumno" + ex.Message);
            }
            finally
            {
                Comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return alumnoInscripto;

        }


        // devuelve los datos de un alumno y todos sus cursos
        public async Task<Alumno> AlumnoCursos(int idAlumno)
        {
            Alumno alumno = null;
            int idCursoAux = -1;
            Curso cursoActual = null;

            using (SqlConnection sqlConexion = conexion())
            {
                await sqlConexion.OpenAsync();

                using (SqlCommand comm = new SqlCommand("dbo.UsuarioInscriptoCursos", sqlConexion))
                {
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    comm.Parameters.Add("@idAlumno", System.Data.SqlDbType.Int).Value = idAlumno;

                    using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (alumno == null)
                            {
                                alumno = new Alumno
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Foto = reader.GetString(reader.GetOrdinal("Foto")),
                                    ListaCursos = new List<Curso>()
                                };
                            }

                            int idCurso = reader.GetInt32(reader.GetOrdinal("idCurso"));
                            if (idCursoAux != idCurso)
                            {
                                if (cursoActual != null)
                                    alumno.ListaCursos.Add(cursoActual);

                                cursoActual = new Curso
                                {
                                    Id = idCurso,
                                    NombreCurso = reader.GetString(reader.GetOrdinal("NombreCurso")),
                                    ListaPrecios = new List<Precio>()
                                };

                                idCursoAux = idCurso;
                            }

                            cursoActual.ListaPrecios.Add(new Precio
                            {
                                Coste = reader.GetDouble(reader.GetOrdinal("Coste")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin"))
                            });
                        }

                        if (cursoActual != null)
                            alumno.ListaCursos.Add(cursoActual);
                    }
                }
            }

            return alumno;
        }
    }
}
