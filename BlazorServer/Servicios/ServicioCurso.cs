using ModeloClasesAlumnos;
using System.Runtime.ConstrainedExecution;

namespace BlazorServer.Servicios
{
    public class ServicioCurso : IServicioCurso
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ServicioCurso> log;

        public ServicioCurso(HttpClient httpClient, ILogger<ServicioCurso> l)
        {
            this.httpClient = httpClient;
            this.log = l;
        }


        public async Task<IEnumerable<Curso>> ObtenerCursos(int id)
        {
            return await httpClient.GetFromJsonAsync<Curso[]>($"API/AlumnosCursos/{id}");
        }


        public async Task<Curso>AltaCurso(Curso curso)
        {
            var response = await httpClient.PostAsJsonAsync("API/Cursos/", curso);  // se envia un POST con el objeto curso serializado como Json
            if (response.IsSuccessStatusCode)
            {
                Curso cur = await response.Content.ReadFromJsonAsync<Curso>();
                if (cur?.error != null && cur.error.mensaje != string.Empty)
                {
                    if (cur.error.mostrarUsuario)
                    {
                        log.LogError("Error dando de alta curso: " + cur.error.mensaje);
                        throw new Exception(cur.error.mensaje);
                    }
                    else
                    {
                        log.LogError("Error dando de alta curso " + cur.error.mensaje);
                        throw new Exception("Error dando de alta curso");
                    }
                }
                return cur;
            }
            else
            {
                // Manejar el error, podrías lanzar una excepción o retornar un valor por defecto
                throw new Exception($"Error al agregar el curso: {response.ReasonPhrase}");
            }
           
        }

        public async Task<Curso> ObtenerCurso(int IdCurso, int idPrecio)
        {
            return await httpClient.GetFromJsonAsync<Curso>("API/Cursos/" + IdCurso.ToString() + "/" + idPrecio.ToString());
        }


        public async Task<Curso> ModificarCurso(int id, Curso curso)
        {
            var response = await httpClient.PutAsJsonAsync<Curso>("API/Cursos/" + id.ToString(), curso);
            
            if (!response.IsSuccessStatusCode)
            {
                // Manejar el error, podrías lanzar una excepción o retornar un valor por defecto
                throw new Exception($"Error al modificar el curso: {response.ReasonPhrase}");
            }

            Curso cur = await response.Content.ReadFromJsonAsync<Curso>();
            if (cur == null)
            {
                // Manejar el caso donde el contenido no se pueda deserializar
                throw new Exception("El contenido de la respuesta es nulo o no se puede deserializar en un objeto Curso.");
            }
            return cur;
        }

        public async Task BorrarCurso(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"API/Cursos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.SeeOther)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    log.LogError("Error borrando curso " + error);
                    // una vez que arme el log con toda la informacion que necesito
                    // relanzo la excepcion pero solo con la info de "error"
                    throw new Exception(error);
                }
                else
                {
                    log.LogError("Error borrando nuestro curso " + response.ReasonPhrase);
                    throw new Exception("Se produjo un error borrando curso");
                }
            }

        }




    }
    
}
