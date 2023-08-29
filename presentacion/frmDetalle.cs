using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tp2_dominio;
using static System.Net.Mime.MediaTypeNames;

namespace presentacion
{
    public partial class frmDetalle : Form
    {
        public frmDetalle()
        {
            InitializeComponent();
        }

        private Articulo Articulo;

        public frmDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.Articulo = articulo;
        }

        private void frmDetalle_Load(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.verDetalle(Articulo);

            lblCodigoRespuesta.Text = Articulo.Codigo;
            lblNombreRespuesta.Text = Articulo.Nombre;
            lblDescripcionRespuesta.Text = Articulo.Descripcion;
            lblCategoriaRespuesta.Text = Articulo.Categoria.categoria_descripcion;
            lblMarcaRespuesta.Text = Articulo.Marca.marca_descripcion;
            lblPrecioRespuesta.Text = Articulo.Precio.ToString();
            try
            {
                pboxArticulo.Load(Articulo.ImagenUrl);
            }
            catch (Exception)
            {
                pboxArticulo.Load("https://media.istockphoto.com/id/1222357475/vector/image-preview-icon-picture-placeholder-for-website-or-ui-ux-design-vector-illustration.jpg?s=612x612&w=0&k=20&c=KuCo-dRBYV7nz2gbk4J9w1WtTAgpTdznHu55W9FjimE=");
            }
        }
    }
}
