using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using ModeloClasesAlumnos;
using System.ComponentModel.DataAnnotations;
using BlazorServer.Servicios;
using CurrieTechnologies.Razor.SweetAlert2;


namespace BlazorServer.Pages
{
    public class ListaAlumnosBase: ComponentBase
    {
        [Inject]  // injecto el servicio alumnos
        public IServicioAlumnos ServicioAlumnos { get; set; }
        
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        public SweetAlertService Swal { get; set; }

        public IEnumerable<Alumno> Alumnos { get; set; }
        public bool MostrarPopPup = false;
        public int idAlumnoBorrar = -1;
        public string nombreAlumnoBorrar = "";

        protected override async Task OnInitializedAsync()
        {
            Alumnos = (await ServicioAlumnos.ObtenerAlumnos()).ToList();
        }
        
        protected async Task Borrar(int idAlumno, string nombreAlumno)
        {
            //idAlumnoBorrar = idAlumno;
            //nombreAlumnoBorrar = nombreAlumno;
            //MostrarPopPup = true;
            var respuesta = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = "Desea eliminar ?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Si ok",
                CancelButtonText = "No, mejor no..."
            });
            if (!string.IsNullOrEmpty(respuesta.Value))
            {
                DarDeBaja(idAlumno);
            }
        }

        protected void CerrarPop()
        {
            idAlumnoBorrar = -1;
            MostrarPopPup = false;
            nombreAlumnoBorrar = "";
        }


        protected void DarDeBaja(int id)
        {
            ServicioAlumnos.BorrarAlumno(id);
            CerrarPop();
            navigationManager.NavigateTo("listaAlumnos", true);
        }



    }
}
