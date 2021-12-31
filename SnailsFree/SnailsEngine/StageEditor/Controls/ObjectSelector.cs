using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
  public partial class ObjectSelector : UserControl
  {
    public event EventHandler SelectedObjectChanged;

    public StageObject SelectedObject
    {
      get
      {
        if (this._ObjectList.SelectedItem == null)
          return null;

        if (this._ObjectList.SelectedItem is ImageListxItemObject == false)
          return null;

        return ((ImageListxItemObject)this._ObjectList.SelectedItem).Object;
      }

      set
      {
        if (this._ObjectList.SelectedItem != null)
        {
          this._ObjectList.SelectedItem.Selected = false;
        }
        this._ObjectList.SelectedItem = null;
      }
    }

    public ObjectSelector()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this._ObjectList.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitializeFromSettings()
    {
        this._ObjectList.Clear();
        Settings.LoadObjectToolboxItems(); // Theme might have changed, reload the items
        if (Settings.ObjectToolboxItems != null)
        {
            foreach (IObjectToolboxItem item in Settings.ObjectToolboxItems)
            {
                this._ObjectList.Add(new ImageListxItemObject(item));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSelectedObjectChanged()
    {
      if (this.SelectedObjectChanged != null)
      {
        this.SelectedObjectChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ObjectList_SelectedItemChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnSelectedObjectChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    private void group1_Resize(object sender, EventArgs e)
    {
        this.Height = this.group1.Height;
    }
  }
}
