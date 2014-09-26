using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace ProyectosWeb.BusinessLogic.general
{
    public class EnvioEmail
    {
        public void SendMail(String emailde, String emailpara, string id, string nombre, String host)
        {            
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(emailde);
            msg.To.Add(emailpara);
            msg.Body =
            " Este es un email automatico, Favor de no responder. <br/> <br/> "
            + " Hola " + nombre + " <br/> <br/> "
            + " Tus Datos de Control de Tareas. <br/><br/> "
            + " Para recuperar tus datos de Inicio de Sesion para el Control de Tareas, simplemente  <a href='http://" + host + "/Views/Login/Inicio.aspx?id=email&u=" + id + "'>Sigue este link</a> <br/><br/>"
            + " Nota: Este proceso expira en 3 Horas. <br/> <br/> "
            + " Nota: El link es valido solo una vez que se inicia el proceso de recuperacion de datos de Inicio de Sesion. <br/> <br/> ";
            msg.IsBodyHtml = true;
            msg.Subject = "Tus Datos de Control de Tareas";
            SmtpClient smt = new SmtpClient("smtp.gmail.com");
            smt.Port = 587;
            smt.Credentials = new NetworkCredential("llopez@solutiaintelligence.com", "gtleo144");
            smt.EnableSsl = true;
            smt.Send(msg);
        }
    }


}