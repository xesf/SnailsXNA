using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor.Controls
{
  public partial class PropertiesCtl : UserControl
  {
    #region events
    public delegate void StageHandler(EditorStage stage);
    public event StageHandler StagePropertyChanged;
    #endregion

    public object SelectedObject 
    {
      get { return this._ObjProps.SelectedObject; }
      set { this._ObjProps.SelectedObject = value; }
    }

    public PropertiesCtl()
    {
      InitializeComponent();
      this._ObjProps.PropertySort = PropertySort.CategorizedAlphabetical;
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ObjProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      try
      {
        if (this.SelectedObject is EditorStage)
        {
          this.OnStagePropertyChanged((EditorStage)this.SelectedObject);
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
        this.SelectedObject = this.SelectedObject;
        base.Refresh();

    }
    #region events
    /// <summary>
    /// 
    /// </summary>
    void OnStagePropertyChanged(EditorStage stage)
    {
      if (this.StagePropertyChanged != null)
      {
        this.StagePropertyChanged(stage);
      }
    }
    #endregion
  }
}
