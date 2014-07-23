using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectosWeb
{
    public partial class CapturaTareas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



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
    }
}