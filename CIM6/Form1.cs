using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CIM6
{


    public partial class Form1 : Form
    {
        public int labelid { get; private set; }
        public Dictionary<int, string> CigarettePosDict = new Dictionary<int, string>();
        public Dictionary<int, int> CigaretteNumDict = new Dictionary<int, int>();

        public Form1()
        {
            InitializeComponent();
            // 1. set the connection string
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Cigarettes.mdf;Integrated Security=True";

            SetLabelText(this.tableLayoutPanel1);
            SetTextBoxNum(this.tableLayoutPanel1);
        }

        private void GetCigaretteNum()
        {

            SqlDataReader rdr = null;
            int pos = 0;
            int num = 0;

            try
            {
                // 2. Open the connection
                conn.Open();

                // 3. Pass the connection to a command object
                SqlCommand cmd = new SqlCommand("select * from CigaretteInventory", conn);

                //
                // 4. Use the connection
                //

                // get query results
                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    // Console.WriteLine("Output is: {0} {1} {2} {3}", rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3));

                    pos = rdr.GetInt32(4);
                    num = rdr.GetInt32(3);

                    if (!CigaretteNumDict.ContainsKey(pos))
                    {
                        CigaretteNumDict.Add(pos, num);
                    }
                    else //update key value
                    {
                        CigaretteNumDict[pos] = num;
                    }
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }


        public void SetTextBoxNum(Control control)
        {

            int textboxid = 0;
            string namestr;

            this.GetCigaretteNum();


            foreach (var pair in CigaretteNumDict)
            {
                Console.WriteLine("Pos = {0}, Num = {1}", pair.Key, pair.Value);
            }

            foreach (Control child in control.Controls)
            {
                if (child is TextBox)
                {
                    TextBox tb = (TextBox)child;
                    if (tb.Name.StartsWith("textBox"))
                    {
                        namestr = tb.Name.Replace("textBox", "");


                        if (Int32.TryParse(namestr, out textboxid))
                        {
                            // Console.WriteLine("label id = {0}", labelid);
                            tb.Text = CigaretteNumDict[textboxid].ToString();

                        }


                    }
                }

            }

        }


        public void SetLabelText(Control control)
        {
            
            int labelid = 0;
            string namestr;

            this.GetCigaretteName();

   
            foreach (var pair in CigarettePosDict)
            {
                Console.WriteLine("Pos = {0}, CigaretteName = {1}", pair.Key, pair.Value);
            }

            foreach (Control child in control.Controls)
            {
                if (child is Label)
                {
                    Label lbl = (Label)child;
                    if (lbl.Name.StartsWith("label"))
                    {
                        namestr = lbl.Name.Replace("label", "");

                        
                        if (Int32.TryParse(namestr, out labelid))
                        {
                           // Console.WriteLine("label id = {0}", labelid);
                            lbl.Text = CigarettePosDict[labelid];
            
                        }

 
                    }
                }

            }

        }

        public SqlConnection conn = new SqlConnection();

        private void GetCigaretteName()
        {

            SqlDataReader rdr = null;
            int pos = 0;
            string cigarettename;

            try
            {
                // 2. Open the connection
                conn.Open();

                // 3. Pass the connection to a command object
                SqlCommand cmd = new SqlCommand("select * from CigaretteInventory", conn);

                //
                // 4. Use the connection
                //

                // get query results
                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    // Console.WriteLine("Output is: {0} {1} {2} {3}", rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3));
 
                    pos = rdr.GetInt32(4);
                    cigarettename = rdr.GetString(2);


                    if (!CigarettePosDict.ContainsKey(pos))
                    {
                        CigarettePosDict.Add(pos, cigarettename);
                    }
                    else //update key value
                    {
                        CigarettePosDict[pos] = cigarettename;
                    }
                }
            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }




        private void keyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
        }

        private const int buttonsperrow = 12;


        private void is_shortage(TextBox tb, int num)
        {
            if (num < 5)
            {
                tb.BackColor = Color.Red;
            }
            else
            {
                tb.BackColor = Color.Empty;
            }

        }

        private void button_Clicked(object sender, EventArgs e)
        {
            Button thebutton = (Button)sender;
            TextBox thebox;
            string buttonname;
            int button_id = 0;
            int row = 0;
            int column = 0;
            int cigarettes = 0;


            buttonname = thebutton.Name.Replace("button", "");

            if (Int32.TryParse(buttonname, out button_id))
            {
  
                    if (thebutton.Text == "ADD")
                    {
                        column = ((button_id % buttonsperrow) / 2) * 2;
                        row = (button_id / buttonsperrow) * 2 + 1;
                        Console.WriteLine("column = {0}", column);
                        thebox = (TextBox)this.tableLayoutPanel1.GetControlFromPosition(column, row);
                       

                        if (Int32.TryParse(thebox.Text, out cigarettes))
                        {
                            cigarettes = cigarettes + 1;
                            thebox.Text = cigarettes.ToString();
                            this.is_shortage(thebox, cigarettes);
                        }
                        else
                        {
                            MessageBox.Show("This should not happen.", "Alert");
                        }
                     }

                    else //SUB
                    {
                        column = (((button_id - 1) % buttonsperrow) / 2) * 2; 
                        row = ((button_id - 1) / buttonsperrow) * 2 + 1;
                        Console.WriteLine("column = {0}", column);
                        thebox = (TextBox)this.tableLayoutPanel1.GetControlFromPosition(column, row);

                        if (Int32.TryParse(thebox.Text, out cigarettes))
                        {
                            if (cigarettes > 0)
                            {
                                cigarettes = cigarettes - 1;
                                thebox.Text = cigarettes.ToString();
                                this.is_shortage(thebox, cigarettes);
                            }
                        }
                        else
                        {
                            MessageBox.Show("This should not happen.", "Alert");
                        }
                     }
                    
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void label_Clicked(object sender, EventArgs e)
        {
            this.Hide();
            Label clickedlabel = (Label)sender;
            Console.WriteLine("LableName = {0} Clicked.", clickedlabel.Name);
            String labelname = clickedlabel.Name.Replace("label", "");
            labelid = Int32.Parse(labelname);
            //every time a lable is clicked, a new form is created???
            EditCigaretteName newname = new EditCigaretteName(this);
            newname.ShowDialog();

            this.SetLabelText(this.tableLayoutPanel1);
            this.Show();
        }
    }
}
