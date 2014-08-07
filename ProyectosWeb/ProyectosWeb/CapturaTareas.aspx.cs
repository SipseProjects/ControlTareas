using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace ProyectosWeb
{
    public partial class CapturaTareas : System.Web.UI.Page
    {
        public static Stopwatch stopWatch = new Stopwatch();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 
                
            }


        }
         
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            Boolean fileOK = false;
            String path = Server.MapPath("~/UploadedExcels/");
            if (FileUploadTareas.HasFile)
            {
                String fileExtension =
                    System.IO.Path.GetExtension(FileUploadTareas.FileName).ToLower();
                String[] allowedExtensions = { ".xls", ".xlsx"};
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
            }

            if (fileOK)
            {
                try
                {
                    FileUploadTareas.PostedFile.SaveAs(path
                        + FileUploadTareas.FileName);
                    LabelSubir.Text = "Tareas cargadas con exito";
                }
                catch (Exception ex)
                {
                    LabelSubir.Text = "Error en carga de tareas";
                }
            }
            else
            {
                LabelSubir.Text = "No se aceptan archivos de este tipo";
            }
        }

        protected void tm1_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {

                TimeSpan ts = stopWatch.Elapsed;
                this.Label1.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            }
        }

        protected void ButtonIniciar_Click(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
                ButtonIniciar.Text = "Iniciar";
                
            }
            else
            {
                stopWatch.Start();
                ButtonIniciar.Text = "Detener";
            }
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {

            Label1.Text = "00:00:00";
            stopWatch.Reset();
            
        
        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {

        }

    }
}