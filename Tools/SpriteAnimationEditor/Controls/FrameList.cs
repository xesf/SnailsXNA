using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpriteAnimationEditor.Controls
{
  internal partial class FrameList : UserControl
  {
    
    #region Events
    public delegate void FrameDoubleClickedHandler(object sender, AnimationFrame frame);

    public event EventHandler SelectedAnimationFrameChanged;
    public event EventHandler AddFramesToAnimationClicked;
    public event EventHandler AddFramesFromFilesClicked;
    public event EventHandler DeleteAnimationFramesClicked;
    public event EventHandler InvertSelectedFramesClick;
    public event FrameDoubleClickedHandler FrameDoubleClicked;
    #endregion

    #region Variables
    Project _Project;
    Animation _Animation;
    bool _SelectedIndexEventSuspended;
    #endregion 
   
    #region Properties
    [BrowsableAttribute(false)]
    public Project Project
    {
      get { return this._Project; }
      set 
      { 
        this._Project = value;
        this.Animation = null;
        this.Refresh();
      }
    }

    [BrowsableAttribute(false)]
    public Animation Animation
    {
      get { return this._Animation; }
      set
      {
        this._Animation = value;
        this.Refresh();
      }
    }

    [BrowsableAttribute(false)]
    public bool HasSingleFrameAnimationSelected
    {
      get { return this._ListAnimation.SelectedItems.Count == 1; }
    }

    [BrowsableAttribute(false)]
    public bool HasAnimationFramesSelected
    {
      get { return this._ListAnimation.SelectedItems.Count > 0; }
    }

    [BrowsableAttribute(false)]
    public bool HasAnimationFrames
    {
      get { return this._ListAnimation.Items.Count > 0; }
    }

    
    [BrowsableAttribute(false)]
    public List<AnimationFrame> SelectedAnimationFrames
    {
      get
      {
        List<AnimationFrame> frameList = new List<AnimationFrame>();

        foreach (ListViewItem item in this._ListAnimation.SelectedItems)
        {
          frameList.Add((AnimationFrame)item.Tag);
        }

        return frameList;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    [BrowsableAttribute(false)]
    public bool IsFirstAnimFrameSelected
    {
      get
      {
        if (this._ListAnimation.SelectedIndices.Count == 0)
          return false;
        return (this._ListAnimation.SelectedIndices[0] == 0);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    [BrowsableAttribute(false)]
    public bool IsLastAnimFrameSelected
    {
      get
      {
        if (this._ListAnimation.SelectedIndices.Count == 0)
          return false;
        return (this._ListAnimation.SelectedIndices[0] == this._ListAnimation.Items.Count - 1);
      }
    }
    #endregion
   
    
    /// <summary>
    /// 
    /// </summary>
    public FrameList()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this._ListAnimation.Items.Clear();
    }

    #region Animation frames methods
    /// <summary>
    /// 
    /// </summary>
    public void AddAnimationFrame(AnimationFrame animFrame)
    {
      ListViewItem item = new ListViewItem();
      item.SubItems.Add("");
      item.SubItems.Add("");
      item.Tag = animFrame;
      this._ListAnimation.Items.Add(item);
      this.RefreshAnimationFrame(animFrame);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshAnimationFrames()
    {

      if (this.Animation != null)
      {
        foreach (AnimationFrame frame in this.Animation.Frames)
        {
          this.RefreshAnimationFrame(frame);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshAnimationFrame(AnimationFrame animationframe)
    {
      ListViewItem item = this.GetAnimationFrameItem(animationframe);
      if (item == null)
        return;

      item.Text = "";
      item.SubItems[1].Text = animationframe.Rectangle.ToString();
      item.Tag = animationframe;
      item.SubItems[2].Text = "";

      if (animationframe.ColisionZones != null)
      {
          string cols = "";
          foreach (ColisionAreaBase col in animationframe.ColisionZones)
          {
              cols += col.ToString() + "; ";
          }
          item.SubItems[2].Text = cols;
      }

      this.RenumberAnimationFrames();

    }

    /// <summary>
    /// 
    /// </summary>
    ListViewItem GetAnimationFrameItem(AnimationFrame animationframe)
    {
      if (this.Animation == null)
        return null;
      foreach (ListViewItem item in this._ListAnimation.Items)
      {
        if ((AnimationFrame)item.Tag == animationframe)
          return item;
      }
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RenumberAnimationFrames()
    {
      if (this.Animation == null)
        return;

      foreach (ListViewItem item in this._ListAnimation.Items)
      {
        item.Text = ((AnimationFrame)item.Tag).FrameNr.ToString();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveAnimationFrameUp(AnimationFrame animationframe)
    {
      if (animationframe.FrameNr - 1 < 0)
        return;

      ListViewItem item = this.GetAnimationFrameItem(animationframe);
      if (item == null)
        return;

      this._ListAnimation.Items.Remove(item);
      this._ListAnimation.Items.Insert(animationframe.FrameNr - 1, item);
      this.RefreshAnimationFrame(animationframe);
      this.RenumberAnimationFrames();
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveAnimationFrameDown(AnimationFrame animationframe)
    {
      if (animationframe.FrameNr - 1 < 0)
        return;

      ListViewItem item = this.GetAnimationFrameItem(animationframe);
      if (item == null)
        return;

      this._ListAnimation.Items.Remove(item);
      this._ListAnimation.Items.Insert(animationframe.FrameNr - 1, item);
      this.RefreshAnimationFrame(animationframe);
      this.RenumberAnimationFrames();
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveAnimationFrames(List<AnimationFrame> animFrames)
    {
      this.SuspendSelectedIndexChanged();
      foreach (AnimationFrame frame in animFrames)
      {
        ListViewItem item = this.GetAnimationFrameItem(frame);
        if (item != null)
          this._ListAnimation.Items.Remove(item);
      }
      this.RenumberAnimationFrames();
      this.ResumeSelectedIndexChanged();
      this._ListAnimation_SelectedIndexChanged(this._ListAnimation, new EventArgs());
    }
    #endregion

    #region Control events
    /// <summary>
    /// 
    /// </summary>
    private void _RbViewProjFrames_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _RbViewAnimFrames_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion

    #region Miscelaneous methods



    /// <summary>
    /// 
    /// </summary>
     public override void Refresh()
    {
     base.Refresh();
      this.Clear();
    

      if (this.Animation != null)
      {
        foreach (AnimationFrame frame in this.Animation.Frames)
        {
          this.AddAnimationFrame(frame);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClearSelection()
    {
    }

   
    #endregion

    #region Animation methods
    /// <summary>
    /// 
    /// </summary>
    public void SelectAllAnimationFrames()
    {
      foreach (ListViewItem item in this._ListAnimation.Items)
      {
        item.Selected = true;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void AnimationFocus()
    {
      this._ListAnimation.Focus();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ListAnimation_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (this._SelectedIndexEventSuspended == true)
          return;

        this.OnSelectedAnimationFrameChanged();
      } 
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ListAnimation_Click(object sender, EventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    
    /// <summary>
    /// 
    /// </summary>
    public void SelectFrame(AnimationFrame frame, bool addToSelection)
    {
      //this.SuspendSelectedIndexChanged();
      try
      {
        for (int i = 0; i < this._ListAnimation.Items.Count; i++)
        {
          if ((AnimationFrame)this._ListAnimation.Items[i].Tag == frame)
          {
            if (!addToSelection)
            {
              this._ListAnimation.SelectedItems.Clear();
            }
            this._ListAnimation.SelectedIndices.Add(i);
            return;
          }
        }
      }
      finally
      {
    //    this.ResumeSelectedIndexChanged();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SelectFrames(List<AnimationFrame> frames)
    {
      this.SuspendSelectedIndexChanged();
      try
      {
        bool bFound = false;
        for (int i = 0; i < this._ListAnimation.Items.Count; i++)
        {
          bFound = false;
          foreach (AnimationFrame frame in frames)
          {
            if ((AnimationFrame)this._ListAnimation.Items[i].Tag == frame)
            {
              this._ListAnimation.SelectedIndices.Add(i);
              bFound = true;
              break ;
            }
          }
          if (!bFound)
            this._ListAnimation.SelectedIndices.Remove(i);
        }
      }
      finally
      {
        this.ResumeSelectedIndexChanged();
        this.OnSelectedAnimationFrameChanged();
      }
    }

    #endregion

    #region Event launching 
    /// <summary>
    /// 
    /// </summary>
    private void OnSelectedAnimationFrameChanged()
    {
      if (this.SelectedAnimationFrameChanged != null)
      {
        this.SelectedAnimationFrameChanged(this, new EventArgs());
      }
    }

    
    /// <summary>
    /// 
    /// </summary>
    private void OnAddFramesToAnimationClicked()
    {
      if (this.AddFramesToAnimationClicked != null)
      {
        this.AddFramesToAnimationClicked(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnAddFramesFromFilesClicked()
    {
      if (this.AddFramesFromFilesClicked != null)
      {
        this.AddFramesFromFilesClicked(this, new EventArgs());
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void OnDeleteAnimationFramesClicked()
    {
      if (this.DeleteAnimationFramesClicked != null)
      {
        this.DeleteAnimationFramesClicked(this, new EventArgs());
      }
    }

     /// <summary>
    /// 
    /// </summary>
    private void OnInvertSelectedFramesClick()
    {
      if (this.InvertSelectedFramesClick != null)
      {
        this.InvertSelectedFramesClick(this, new EventArgs());
      }
    }

     /// <summary>
    /// 
    /// </summary>
    private void OnFrameDoubleClicked()
    {
      if (this.FrameDoubleClicked != null)
      {
        this.FrameDoubleClicked(this, (AnimationFrame) this._ListAnimation.SelectedItems[0].Tag);
      }
    }
    
    #endregion

    #region Project list context menu
    /// <summary>
    /// 
    /// </summary>
    private void _MenuProjFrames_Opening(object sender, CancelEventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
   
    #endregion

    #region Animation list context menu  
    /// <summary>
    /// 
    /// </summary>
    private void _MenuAnimFrames_Opening(object sender, CancelEventArgs e)
    {
      try
      {
        this._OptRemAnimFrames.Enabled = (this.HasAnimationFramesSelected);
        this._OptSelAllAnimFrames.Enabled = (this.HasAnimationFrames);

        e.Cancel = (!(this._OptRemAnimFrames.Enabled  && this._OptSelAllAnimFrames.Enabled ));
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptSelAllAnimFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this.SelectAllAnimationFrames();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptRemAnimFrames_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnDeleteAnimationFramesClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    public void SuspendSelectedIndexChanged()
    {
      this._SelectedIndexEventSuspended = true;
    }
    /// <summary>
    /// 
    /// </summary>
    public void ResumeSelectedIndexChanged()
    {
      this._SelectedIndexEventSuspended = false;
    }
    /// <summary>
    /// 
    /// </summary>
    private void _ListAnimation_DoubleClick(object sender, EventArgs e)
    {
      this.OnFrameDoubleClicked();
    }

  }
}
