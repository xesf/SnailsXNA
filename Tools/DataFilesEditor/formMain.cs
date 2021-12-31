using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using System.Xml;
using System.Xml.Linq;

namespace DataFilesEditor
{
    public partial class formMain : Form
    {
        const string FORM_TITLE = "Data Files Editor";
        const string FORM_TITLE_PATH = FORM_TITLE + " - [{0}]";

        string _filename;
        string _ext;
        string _redo;
        string _originalContent;
        bool _hasChanged;

        public formMain()
        {
            InitializeComponent();
        }

        private void EnabledEdition(bool enabled)
        {
            _redo = null;
            txtContent.Clear();

            undoToolStripMenuItem.Enabled = enabled;
            redoToolStripMenuItem.Enabled = enabled;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;

            txtContent.Visible = true;
            EnabledEdition(true);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;

            txtContent.Visible = false;
            EnabledEdition(false);
            this.Text = FORM_TITLE;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _redo = txtContent.Text;
            txtContent.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.Text = _redo;
        }

        private void txtContent_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_filename))
                _filename = "Untitled";
            this.Text = string.Format(FORM_TITLE_PATH, _filename);

            if (txtContent.Text == null || 
                txtContent.Text == string.Empty ||
                _originalContent == txtContent.Text ||
                _redo == txtContent.Text)
            {
                _hasChanged = false;
                return;
            }

            _redo = txtContent.Text;
            _hasChanged = true;
            
            this.Text += "*"; 
        }

        private string FormatXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            return doc.ToString();
        }

        private void LoadFile(string filename)
        {
            EnabledEdition(true);

            _filename = filename;
            _ext = Path.GetExtension(_filename);

            IDataFileReader reader = null;
            switch (_ext)
            {
                case ".xdf": reader = new XmlDataFileReader(); break;
                case ".bdf": reader = new BinaryDataFileReader(); break;
            }

            if (File.Exists(_filename))
            {
                DataFile dataFile = reader.Read(_filename);

                XmlDataFileWriter wr = new XmlDataFileWriter();
                XmlDocument xmlDoc = wr.ToXmlDocument(dataFile);

                txtContent.Visible = true;
                txtContent.Text = FormatXml(xmlDoc.InnerXml);
                _hasChanged = false;
                _originalContent = txtContent.Text;

                this.Text = string.Format(FORM_TITLE_PATH, _filename);
            }
        }

        private bool IsContentValid()
        {
            // check if content is valid
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(txtContent.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data file is not in a valid xml format.\n" + ex.ToString(), "Invalid Data File!!");
                return false;
            }

            return true;
        }

        private void loadDataFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;

            if (!IsContentValid())
                return;

            if (openDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadFile(openDiag.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;
                
            Application.Exit();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.WordWrap = !txtContent.WordWrap;
            wordWrapToolStripMenuItem.Checked = txtContent.WordWrap;
        }

        private void formMain_DragDrop(object sender, DragEventArgs e)
        {
            if (!IfNotCancelled())
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1) // only accept to drag 1 file
            {
                LoadFile(files[0]);
            }
            else
            {
                MessageBox.Show("You can only drag one data file at a time.");
            }
        }

        private void formMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void txtContent_DragEnter(object sender, DragEventArgs e)
        {
            formMain_DragEnter(sender, e);
        }

        private void txtContent_DragDrop(object sender, DragEventArgs e)
        {
            formMain_DragDrop(sender, e);
        }

        private void SaveFile(string filename, string ext)
        {
            IDataFileWriter writer = null;
            switch (ext)
            {
                case ".xdf": writer = new XmlDataFileWriter(); break;
                case ".bdf": writer = new BinaryDataFileWriter(); break;
            }

            MemoryStream mStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(txtContent.Text));
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile df = reader.Read(mStream);

            writer.Write(filename, df);

            this.Text = string.Format(FORM_TITLE_PATH, _filename);

            _hasChanged = false;
            _originalContent = txtContent.Text;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;

            if (!IsContentValid())
                return;
            
            SaveFile(_filename, _ext);
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IfNotCancelled())
                return;

            if (!IsContentValid())
                return;

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                _filename = saveDialog.FileName;
                _ext = Path.GetExtension(_filename);
                SaveFile(_filename, _ext);
            }
        }

        private bool IfNotCancelled()
        {
            bool canRun = true;
            DialogResult result = System.Windows.Forms.DialogResult.Ignore;
            
            if (_hasChanged)
            {
                _hasChanged = false;
                result = MessageBox.Show("Do you want to save the changed data file?", "Save", MessageBoxButtons.YesNoCancel);
                switch (result)
                { 
                    case System.Windows.Forms.DialogResult.Yes:
                        saveasToolStripMenuItem_Click(this, null);
                        canRun = true;
                        break;
                    case System.Windows.Forms.DialogResult.No:
                        canRun = true;
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        canRun = false;
                        _hasChanged = true;
                        break;
                }
            }

            return canRun;
        }
    }
}
