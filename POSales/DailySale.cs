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
    public partial class DailySale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public string solduser;
        MainForm main;
        public DailySale(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadCashier();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCashier()
        {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("Selecciona cashier");
            cn.Open();
            cm = new SqlCommand("SELECT * FROM Usuarios WHERE role LIKE 'cashier'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public void LoadSold()
        {
            int i = 0;
            double total = 0;
            dgvSold.Rows.Clear();
            cn.Open();
            if(cboCashier.Text== "Selecciona cashier")
            {
                cm = new SqlCommand("select c.id, c.trasnno, c.pcode, p.pDesc, c.price, c.cantidad, c.disc, c.total from Carrito as c inner join Productos as p on c.pcode = p.codigo where status like 'Sold' and sdate between '" + dtFrom.Value.Date.ToString("yyyy/MM/dd") + "' and '" + dtTo.Value.ToString("yyyy/MM/dd") + "'", cn);
            }
            else
            {
                cm = new SqlCommand("select c.id, c.trasnno, c.pcode, p.pDesc, c.price, c.cantidad, c.disc, c.total from Carrito as c inner join Productos as p on c.pcode = p.codigo where status like 'Sold' and sdate between '" + dtFrom.Value.Date.ToString("yyyy/MM/dd") + "' and '" + dtTo.Value.ToString("yyyy/MM/dd") + "' and cashier like '" + cboCashier.Text + "'", cn);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                total += double.Parse(dr["total"].ToString());
                dgvSold.Rows.Add(i, dr["id"].ToString(), dr["trasnno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["cantidad"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
            lblTotal.Text = total.ToString("#,##0.00");
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void DailySale_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancelOrder = new CancelOrder(this);
                cancelOrder.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancelOrder.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancelOrder.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancelOrder.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancelOrder.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancelOrder.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancelOrder.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancelOrder.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                cancelOrder.txtCancelBy.Text = solduser;
                cancelOrder.ShowDialog();

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "Fltrar por fecha: De " + dtFrom.Value.ToShortDateString() + " A: " + dtTo.Value.ToShortDateString();
               
            if (cboCashier.Text == "Selecciona cashier")
            {
                report.LoadDailyReport("select c.id, c.trasnno, c.pcode, p.pDesc, c.price, c.cantidad, c.disc as discount, c.total from Carrito as c inner join Productos as p on c.pcode = p.codigo where status like 'Sold' and sdate between '" + dtFrom.Value.Date.ToString("yyyy/MM/dd") + "' and '" + dtTo.Value.Date.ToString("yyyy/MM/dd") + "'", param, cboCashier.Text);
            }
            else
            {
                report.LoadDailyReport("select c.id, c.trasnno, c.pcode, p.pDesc, c.price, c.cantidad, c.disc as discount, c.total from Carrito as c inner join Productos as p on c.pcode = p.codigo where status like 'Sold' and sdate between '" + dtFrom.Value.Date.ToString("yyyy/MM/dd") + "' and '" + dtTo.Value.Date.ToString("yyyy/MM/dd") + "' and cashier like '" + cboCashier.Text + "'", param, cboCashier.Text);
            }
            report.ShowDialog();
        }

        private void dgvSold_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancel = new CancelOrder(this);
                cancel.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancel.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancel.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancel.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancel.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancel.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancel.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancel.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                if(lblTitle.Visible==false)
                    cancel.txtCancelBy.Text = main.lblUsername.Text;
                else
                    cancel.txtCancelBy.Text = solduser;
                cancel.ShowDialog();
            }
        }
    }
}
