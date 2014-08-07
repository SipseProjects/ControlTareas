using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Seguridad.ControlAcceso
{
    public class PersonaLogin
    {
        public int idPersona;
        public int idUsuario;
        public String nombre;
        public String apellido;
        public DateTime fechaRegistro;
        public String tecnologias;
        public String estado;
        public String email;
        public long telefono;
        public String empresa;
    }
}