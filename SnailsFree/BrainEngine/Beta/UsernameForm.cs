using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoBrainsGames.BrainEngine.Beta
{
	public partial class UsernameForm : Form
	{
		public string Username { get { return this._txtUsername.Text; } }
		public UsernameForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		private void UsernameForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.EnableButtons();
			}
			catch (System.Exception ex)
			{
				ClosedBeta.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void EnableButtons()
		{
			this._btnOk.Enabled = !string.IsNullOrEmpty(this._txtUsername.Text);
		}

		/// <summary>
		/// 
		/// </summary>
		private void _txtUsername_TextChanged(object sender, EventArgs e)
		{
			try
			{
				this.EnableButtons();
			}
			catch (System.Exception ex)
			{
				ClosedBeta.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void UsernameForm_Shown(object sender, EventArgs e)
		{
			try
			{
				this._txtUsername.Focus();
			}
			catch (System.Exception ex)
			{
				ClosedBeta.ShowException(this, ex);
			}
		}
	}
}
