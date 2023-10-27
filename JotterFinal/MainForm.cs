using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using Xceed.Words.NET;

namespace JotterFinal
{
    public partial class MainForm : Form
    {
        private String title, text, folder;
        private float size;
        Connect kon = new Connect();
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader rdr;
         
        public MainForm()
        {
            InitializeComponent();
            TitleLabel.BackColor = System.Drawing.Color.Transparent;
            SavedNotesLabel.BackColor = System.Drawing.Color.Transparent;

            //The next lines of code populates the Tree View using the database
            conn = kon.GetCon();
            conn.Open();
            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM JotterFile", conn);
            da.Fill(dt);

            RefreshTreeView();
        }

        private void DeleteButton_Click(object sender, EventArgs e)

        {
            // Display if nothing is selected
            if (SavedTreeView.SelectedNode == null)
            {
                MessageBox.Show("Please select a note to delete.", "Select Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TreeNode selected = SavedTreeView.SelectedNode;

            // Checking if the selected node is a folder (root node) or a note
            if (IsFolderNode(selected))
            {
                // Deleting a folder

                // Getting folder
                folder = selected.Text;

                // Confirm with the user
                DialogResult result = MessageBox.Show("Delete the folder '" + folder.Trim()+"' and all its contents?", "Delete Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Remove the folder and its child nodes from the Tree View
                    SavedTreeView.Nodes.Remove(selected);

                    // Deletes the folder by finding records with the same folder and removing all of them
                    conn = kon.GetCon();
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM JotterFile WHERE Folder = '" + folder + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                // Deleting a note

                // Getting folder name and title of the note
                folder = selected.Parent.Text.Trim();
                title = selected.Text.Trim();

                // Confirm with the user
                DialogResult result = MessageBox.Show("Delete the note '"+title+"' in folder '"+folder+"'?", "Delete Note", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    // Removing only the note from the Tree View
                    selected.Parent.Nodes.Remove(selected);

                    // Deleting the note form the database
                    conn = kon.GetCon();
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM JotterFile WHERE Folder = '" + folder + "' AND Title = '" + title + "'", conn);
                    cmd.ExecuteNonQuery();                  
                    conn.Close();
                }
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            // Ask the user for the name of the new Folder, error handling is done on the FolderNameForm
            var FolderNameForm = new FolderNameForm();
            FolderNameForm.Closed += (s, args) => this.Close();
            FolderNameForm.ShowDialog();

            folder = FolderNameForm.GetName();
            if (string.IsNullOrEmpty(folder)) // If folder name is still empty, then the user clicked cancel, so we return
            {
                return;
            }

            // Adds the folder into the database with placeholder values
            conn = kon.GetCon();
            conn.Open();

            string insertQuery = "INSERT INTO JotterFile (Folder, Title, Text, FontFamily, FontStyle, FontSize) VALUES (@folder, @title, @text, @fontFamily, @fontStyle, @fontSize)";
            cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@folder", folder);
            cmd.Parameters.AddWithValue("@title", ""); 
            cmd.Parameters.AddWithValue("@text", "");
            cmd.Parameters.AddWithValue("@fontFamily", "");
            cmd.Parameters.AddWithValue("@fontStyle", ""); 
            cmd.Parameters.AddWithValue("@fontSize", 0);

            cmd.ExecuteNonQuery();
            

            conn.Close();

            // Updates the tree view to display the updated database
            RefreshTreeView();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (SavedTreeView.SelectedNode == null || IsFolderNode(SavedTreeView.SelectedNode))
            {
                MessageBox.Show("Please select a note to open.", "Select Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            folder = SavedTreeView.SelectedNode.Parent.Text;
            title = SavedTreeView.SelectedNode.Text;

            // Get the selected note's title, folder, font, and size from the database
            conn = kon.GetCon();
            conn.Open();

            cmd = new SqlCommand("SELECT * FROM JotterFile WHERE Folder = '" + folder + "' AND Title = '" + title + "'", conn);
            rdr = cmd.ExecuteReader();

            rdr.Read();

            // Display the data saved in the database by updating Title and Main text box. Then set the font of the Main text box
            TitleBox.Text = rdr["Title"].ToString();
            MainTextBox.Text = rdr["Text"].ToString();
            FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), rdr["FontStyle"].ToString());
            MainTextBox.Font = new Font(rdr["FontFamily"].ToString().Trim(), float.Parse(rdr["FontSize"].ToString()), fontStyle);
          
            SavedTreeView.SelectedNode = null;
            rdr.Close();
            conn.Close();
            return;
        }


        private void EditButton_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();

            // Ask's the user to choose a font 
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                MainTextBox.Font = fontDialog.Font;
                
                if(MainTextBox.SelectionLength > 0)
                {
                    MainTextBox.SelectionFont = fontDialog.Font;
                }
            }
        }

        // This method is used to export the currently opened note. This method utilizes the DocX library to create a Word Document.
        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (MainTextBox.Text.Length == 0)
            {
                MessageBox.Show("There is no content to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Ask the user where they want to save it
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export as Word Document",
                Filter = "Word Document (*.docx)|*.docx|All Files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            // using DocX library, we create the new Word Document then populate it with the text in MainTextBox and save it.
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                using (DocX document = DocX.Create(fileName))
                {
                    document.InsertParagraph(MainTextBox.Text);
                    document.Save();
                }

                MessageBox.Show("File has been saved to " + fileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            title = TitleBox.Text;
            text = MainTextBox.Text;

            // Extract font-related data in MainTextBox
            string fontFamilyName = MainTextBox.Font.FontFamily.Name;
            float fontSize = MainTextBox.Font.Size;
            FontStyle fontStyle = MainTextBox.Font.Style;

            if (SavedTreeView.SelectedNode == null) // If user has not selected any nodes 
            {
                conn = kon.GetCon();
                conn.Open();

                // Check if the note already exists in the database
                cmd = new SqlCommand("SELECT COUNT(*) FROM JotterFile WHERE Folder = '" + folder + "' AND Title = '" + title + "'", conn);
                int noteCount = (int)cmd.ExecuteScalar();

                if (noteCount > 0)
                {
                    // Update the existing note
                    cmd = new SqlCommand("UPDATE JotterFile SET Text = @text, FontFamily = @fontFamily, FontSize = @fontSize, FontStyle = @fontStyle WHERE Folder = @folder AND Title = @title", conn);
                }
                else
                {
                    // Insert a new note
                    cmd = new SqlCommand("INSERT INTO JotterFile (Folder, Title, Text, FontFamily, FontSize, FontStyle) VALUES (@folder, @title, @text, @fontFamily, @fontSize, @fontStyle)", conn);
                }

                // Add parameters to the command
                cmd.Parameters.AddWithValue("@folder", folder);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@fontFamily", fontFamilyName);
                cmd.Parameters.AddWithValue("@fontSize", fontSize);
                cmd.Parameters.AddWithValue("@fontStyle", fontStyle.ToString());

                // Execute the command
                cmd.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show(title.Trim() + " saved.", "Note Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Check if a folder is selected and not a note
                if (!IsFolderNode(SavedTreeView.SelectedNode))
                {
                    MessageBox.Show("Please select a folder, not a note.", "Select Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Check if the folder has a placeholder
                conn = kon.GetCon();
                conn.Open();

                cmd = new SqlCommand("SELECT COUNT(*) FROM JotterFile WHERE Folder = '" + folder + "' AND Title = ''", conn);
                int placeholderCount = (int)cmd.ExecuteScalar();

                if (placeholderCount > 0)
                {
                    // Update the existing placeholder with font-related data
                    string updateQuery = "UPDATE JotterFile SET Title = @title, Text = @text, FontFamily = @fontFamily, FontStyle = @fontStyle, FontSize = @fontSize WHERE Folder = @folder AND Title = ''";

                    cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@folder", folder);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@text", text);
                    cmd.Parameters.AddWithValue("@fontFamily", fontFamilyName);
                    cmd.Parameters.AddWithValue("@fontStyle", fontSize);
                    cmd.Parameters.AddWithValue("@fontSize", fontStyle);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return;
                }


                folder = SavedTreeView.SelectedNode.Text; // Get the selected folder name

                // Check if the note already exists in the database
                cmd = new SqlCommand("SELECT COUNT(*) FROM JotterFile WHERE Folder = '" + folder + "' AND Title = '" + title + "'", conn);
                int noteCount = (int)cmd.ExecuteScalar();

                if (noteCount > 0)
                {
                    // Update the existing note
                    cmd = new SqlCommand("UPDATE JotterFile SET Text = @text, FontFamily = @fontFamily, FontSize = @fontSize, FontStyle = @fontStyle WHERE Folder = @folder AND Title = @title", conn);
                }
                else
                {
                    // Insert a new note
                    cmd = new SqlCommand("INSERT INTO JotterFile (Folder, Title, Text, FontFamily, FontSize, FontStyle) VALUES (@folder, @title, @text, @fontFamily, @fontSize, @fontStyle)", conn);
                }

                // Add parameters to the command
                cmd.Parameters.AddWithValue("@folder", folder);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@fontFamily", fontFamilyName);
                cmd.Parameters.AddWithValue("@fontSize", fontSize);
                cmd.Parameters.AddWithValue("@fontStyle", fontStyle.ToString());

                // Execute the command
                cmd.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Note saved in Folder: " + folder, "Note Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            RefreshTreeView(); // Refresh tree to show updated data
        }



        // This method is used whenever we save new data. It re-populates the tree with the updated database.
        private void RefreshTreeView() 
        {
            SavedTreeView.Nodes.Clear(); // Clear existing nodes

            conn = kon.GetCon();
            conn.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM JotterFile", conn);
            da.Fill(dt);

            // Goes through each record in the database and gets the Folder and Title
            foreach (DataRow dr in dt.Rows)
            {
                string folderName = dr["Folder"].ToString();
                string title = dr["Title"].ToString();

                // Check if the folder node already exists in the TreeView
                TreeNode folderNode = null;
                foreach (TreeNode node in SavedTreeView.Nodes)
                {
                    if (node.Text == folderName)
                    {
                        folderNode = node;
                        break;
                    }
                }

                if (folderNode == null)
                {
                    folderNode = new TreeNode(folderName);
                    SavedTreeView.Nodes.Add(folderNode);
                }

                TreeNode titleNode = new TreeNode(title);
                folderNode.Nodes.Add(titleNode);
            }

            conn.Close();
        }

        // This method is used in verifying if the selected node in the Tree View is a root, which means that it would be a folder in the database
        public bool IsFolderNode(TreeNode node)
        {
            //We check each node in the tree to identify if the node is already a folder
            foreach (TreeNode root in SavedTreeView.Nodes)
            {
                if (root.Text == node.Text)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

