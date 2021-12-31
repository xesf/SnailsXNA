using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontEdit
{
    public partial class FontPropsForm : Form
    {
        private CFont _Font;
        public FontPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner, CFont font)
        {
            this._Font = font;
            return this.ShowDialog(owner);
        }

        /// <summary>
        /// 
        /// </summary>
        private void FontPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._TxtImageFile.Text = this._Font.ImageFilename;
                this._txtImageId.Text = this._Font.ImageId;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this._Font.ImageFilename = this._TxtImageFile.Text;
                this._Font.ImageId = this._txtImageId.Text;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}