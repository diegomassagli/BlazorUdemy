using ModeloClasesAlumnos;
using System.Net.Http.Json;

namespace BlazorServer.Servicios
{
    public class ServicioAlumnos : IServicioAlumnos
    {
        private readonly HttpClient httpClient;

        public ServicioAlumnos(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
            Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();

            return alu;
        }

        public async Task<Alumno> ModificarAlumno(int id, Alumno alumno)
        {
            var response = await httpClient.PutAsJsonAsync<Alumno>("API/Alumnos/" + id.ToString(), alumno);
            Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();

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
            Alumno alu = await response.Content.ReadFromJsonAsync<Alumno>();
            return alu;
        }



    }
}
