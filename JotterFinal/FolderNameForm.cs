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

namespace JotterFinal
{
    public partial class FolderNameForm : Form
    {
        public String folderName = "";
        internal Connect kon = new Connect();
        public SqlConnection conn;
        public SqlCommand cmd;
        public SqlDataReader rdr;
        public FolderNameForm()
        {
            InitializeComponent();
            newFolderLabel.BackColor = System.Drawing.Color.Transparent;
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            folderName = FolderNameBox.Text.Trim(); 

            if (string.IsNullOrWhiteSpace(folderName))
            {
                MessageBox.Show("Please input a folder name.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Checks if the name is a duplicate by selecting any attribute with the same value
            conn = kon.GetCon();
            conn.Open();

            cmd = new SqlCommand("SELECT Folder FROM JotterFile WHERE Folder = '"+ folderName+"'", conn);
            
            rdr = cmd.ExecuteReader();
            
            // If the reader reads anything, this means that the folder already exists in the database and will ask for another input
            if (rdr.Read())
            {
                MessageBox.Show("Folder name already exists in the database. Please choose a different name.", "Duplicate Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                conn.Close();
                return;
            }

            conn.Close();
            this.Hide();
            FolderNameBox.Text = "";
            
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            FolderNameBox.Text = "";
            this.Hide();
        }

        public String GetName() { return folderName; }
    }
}
