using ModeloClasesAlumnos;

namespace BlazorServer.Servicios
{
    public class ServicioCurso : IServicioCurso
    {
        private readonly HttpClient httpClient;

        public ServicioCurso(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<IEnumerable<Curso>> ObtenerCursos(int id)
        {
            return await httpClient.GetFromJsonAsync<Curso[]>($"API/AlumnosCursos/{id}");
        }


        public async Task<Curso>AltaCurso(Curso curso)
        {
            var response = await httpClient.PostAsJsonAsync("API/Cursos/", curso);  // se envia un POST con el objeto curso serializado como Json
            Curso cur = await response.Content.ReadFromJsonAsync<Curso>();

            return cur;
        }
    }
    
}
