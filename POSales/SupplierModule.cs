using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class SupplierModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Punto de venta";
        Supplier supplier;
        public SupplierModule(Supplier sp)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            supplier = sp;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtAddress.Clear();
            txtConPerson.Clear();
            txtEmail.Clear();
            txtFaxNo.Clear();
            txtPhone.Clear();
            txtSupplier.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtSupplier.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Guardar este provedor? click yes para confirmar.", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Insert into Proveedores (proveedor, direccion, contactPerson, telefono, email, fax) values (@proveedor, @direccion, @contactPerson, @telefono, @email, @fax) ", cn);
                    cm.Parameters.AddWithValue("@proveedor", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@direccion", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactPerson", txtConPerson.Text);
                    cm.Parameters.AddWithValue("@telefono", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFaxNo.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Proovedor guardado con exito!", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    supplier.LoadSupplier();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Actualizar este Proveedor? click yes para confirmar.", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Update Proveedores set proveedor=@proveedor, direccion=@direccion, contactPerson=@contactPerson, telefono=@telefono, email=@email, fax=@fax where Id=@Id ", cn);
                    cm.Parameters.AddWithValue("@Id", lblId.Text);
                    cm.Parameters.AddWithValue("@proveedor", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@direccion", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactPerson", txtConPerson.Text);
                    cm.Parameters.AddWithValue("@telefono", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFaxNo.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Proveedor actualizado correctamente!", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void SupplierModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
