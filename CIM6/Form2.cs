using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CIM6
{
    class Form2: Form1
    {
        public Form2()
        {
     
            this.Name = "Form2";
            this.Text = "Form2";

            // 
            // nextPageToolStripMenuItem
            // 

            this.nextPageToolStripMenuItem.Text = "Prev Page";
            //this.nextPageToolStripMenuItem.Click += new System.EventHandler(this.nextPageToolStripMenuItem_Click);


        }

        //cannot use method hiding here, why?
        protected override void nextPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Prev Page Button Clicked!");
            this.Hide();
            this.DialogResult = DialogResult.OK;

        }

        protected override void GetCigaretteNumFromDB()
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
                    if (rdr.GetInt32(0) <= CigarettesPerPage)
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

        protected override void GetTextBoxBum(Control control)
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
                            CigaretteNumDictSave[textid+CigarettesPerPage] = Int32.Parse(tb.Text);
                            // Console.WriteLine("Updated Cigarette Num = {0}", CigaretteNumDictSave[textid]);

                        }
                    }
                }

            }

        }


        protected override void SetTextBoxNum(Control control)
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
                            tb.Text = CigaretteNumDict[textboxid+CigarettesPerPage].ToString();
                            this.is_shortage(tb, CigaretteNumDict[textboxid+CigarettesPerPage]);

                        }


                    }
                }

            }

        }


        protected override void SetLabelText(Control control)
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
                            lbl.Text = CigarettePosDict[labelid+CigarettesPerPage];

                        }
                    }
                }

            }

        }

      
        protected override void GetCigaretteName()
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
                    if (rdr.GetInt32(0) <= CigarettesPerPage)
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




    }
}
