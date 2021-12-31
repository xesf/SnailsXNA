using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  internal partial class ResolutionMappersForm : Form
  {
    Project Project
    {
      get;
      set;
    }

    ResolutionMapper SelectedMapper
    {
      get
      {
        if (this._lstMappers.SelectedItem == null)
          return null;

        return (ResolutionMapper)(this._lstMappers.SelectedItem);
      }

    }
    /// <summary>
    /// 
    /// </summary>
    public ResolutionMappersForm()
    {
      InitializeComponent();
    }
    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(Form parent, Project project)
    {
      this.Project = project;
      return this.ShowDialog(parent);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      if (this.Project == null)
        return;

      this._lblProject.Text = this.Project.Name;
      this._lstMappers.Items.Clear();
      foreach (ResolutionMapper mapper in this.Project.ResolutionMappers)
      {
        this._lstMappers.Items.Add(mapper);
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void _btnAdd_Click(object sender, EventArgs e)
    {
      try
      {
        ResolutionMappingForm form = new ResolutionMappingForm();
        if (form.Add(this, this.Project) == DialogResult.Cancel)
        {
          return;
        }

        this.Project.AddMapper(form.Mapper);
        this._lstMappers.Items.Add(form.Mapper);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableControls();
      } 
    }

    /// <summary>
    /// 
    /// </summary>
    private void _btnEdit_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.SelectedMapper == null)
          return;

        ResolutionMappingForm form = new ResolutionMappingForm();
        if (form.Edit(this, this.SelectedMapper) == DialogResult.Cancel)
        {
          return;
        }

        if (this._lstMappers.SelectedItem != null)
        {
          this.Project.SetMapper(this._lstMappers.SelectedIndex, form.Mapper);
          this._lstMappers.Items[this._lstMappers.SelectedIndex] = form.Mapper;
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableControls();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _btnDelete_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.SelectedMapper == null)
          return;

        if (MessageBox.Show(this, "Are you sure you want to remove mapper '" + this.SelectedMapper.Name + "'?", 
                Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Cancel)
          return;
        this.Project.RemoveMapper(this.SelectedMapper);
        this._lstMappers.Items.Remove(this.SelectedMapper);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableControls();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableControls()
    {
      try
      {
        this._btnDelete.Enabled = (this.SelectedMapper != null);
        this._btnEdit.Enabled = (this.SelectedMapper != null);
      }
      catch (System.Exception) { }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResolutionMappersForm_Load(object sender, EventArgs e)
    {
      try
      {
        this.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
      finally
      {
        this.EnableControls();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void _lstMappers_SelectedIndexChanged(object sender, EventArgs e)
    {
      
        this.EnableControls();
     
   
    }
  }
}
