using System;
using System.Windows.Forms;


namespace CIM6
{
    public partial class ConfirmSave : Form
    {
        Form1 _f1 = null;
   

        public ConfirmSave(Form1 f)
        {

            InitializeComponent();
            this._f1 = f;
 

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this._f1.savechages = false;
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._f1.savechages = true;
            this.DialogResult = DialogResult.OK;
        }
    }
}
