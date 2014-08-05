using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Seguridad.ControlAcceso;

namespace Models.Seguridad.ControlAcceso
{
    public class UsuarioLogin
    {
        public int idUsuario;
        public int idCatalogoProveedores;
        public String nombre;
        public String esEmpleado;
        public String contraseña;
        public String estado;
        public PersonaLogin persona = new PersonaLogin();
        public DateTime tiempoExpiracion;
        public int linkCliked;
        public List<PerfilLogin> Perfiles = new List<PerfilLogin>();
        public List<GrupoLogin> Grupos = new List<GrupoLogin>();
        public SistemasModulos sistemasModulos = new SistemasModulos();
    }
}