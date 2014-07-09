using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectosWeb.Models
{
    public class Perfil
    {
        public int  idPerfil;
        public int idGrupo;
        public int idCatalogoPerfil;
        public String nombre;
        public String descripcion;
        public String usuarioAlta;
        public String usuarioBaja;
        public String usuarioModifica;
        public String estado;
    }
}