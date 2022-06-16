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
    public partial class Supplier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Supplier()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadSupplier();
        }

        public void LoadSupplier()
        {
            dgvSupplier.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("SELECT * FROM Proveedores", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvSupplier.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(), dr[10].ToString(), dr[11].ToString(), dr[12].ToString(), dr[13].ToString(), dr[14].ToString(), dr[15].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SupplierModule supplierModule = new SupplierModule(this);
            supplierModule.ShowDialog();
        }

        private void dgvSupplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSupplier.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                SupplierModule supplierModule = new SupplierModule(this);
                supplierModule.lblId.Text = dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplierModule.txtSupplier.Text = dgvSupplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplierModule.txtAddress.Text = dgvSupplier.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplierModule.txtConPerson.Text = dgvSupplier.Rows[e.RowIndex].Cells[4].Value.ToString();
                supplierModule.txtPhone.Text = dgvSupplier.Rows[e.RowIndex].Cells[5].Value.ToString();
                supplierModule.txtEmail.Text = dgvSupplier.Rows[e.RowIndex].Cells[6].Value.ToString();
                supplierModule.txtFaxNo.Text = dgvSupplier.Rows[e.RowIndex].Cells[7].Value.ToString();
                supplierModule.txtCity.Text = dgvSupplier.Rows[e.RowIndex].Cells[12].Value.ToString();
                supplierModule.txtCountry.Text = dgvSupplier.Rows[e.RowIndex].Cells[13].Value.ToString();
                supplierModule.txtReasonS.Text = dgvSupplier.Rows[e.RowIndex].Cells[8].Value.ToString();
                supplierModule.txtCiRuc.Text = dgvSupplier.Rows[e.RowIndex].Cells[9].Value.ToString();
                supplierModule.txtDays.Text = dgvSupplier.Rows[e.RowIndex].Cells[10].Value.ToString();
                supplierModule.txtPageWeb.Text = dgvSupplier.Rows[e.RowIndex].Cells[16].Value.ToString();
                supplierModule.txtCPostal.Text = dgvSupplier.Rows[e.RowIndex].Cells[15].Value.ToString();
                supplierModule.txtProvince.Text = dgvSupplier.Rows[e.RowIndex].Cells[14].Value.ToString();
                supplierModule.cboState.Text = dgvSupplier.Rows[e.RowIndex].Cells[11].Value.ToString();
                supplierModule.btnSave.Enabled = false;
                supplierModule.btnUpdate.Enabled = true;
                supplierModule.ShowDialog();
            }
            else if(colName=="Delete")
            {
                if (MessageBox.Show("Eliminar este proovedor? click yes para confirmar", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Delete from Proveedores where id like '" + dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Eliminado con exito.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            LoadSupplier();
        }
    }
}
