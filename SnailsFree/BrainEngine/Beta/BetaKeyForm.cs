using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Windows;

namespace TwoBrainsGames.BrainEngine.Beta
{
	public partial class BetaKeyForm : Form
	{
		public string BetaKey { get { return this._txtBetaKey.Text; } }
		public BetaKeyForm()
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
				BEMessageBox.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void EnableButtons()
		{
			this._btnOk.Enabled = !string.IsNullOrEmpty(this._txtBetaKey.Text);
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
                BEMessageBox.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void UsernameForm_Shown(object sender, EventArgs e)
		{
			try
			{
				this._txtBetaKey.Focus();
			}
			catch (System.Exception ex)
			{
                BEMessageBox.ShowException(this, ex);
			}
		}
	}
}
