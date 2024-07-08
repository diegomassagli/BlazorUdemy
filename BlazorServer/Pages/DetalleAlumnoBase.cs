using BlazorServer.Servicios;
using Microsoft.AspNetCore.Components;
using ModeloClasesAlumnos;

namespace BlazorServer.Pages
{
    public class DetalleAlumnoBase : ComponentBase
    {
        [Inject]
        public IServicioAlumnos ServicioAlumnos { get; set; }   // injectamos el servicio que consulta la api

        [Parameter]
        public string Id { get; set; }                          // indicamos que vamos a recibir por parametro el Id

        public Alumno alumno { get; set; } = new Alumno();      // creamos un objeto del tipo alumno que va a contener la respuesta de la api


        protected override async Task OnInitializedAsync()       // cuando se carga el componente le decimos que consulta la api
        {
            alumno = (await ServicioAlumnos.ObtenerAlumno(Convert.ToInt32(Id)));
        }
    }
}
