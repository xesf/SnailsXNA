using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageEditor.Controls;
using System.Xml;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
	public partial class BaseForm : Form
	{
		public BaseForm()
		{
			InitializeComponent();
		}

		#region State save
		/// <summary>
		/// 
		/// </summary>
        public virtual DataFileRecord CreateSaveSate()
		{
            DataFileRecord record = new DataFileRecord("FormStateSave");
            record.AddField("Name", this.Name);

            DataFileRecord locationRecord = new DataFileRecord("Location");
            locationRecord.AddField("Left", this.Left);
            locationRecord.AddField("Top", this.Top);
            record.AddRecord(locationRecord);

            DataFileRecord sizeRecord = new DataFileRecord("Size");
            sizeRecord.AddField("Width", this.Width);
            sizeRecord.AddField("Height", this.Height);
            record.AddRecord(sizeRecord);

            return record;
		}

		/// <summary>
		/// 
		/// </summary>
        public virtual void RestoreSaveSate(DataFileRecord record)
		{
            if (record == null)
			{
				return;
			}

			DataFileRecord locationRecord = record.SelectRecord("Location");
            if (locationRecord != null)
			{
                this.Left = locationRecord.GetFieldValue<int>("Left", this.Left);
                this.Top = locationRecord.GetFieldValue<int>("Top", this.Top);
				if (this.Left + this.Width < 0)
					this.Left = 0;
				if (this.Top + this.Height < 0)
					this.Top = 0;
			}

            DataFileRecord locationSize = record.SelectRecord("Size");
            if (locationSize != null)
			{
                this.Width = locationSize.GetFieldValue<int>("Width", this.Width);
                this.Height = locationSize.GetFieldValue<int>("Height", this.Height);
				if (this.Width < 200)
					this.Width = 200;
				if (this.Height < 200)
					this.Height = 200;
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		private void BaseForm_Load(object sender, EventArgs e)
		{
			try
			{
				this.InitControls();
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitControls()
		{
			foreach (Control ctl in this.Controls)
			{
				this.InitControl(ctl);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitControl(Control ctl)
		{
			if (ctl is BaseComboBox)
			{
				BaseComboBox cb = (BaseComboBox)ctl;
				cb.LoadValues();
			}

			foreach (Control ctl1 in ctl.Controls)
			{
				this.InitControl(ctl1);
			}
		}
	}
}
