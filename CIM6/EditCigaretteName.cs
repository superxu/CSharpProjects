using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CIM6
{
    public partial class EditCigaretteName : Form
    {
        public SqlConnection conn = new SqlConnection();


        Form1 _f1;
        Form2 _f2;

        public EditCigaretteName(Form f)
        {
            InitializeComponent();
            if (f.Name == "form1")
            {
                this._f1 = (Form1)f;
                this.CigaretteID.Text = this._f1.labelid.ToString();
            }
            else
            {
                this._f2 = (Form2)f;
                this.CigaretteID.Text = this._f2.labelid.ToString();
            }

            // 1. set the connection string
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Cigarettes.mdf;Integrated Security=True";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please input Cigarette Name!");
                return;
            }

            

            try
            {
                // 2. Open the connection
                conn.Open();

                // 3. Pass the connection to a command object
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string cmdstring = "UPDATE CigaretteInventory SET CigaretteName='" + textBox1.Text + "'  WHERE CigaretteID=" + this._f1.labelid + " ";
                MessageBox.Show(cmdstring);
                cmd.CommandText = cmdstring;

                //
                // 4. Use the connection
                //
                cmd.ExecuteNonQuery();

            }
            finally
            {

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
