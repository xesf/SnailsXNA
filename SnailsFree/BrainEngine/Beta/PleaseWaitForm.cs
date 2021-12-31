using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrainEngine.Web.Services;
using System.Threading;

namespace TwoBrainsGames.BrainEngine.Beta
{
    public partial class PleaseWaitForm : Form
    {
        private IWService _service;
        private Thread _thService;

        public PleaseWaitForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner, IWService service)
        {
            this._service = service;
            return this.ShowDialog(owner);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PleaseWaitForm_Shown(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex)
            {
                ClosedBeta.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PleaseWaitForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._thService = new Thread(new ParameterizedThreadStart(PleaseWaitForm.ServiceThread));
            }
            catch (System.Exception ex)
            {
                ClosedBeta.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ServiceThread(object par)
        {
            try
            {
                IWService service = (IWService)par;

                service.Execute();
            }
            catch (System.Exception )
            {
            }
        }
    }
}
