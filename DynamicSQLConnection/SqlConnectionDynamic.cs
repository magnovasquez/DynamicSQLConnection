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

namespace DynamicSQLConnection
{
    public partial class SqlConnectionDynamic : Form
    {
        public SqlConnectionDynamic()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void LoadServers()
        {
            cboServers.Items.Clear();
            //Adding local server in case you are using SQL SERVER Express locally.
            cboServers.Items.Add("(localDB)\\MSSQLLocalDB");
            
            DataTable table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            
            foreach (DataRow server in table.Rows)
            {
                cboServers.Items.Add(server[table.Columns["ServerName"]].ToString());
            }
        }

        private void btnLoadServers_Click(object sender, EventArgs e)
        {
            LoadServers();
        }

        private void cboDB_DropDown(object sender, EventArgs e)
        {
            try
            {
                string server = cboServers.Text;

                string connectionString = @"Data Source=" + server + "; User = "+txtUsername.Text.Trim()+"; Password="+txtPassword.Text.Trim()+";";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            cboDB.Items.Clear();
                            while (dr.Read())
                            {
                                cboDB.Items.Add(dr[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
               
            }
           
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=" + cboServers.Text + "; Initial Catalog = " + cboDB.Text + ";User = " + txtUsername.Text.Trim() + "; Password=" + txtPassword.Text.Trim() + ";";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    MessageBox.Show("Connection succeful!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
               
            }
 
            
           
            
        }
    }
}
