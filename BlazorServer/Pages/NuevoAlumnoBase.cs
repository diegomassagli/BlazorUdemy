using BlazorInputFile;
using BlazorServer.Servicios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ModeloClasesAlumnos;




using System.IO;

namespace BlazorServer.Pages
{
    public class NuevoAlumnoBase : ComponentBase
    {
        [Inject]
        public IServicioAlumnos ServicioAlumnos { get; set; }  // injecto servicio alumnos para buscar en la base
        [Inject]
        public NavigationManager navigationManager { get; set; }  // injecto navigation manager para hacer redireccion

        public Alumno alumno = new Alumno();
        public IFileListEntry fichero;

        public void HandleValidSubmit()
        {
            Console.WriteLine("OnValidSubmit");
        }

        protected async Task Guardar()
        {
            try
            {
                alumno.FechaAlta = DateTime.Now;
                if (alumno.Nombre != null && alumno.Email != null && alumno.Foto != null)
                {
                    var ms = new MemoryStream();
                    await fichero.Data.CopyToAsync(ms);  // volcamos el contenido del fichero en el memory stream
                    string nombreFichero = "images/" + Guid.NewGuid() + ".jpg";
                    using (FileStream file = new FileStream("wwwroot/" + nombreFichero, FileMode.Create, FileAccess.Write))
                        ms.WriteTo(file);   // guardamos en disco el memory stream con el nombre armado

                    alumno.Foto = nombreFichero;


                    alumno = (await ServicioAlumnos.AltaAlumno(alumno));
                    navigationManager.NavigateTo("/listaAlumnos");
                }
            }
            catch (Exception ex)
            {
                // Guardaremos excepcion en log y mostrarla por pantalla de momento relanzo
                throw new Exception(ex.Message);
            }
        }

        protected void Cancelar()
        {
            navigationManager.NavigateTo("/listaAlumnos");
        }


        public void HandleFileSelected(IFileListEntry[] ficheros)
        {
            fichero = ficheros[0];
            string extension = Path.GetExtension(fichero.Name);
            if (extension == ".jpg")
                alumno.Foto = fichero.Name;
            else
                fichero = null;
        }



    }
}
