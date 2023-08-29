using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp2_dominio
{
    public class Categoria
    {
        public int categoria_id { get; set; }

        public string categoria_descripcion { get; set; }

        public override string ToString()
        {
            return categoria_descripcion;
        }

    }

}
