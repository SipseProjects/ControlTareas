using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class GlobalDataSingleton
    {
        
        private static GlobalDataSingleton instance;

        private GlobalDataSingleton() { }

        public static GlobalDataSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalDataSingleton();
                }
                return instance;
            }
        }

        public int sistemaID { get; set; }
        public int noEliminar { get; set; }
        public String controlAcceso { get; set; }
        public DateTime expirarTimepo { get; set; }
        public String controlAccesoOpc { get; set; }
    }
}
