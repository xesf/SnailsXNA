using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontEdit
{
    public partial class PreferencesForm : Form
    {
        public PreferencesForm()
        {
            InitializeComponent();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._TxtDefCharWidth.Text = CSettings.DefaultCharWidth.ToString();
                this._TxtDefCharHeight.Text = CSettings.DefaultCharHeight.ToString();
                this._RdBlackBox.Checked = CSettings.UseBlackBox;
                this._RdWhiteBox.Checked = !CSettings.UseBlackBox;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void _BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                CSettings.DefaultCharWidth = Convert.ToInt32(this._TxtDefCharWidth.Text);
                CSettings.DefaultCharHeight = Convert.ToInt32(this._TxtDefCharHeight.Text);

                CSettings.UseBlackBox = this._RdBlackBox.Checked;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }
    }
}