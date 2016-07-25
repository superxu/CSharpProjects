using System;
using System.Windows.Forms;

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
            

     
        }

        protected override void nextPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Prev Page Button Clicked!");
            this.Hide();
            this.DialogResult = DialogResult.OK;

        }


    }
}
