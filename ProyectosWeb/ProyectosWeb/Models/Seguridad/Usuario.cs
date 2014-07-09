using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectosWeb.Models
{
    public class Usuario
    {
        public int idUsuario;
        public int idCatalogoProveedores;
        public String nombre;
        public String esEmpleado;
        public String contraseña;
        public String estado;
        public Persona persona= new Persona();
        public DateTime tiempoExpiracion;
        public int linkCliked;

    }
}