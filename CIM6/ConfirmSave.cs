using System;
using System.Windows.Forms;


namespace CIM6
{
    public partial class ConfirmSave : Form
    {
        Form1 _f1 = null;
        Form2 _f2 = null;

        public ConfirmSave(Form f)
        {
            InitializeComponent();
            if (f.Name == "Form1")
            {
                this._f1 = (Form1)f;
            }
            else
            {
                this._f2 = (Form2)f;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this._f1 != null)
            {
                this._f1.savechages = false;
            }
            else
            {
                this._f2.savechages = false;
            }
          
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (this._f1 != null)
            {
                this._f1.savechages = true;
            }
            else
            {
                this._f2.savechages = true;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
