using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

using System.Data;
using iTextSharp.text.html.simpleparser;

namespace CIM6
{


    public partial class Form2 : Form
    {
        public int labelid { get; private set; }
        public bool savechages { get; set; }

        public const int CigarettePerPage = 24;

        public Dictionary<int, string> CigarettePosDict = new Dictionary<int, string>();
        public Dictionary<int, int> CigaretteNumDict = new Dictionary<int, int>();
        public Dictionary<int, int> CigaretteNumDictSave = new Dictionary<int, int>();
        public Dictionary<string, int> CigaretteNameNum = new Dictionary<string, int>();

        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            // 1. set the connection string
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Cigarettes.mdf;Integrated Security=True";

            SetLabelText(this.tableLayoutPanel1);
            SetTextBoxNum(this.tableLayoutPanel1);
        }



        private void GetCigaretteNumFromDB()
        {

            SqlDataReader rdr = null;
            int pos = 0;
            int num = 0;
            string name;

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
                    // only get ID > CigarettePerPage
                    if (rdr.GetInt32(0) <= CigarettePerPage)
                    {
                        continue;
                    }

                    pos = rdr.GetInt32(4);
                    num = rdr.GetInt32(3);
                    name = rdr.GetString(2);

                    if (!CigaretteNumDict.ContainsKey(pos))
                    {
                        CigaretteNumDict.Add(pos, num);
                    }
                    else //update key value
                    {
                        CigaretteNumDict[pos] = num;
                    }

                    if (!CigaretteNameNum.ContainsKey(name))
                    {
                        CigaretteNameNum.Add(name, num);
                    }
                    else
                    {
                        CigaretteNameNum[name] = num;
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

        private void GetTextBoxBum(Control control)
        {
            string namestr;
            int textid;

            foreach (Control child in control.Controls)
            {
                if (child is TextBox)
                {
                    TextBox tb = (TextBox)child;
                    if (tb.Name.StartsWith("textBox"))
                    {
                        namestr = tb.Name.Replace("textBox", "");

                        if (Int32.TryParse(namestr, out textid))
                        {
                            // Console.WriteLine("label id = {0}", labelid);
                            CigaretteNumDictSave[textid+CigarettePerPage] = Int32.Parse(tb.Text);
                           // Console.WriteLine("Updated Cigarette Num = {0}", CigaretteNumDictSave[textid]);

                        }
                    }
                }

            }

        }


        private void SetTextBoxNum(Control control)
        {

            int textboxid = 0;
            string namestr;

            this.GetCigaretteNumFromDB();


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
                            tb.Text = CigaretteNumDict[textboxid+CigarettePerPage].ToString();
                            this.is_shortage(tb, CigaretteNumDict[textboxid+CigarettePerPage]);

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
                            lbl.Text = CigarettePosDict[labelid+CigarettePerPage];
            
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
                    // only get ID > CigarettePerPage
                    if (rdr.GetInt32(0) <= CigarettePerPage)
                    {
                        continue;
                    }

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

        private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!this.savechages)
            {
                savechagestodb();
            }

        }


        private void savechagestodb()
        {
            this.GetTextBoxBum(this.tableLayoutPanel1);

            try
            {
                // 2. Open the connection
                conn.Open();

                foreach (var pair in CigaretteNumDictSave)
                {
                  //  Console.WriteLine("Pos = {0}, Num = {1}", pair.Key, pair.Value);

                    // 3. Pass the connection to a command object
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    string cmdstring = "UPDATE CigaretteInventory SET CigaretteNum='" + pair.Value + "'  WHERE CigarettePos=" + pair.Key + " ";
                    //MessageBox.Show(cmdstring);
                    cmd.CommandText = cmdstring;

                    //
                    // 4. Use the connection
                    //
                    cmd.ExecuteNonQuery();
                }

            }
            finally
            {

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
    
            ConfirmSave save = new ConfirmSave(this);
            save.ShowDialog();

            this.Show();
            Console.WriteLine("Save changes: {0}", this.savechages);
            if (this.savechages)
            {
                savechagestodb();
            }
           
        }

 

        private void prevPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.OK;

        }



    }
}
