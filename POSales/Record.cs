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
    public partial class Record : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Record()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadCriticalItems();
            LoadInventoryList();
        }

        public void LoadTopSelling()
        {
            int i = 0;
            dgvTopSelling.Rows.Clear();
            cn.Open();

            //Sort By Total Amount
            if (cbTopSell.Text == "Ordenar por cantidad")
            {
                cm = new SqlCommand("SELECT TOP 10 pcode, pDesc, isnull(sum(cantidad),0) AS cantidad, ISNULL(SUM(total),0) AS total FROM vwTopSelling WHERE sdate BETWEEN '" + dtFromTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Sold' GROUP BY pcode, pDesc ORDER BY cantidad DESC", cn);
           }
            else if (cbTopSell.Text == "Ordenar por cantidad total")
            {
                cm = new SqlCommand("SELECT TOP 10 pcode, pDesc, isnull(sum(cantidad),0) AS cantidad, ISNULL(SUM(total),0) AS total FROM vwTopSelling WHERE sdate BETWEEN '" + dtFromTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Sold' GROUP BY pcode, pDesc ORDER BY total DESC", cn);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvTopSelling.Rows.Add(i, dr["pcode"].ToString(), dr["pDesc"].ToString(), dr["cantidad"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            cn.Close();
        }

        public void LoadSoldItems()
        {
            try
            {
                dgvSoldItems.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT c.pcode, p.pDesc, c.price, sum(c.cantidad) as cantidad, SUM(c.disc) AS disc, SUM(c.total) AS total FROM Carrito AS c INNER JOIN Productos AS p ON c.pcode=p.codigo WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dtFromSoldItems.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToSoldItems.Value.Date.ToString("yyyy/MM/dd") + "' GROUP BY c.pcode, p.pDesc, c.price",cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvSoldItems.Rows.Add(i, dr["pcode"].ToString(), dr["pDesc"].ToString(), double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["cantidad"].ToString(), dr["disc"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(total),0) FROM Carrito WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dtFromSoldItems.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToSoldItems.Value.Date.ToString("yyyy/MM/dd") + "'", cn);
                lblTotal.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void LoadCriticalItems()
        {
            try
            {
                dgvCriticalItems.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT * FROM vwCriticalItems",cn);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    dgvCriticalItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void LoadInventoryList()
        {
            try
            {
                dgvInventoryList.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT * FROM vwInventoryList", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvInventoryList.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public void LoadCancelItems()
        {
            int i = 0;
            dgvCancel.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM vwCancelItems WHERE sdate BETWEEN '" + dtFromCancel.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToCancel.Value.Date.ToString("yyyy/MM/dd") + "'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvCancel.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(),  DateTime.Parse(dr[6].ToString()).ToShortDateString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString(), dr[10].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadStockInHist()
        {
            int i = 0;
            dgvStockIn.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM vwEnStock WHERE cast(sdate AS date) BETWEEN '" + dtFromStockIn.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToStockIn.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnLoadTopSell_Click(object sender, EventArgs e)
        {
            if(cbTopSell.Text== "seleccionar tipo de clasificación")
            {
                MessageBox.Show("Seleccione el tipo de clasificación de la lista desplegable.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTopSell.Focus();
                return;
            }
            LoadTopSelling();
        }

        private void btnLoadSoldItems_Click(object sender, EventArgs e)
        {
            LoadSoldItems();
        }

        private void btnPrintSoldItems_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "From : " + dtFromSoldItems.Value.Date.ToString("yyyy/MM/dd") + " To : " + dtToSoldItems.Value.Date.ToString("yyyy/MM/dd");
            report.LoadSoldItems("SELECT c.pcode, p.pdesc, c.price, sum(c.cantidad) as cantidad, SUM(c.disc) AS disc, SUM(c.total) AS total FROM Carrito AS c INNER JOIN Productos AS p ON c.pcode=p.codigo WHERE status LIKE 'Sold' AND sdate BETWEEN '" + dtFromSoldItems.Value.ToString() + "' AND '" + dtToSoldItems.Value.ToString() + "' GROUP BY c.pcode, p.pDesc, c.price",param);
            report.ShowDialog();
        }

        private void btnLoadCancel_Click(object sender, EventArgs e)
        {
            LoadCancelItems();
        }

        private void btnLoadStockIn_Click(object sender, EventArgs e)
        {
            LoadStockInHist();
        }

        private void btnPrintTopSell_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "From : " + dtFromTopSell.Value.Date.ToString("yyyy/MM/dd") + " To : " + dtToTopSell.Value.Date.ToString("yyyy/MM/dd");
            if (cbTopSell.Text == "Ordenar por cantidad")
            {
                report.LoadTopSelling("SELECT TOP 10 pcode, pdesc, isnull(sum(cantidad),0) AS cantidad, ISNULL(SUM(total),0) AS total FROM vwTopSelling WHERE sdate BETWEEN '" + dtFromTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Sold' GROUP BY pcode, pdesc ORDER BY cantidad DESC", param, "ORDENAR POR CANTIDAD");
            }
            else if (cbTopSell.Text == "Ordenar por cantidad total")
            {
                report.LoadTopSelling("SELECT TOP 10 pcode, pdesc, isnull(sum(cantidad),0) AS cantidad, ISNULL(SUM(total),0) AS total FROM vwTopSelling WHERE sdate BETWEEN '" + dtFromTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToTopSell.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Sold' GROUP BY pcode, pdesc ORDER BY total DESC", param, "ORDENAR POR CANTIDAD TOTAL");
            }
            report.ShowDialog();
        }

        private void btnPrintInventoryList_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            report.LoadInventory("SELECT * FROM vwInventoryList");
            report.ShowDialog();
        }

        private void btnPrintCancel_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "From : " + dtFromCancel.Value.Date.ToString("yyyy/MM/dd") + " To : " + dtToCancel.Value.Date.ToString("yyyy/MM/dd");
            report.LoadCancelledOrder("SELECT * FROM vwCancelItems WHERE sdate BETWEEN '" + dtFromCancel.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToCancel.Value.Date.ToString("yyyy/MM/dd") + "'", param);
            report.ShowDialog();
        }

        private void btnPrintStockIn_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "From : " + dtFromStockIn.Value.Date.ToString("yyyy/MM/dd") + " To : " + dtToStockIn.Value.Date.ToString("yyyy/MM/dd");
            report.LoadStockInHist("SELECT * FROM vwEnStock WHERE cast(sdate AS date) BETWEEN '" + dtFromStockIn.Value.Date.ToString("yyyy/MM/dd") + "' AND '" + dtToStockIn.Value.Date.ToString("yyyy/MM/dd") + "' AND status LIKE 'Done'", param);
            report.ShowDialog();
        }
    }
}
