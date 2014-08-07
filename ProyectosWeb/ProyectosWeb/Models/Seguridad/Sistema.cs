using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectosWeb.Models.Seguridad
{
    public class Sistema
    {
        public int idSistema;
        public string clave;
        public string nombre;
        public string descripcion;
        public string cliente;
        public DateTime fechaRegistro;
        public string fechaInicio;
        public string fechaFinEstimada;
        public string fechaFinReal;
        public string tecnologias;
        public int estado;
      
    }
}