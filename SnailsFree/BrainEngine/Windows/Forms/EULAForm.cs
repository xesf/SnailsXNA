using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoBrainsGames.BrainEngine.Windows.Forms
{
    public partial class EULAForm : Form
    {
        public EULAForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        private void EULAForm_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
                // Better close and cancel, we have to make sure the user reads the EULA
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
