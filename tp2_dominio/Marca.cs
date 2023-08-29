using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp2_dominio
{
    public class Marca
    {
        public int marca_id { get; set; }

        public string marca_descripcion { get; set; }

        public override string ToString()
        {
            return marca_descripcion;
        }

    }
}
