using ModeloClasesAlumnos;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;

namespace BlazorServer.Servicios
{
    public class ServicioAlumnos : IServicioAlumnos
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ServicioAlumnos> log;

        public ServicioAlumnos(HttpClient httpClient, ILogger<ServicioAlumnos> l)
        {
            this.httpClient = httpClient;
            this.log = l;
        }

        public async Task<IEnumerable<Alumno>> ObtenerAlumnos()
        {
            return await httpClient.GetFromJsonAsync<Alumno[]>("API/Alumnos");
        }

        public async Task<Alumno> ObtenerAlumno(int id)
        {
            return await httpClient.GetFromJsonAsync<Alumno>("API/Alumnos/" + id.ToString());
        }


        public async Task<Alumno> AltaAlumno(Alumno alumno)
        {
            var response = await httpClient.PostAsJsonAsync("API/Alumnos/", alumno);  // se envia un POST con el objeto alumno serializado como Json
            if (response.IsSuccessStatusCode)
            {
                Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();
                if (alu?.error != null && alu.error.mensaje != string.Empty)
                {
                    if (alu.error.mostrarUsuario)
                    {
                        log.LogError("Error dando de alta alumno: " + alu.error.mensaje);
                        throw new Exception(alu.error.mensaje);
                    }
                    else
                    {
                        log.LogError("Error dando de alta alumno " + alu.error.mensaje);
                        throw new Exception("Error dando de alta alumno");
                    }
                }
                return alu;
            }
            else
            {
                // Manejar el error, podrías lanzar una excepción o retornar un valor por defecto
                throw new Exception($"Error al agregar el alumno: {response.ReasonPhrase}");
            }
        }

        public async Task<Alumno> ModificarAlumno(int id, Alumno alumno)
        {
            var response = await httpClient.PutAsJsonAsync<Alumno>("API/Alumnos/" + id.ToString(), alumno);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar el error, podrías lanzar una excepción o retornar un valor por defecto
                throw new Exception($"Error al modificar el alumno: {response.ReasonPhrase}");
            }
            Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();
            if (alu == null)
            {
                // Manejar el caso donde el contenido no se pueda deserializar
                throw new Exception("El contenido de la respuesta es nulo o no se puede deserializar en un objeto Curso.");
            }
            return alu;
        }

        public async Task BorrarAlumno(int id)
        {
            await httpClient.DeleteAsync($"API/Alumnos/{id}");
        }


        public async Task<Alumno> CursosInscriptosAlumno(int id)
        {
            return await httpClient.GetFromJsonAsync<Alumno>($"API/Alumnos/CursosAlumno/{id}");
        }


        public async Task<Alumno> CursosInscribirAlumno(Alumno alumno, int idCurso, int idPrecio )
        {
            var response = await httpClient.PostAsJsonAsync($"API/Alumnos/InscribirAlumno/{idCurso}/{idPrecio}", alumno );
            if (!response.IsSuccessStatusCode)
            {
                // Manejar el error, podrías lanzar una excepción o retornar un valor por defecto
                throw new Exception($"Error al modificar el alumno: {response.ReasonPhrase}");
            }
            Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();
            if (alu == null)
            {
                // Manejar el caso donde el contenido no se pueda deserializar
                throw new Exception("El contenido de la respuesta es nulo o no se puede deserializar en un objeto Curso.");
            }
            return alu;
        }



    }
}
