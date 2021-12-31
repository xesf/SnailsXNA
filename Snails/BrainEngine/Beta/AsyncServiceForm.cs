using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Web.Services;
using System.Threading;
using TwoBrainsGames.BrainEngine.Windows;

namespace TwoBrainsGames.BrainEngine.Beta
{
    public partial class AsyncServiceForm : Form
    {
		class ServiceThreadParams
		{
			public ServiceParams SrvParams { get; private set;}
			public AsyncServiceForm AsyncForm { get; private set; }

			public ServiceThreadParams(ServiceParams srvParams, AsyncServiceForm form)
			{
				this.SrvParams = srvParams;
				this.AsyncForm = form;
			}
		}

		class ServiceParams
		{
			public ServiceExecuteCallback ExecuteCallback { get; private set;}
			public IRequest Request { get; private set; }
			public IResponse Response { get; set; }
			public Exception SrvException { get; set; }

			public ServiceParams(ServiceExecuteCallback executeCallback, IRequest request)
			{
				this.ExecuteCallback = executeCallback;
				this.Request = request;
				this.Response = null;
				this.SrvException = null;
			}
		}

		public delegate IResponse ServiceExecuteCallback(IRequest request);
		public delegate void ServiceExecutionEndedHandler(IResponse response, System.Exception ex);

		private Thread _thService;
		ServiceParams ServiceParameters { get; set;}

        public AsyncServiceForm()
        {
            InitializeComponent();
        }

		/// <summary>
		/// 
		/// </summary>
		public static IResponse Execute(IRequest request, ServiceExecuteCallback executeCallback)
		{
			AsyncServiceForm form = new AsyncServiceForm();
			form.ServiceParameters = new ServiceParams(executeCallback, request);
			form.ShowDialog(null);
			if (form.ServiceParameters.SrvException != null)
			{
				throw form.ServiceParameters.SrvException;
			}
			return form.ServiceParameters.Response;
		}

        /// <summary>
        /// 
        /// </summary>
        private void PleaseWaitForm_Shown(object sender, EventArgs e)
        {
            try
            {
				this._timer.Enabled = true;
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PleaseWaitForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._thService = new Thread(new ParameterizedThreadStart(AsyncServiceForm.ServiceThread));
				this._thService.Start(new ServiceThreadParams(this.ServiceParameters, this));

				this._lblProgress.Text = "";
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		private void OnServiceEnded(IResponse response, System.Exception ex)
		{
			this.Close();
			this.ServiceParameters.Response = response;
			this.ServiceParameters.SrvException = ex;
		}

        /// <summary>
        /// 
        /// </summary>
        private static void ServiceThread(object par)
        {

			System.Exception srvException = null;
			ServiceThreadParams threadParams = (ServiceThreadParams)par;
			ServiceParams serviceParameters = threadParams.SrvParams;
			try
			{
                
                serviceParameters = threadParams.SrvParams;
				serviceParameters.Response = serviceParameters.ExecuteCallback(serviceParameters.Request);
			}
			catch (System.Exception ex)
			{
				srvException = ex;
			}
			finally
			{
				threadParams.AsyncForm.Invoke(new ServiceExecutionEndedHandler(threadParams.AsyncForm.OnServiceEnded), new object[] { serviceParameters.Response, srvException });
            }
        }

		/// <summary>
		/// 
		/// </summary>
		private void _timer_Tick(object sender, EventArgs e)
		{
			try
			{
				this._lblProgress.Text += ".";
				if (this._lblProgress.Text.Length > 10)
				{
					this._lblProgress.Text = ".";
				}
			}
			catch (System.Exception )
			{
				this._timer.Enabled = false;
			}
		}
    }
}
