using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using tp2_dominio;

namespace presentacion
{
    public partial class frmCatalogo : Form
    {
        private List<Articulo> articulos = new List<Articulo>();
        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void frmCatalogo_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Precio");
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                cargarImg(seleccionado.ImagenUrl);
            }
            else
            {

                pboxArticulo.Load("https://media.istockphoto.com/id/1222357475/vector/image-preview-icon-picture-placeholder-for-website-or-ui-ux-design-vector-illustration.jpg?s=612x612&w=0&k=20&c=KuCo-dRBYV7nz2gbk4J9w1WtTAgpTdznHu55W9FjimE=");
            }
            
            
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                articulos = negocio.listar_articulos();
                dgvCatalogo.DataSource = articulos;
                ocultarColumnas();
                pboxArticulo.Load(articulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvCatalogo.Columns["ImagenUrl"].Visible = false;
            dgvCatalogo.Columns["Id"].Visible = false;
        }

        private void cargarImg(string imagen)
        {
            try
            {
                pboxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pboxArticulo.Load("https://media.istockphoto.com/id/1222357475/vector/image-preview-icon-picture-placeholder-for-website-or-ui-ux-design-vector-illustration.jpg?s=612x612&w=0&k=20&c=KuCo-dRBYV7nz2gbk4J9w1WtTAgpTdznHu55W9FjimE=");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar frmAgregar = new frmAgregar();
            frmAgregar.ShowDialog();
            cargar();
        }

        private bool validarVacio()
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                return true;
            }
            MessageBox.Show("¡Debes elegir un artículo antes!", "ERROR");
            return false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (validarVacio())
            {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                frmAgregar frmModificar = new frmAgregar(seleccionado);
                frmModificar.ShowDialog();
                cargar();
            }
            else
            {
                return;
            }
        }

        private void btnEliminarFisico_Click_1(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                if (validarVacio())
                {
                    DialogResult respuesta = MessageBox.Show("¿CONFIRMAR ELIMINACIÓN?", "ELIMINAR", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                        cargar();
                        MessageBox.Show("ARTÍCULO ELIMINADO EXITOSAMENTE");
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtFiltroAvanzado_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;

            if (filtro != "")
            {
                listaFiltrada = articulos.FindAll(x => x.Nombre.ToLower().StartsWith(txtFiltroRapido.Text.ToLower()) || x.Categoria.categoria_descripcion.ToLower().StartsWith(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = articulos;
            }
            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a:");
                cboCriterio.Items.Add("Menor a:");
                cboCriterio.Items.Add("Igual a:");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:");
                cboCriterio.Items.Add("Termina con:");
                cboCriterio.Items.Add("Contiene:");
            }
        }

        private bool validarNumeros(string txtFiltro)
        {
            foreach (char character in txtFiltro)
            {
                if (char.IsLetter(character))
                {
                    return true;
                }
            }
            return false;
        }
        private bool validarCamposFiltro()
        {
            if (cboCampo.SelectedIndex < 0 || cboCriterio.SelectedIndex < 0 || string.IsNullOrEmpty(txtFiltro.Text))
            {
                MessageBox.Show("¡Faltan campos por completar!", "ERROR");

            } else if (validarNumeros(txtFiltro.Text)) 
            {
                MessageBox.Show("¡El precio debe buscarse en números!", "ERROR");
                return true;
            }
            return false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarCamposFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvCatalogo.DataSource = null;
                dgvCatalogo.DataSource = negocio.filtrar(campo, criterio, filtro);
                ocultarColumnas();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dgvCatalogo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (validarVacio())
            {
                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;
                frmDetalle frmDetalle = new frmDetalle(seleccionado);
                frmDetalle.ShowDialog();
                cargar();
            }
            else
            {
                return;
            }
        }
    }
}
