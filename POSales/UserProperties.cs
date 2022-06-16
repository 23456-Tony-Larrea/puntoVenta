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
    public partial class UserProperties : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();

        UserAccount account;
        public string username;
        public UserProperties(UserAccount user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            account = user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

  
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if ((MessageBox.Show("Está seguro de que desea cambiar las propiedades de esta cuenta?", "Cambiar propiedades", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
                 {
                    cn.Open();
                    cm = new SqlCommand("UPDATE Usuarios SET nombre=@nombre, role=@role, isactive=@isactive WHERE username='" + username + "'",cn);
                    cm.Parameters.AddWithValue("@nombre", txtName.Text);
                    cm.Parameters.AddWithValue("@role", cbRole.Text);
                    cm.Parameters.AddWithValue("@isactive", cbActivate.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Las propiedades de la cuenta se han cambiado correctamente!", "Actualizar propiedades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    account.LoadUser();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void UserProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void UserProperties_Load(object sender, EventArgs e)
        {

        }
    }
}
