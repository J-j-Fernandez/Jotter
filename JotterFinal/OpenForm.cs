using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JotterFinal
{
    public partial class OpenForm : Form
    {
        public OpenForm()
        {
            InitializeComponent();
            TitleLabel1.BackColor = System.Drawing.Color.Transparent;
        }

        private void OpenForm_Load(object sender, EventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            // Opens main form
            this.Hide();
            var MainForm = new MainForm();
            MainForm.Closed += (s, args) => this.Close();
            MainForm.Show();
        }
    }

}
