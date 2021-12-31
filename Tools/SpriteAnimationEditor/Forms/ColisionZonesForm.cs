using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{ 
  internal partial class ColisionZonesForm : Form
  {
	  public enum EditType
	  {
		  Animation,
		  Frame
	  }
    #region Variables
    Animation _Animation;
	AnimationFrame _AnimationFrame;
	EditType _EditType;
    bool _changed;
    bool _accepted;
    #endregion

    #region Properties
    public List<ColisionAreaBase> ColisionZones
    {
      get 
      {
        List<ColisionAreaBase> zones = new List<ColisionAreaBase>();
        foreach (ColisionAreaBase area in this._LstZones.Items)
        {
          zones.Add(area);
        }
        return zones; 
      }
    }

    Animation Animation
    {
      get { return this._Animation; }
      set
      { 
        this._Animation = value;
        this._Sprite.Animation = value;
      }
    }
    
    BoundingBox BoundingBox
    {
      get
      {
        BoundingBox bb = new BoundingBox();
        int x, y, width, height;

        int.TryParse(this._txtX.Text, out x);
        int.TryParse(this._txtY.Text, out y);
        int.TryParse(this._txtWidth.Text, out width);
        int.TryParse(this._txtHeight.Text, out height);

        bb.Rectangle = new Rectangle(x, y, width, height);
        bb.Description = this._txtDescription.Text;
        return bb;
      }
      set
      {
        this._txtX.Text = value.Rectangle.Left.ToString();
        this._txtY.Text = value.Rectangle.Top.ToString();
        this._txtWidth.Text = value.Rectangle.Width.ToString();
        this._txtHeight.Text = value.Rectangle.Height.ToString();
        this._txtDescription.Text = value.Description;
      }
    }

    BoundingSphere BoundingSphere
    {
      get
      {
        int x, y, radius;

        int.TryParse(this._txtBSX.Text, out x);
        int.TryParse(this._txtBSY.Text, out y);
        int.TryParse(this._txtBSRadius.Text, out radius);

        BoundingSphere bs = new BoundingSphere(x, y, radius);
        bs.Description = this._txtBSDescription.Text;
        return bs;
      }

      set
      {
        this._txtBSX.Text = value.X.ToString();
        this._txtBSY.Text = value.Y.ToString();
        this._txtBSRadius.Text = value.Radius.ToString();
        this._txtBSDescription.Text = value.Description;
      }
    }

    ColisionAreaBase ActiveColisionArea
    {
      get 
      {
        if (this._rbBB.Checked)
        {
          return (this.BoundingBox);
        }

        if (this._rbBs.Checked)
        {
          return (this.BoundingSphere);
        }

        return null;
      }

      set
      {
        if (value as BoundingBox != null)
        {
          this.BoundingBox = (BoundingBox)value;
          this._rbBB.Checked = true;
        }
        else
        if (value as BoundingSphere != null)
        {
          this.BoundingSphere = (BoundingSphere)value;
          this._rbBs.Checked = true;
        }
      }
    }
    #endregion

    #region Class constructors and overrides
    /// <summary>
    /// 
    /// </summary>
    public ColisionZonesForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(IWin32Window owner, Animation animation)
    {
      if (animation == null)
        throw new ApplicationException("No current animation! Colisions zones cannot be edited.");
      this.Animation = animation;
      this._LstZones.Items.Clear();
      foreach (ColisionAreaBase colision in animation.ColisionZones)
      {
        this._LstZones.Items.Add(colision);
      }

	  this._EditType = EditType.Animation;
	  this.Text = "Animation Collision Zones";
	  this._Sprite.AllowFrameClick = true;
	  return this.ShowDialog(owner);
    }

	/// <summary>
	/// 
	/// </summary>
	public DialogResult ShowDialog(IWin32Window owner, AnimationFrame animationFrame)
	{
		if (animationFrame == null)
			throw new ApplicationException("No current animation! Colisions zones cannot be edited.");
		this.Animation = animationFrame.ParentAnimation;
		this._LstZones.Items.Clear();
		foreach (ColisionAreaBase colision in animationFrame.ColisionZones)
		{
			this._LstZones.Items.Add(colision);
		}

		this._EditType = EditType.Frame;
		this._AnimationFrame = animationFrame;
		this._Sprite.AllowFrameClick = false;
		this.Text = "Frame Collision Zones";
		return this.ShowDialog(owner);
	}
    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      this._Sprite.Project = this.Animation.ParentProject;
      this.OnFormResize();
    }
    #endregion

    #region Control events
    /// <summary>
    /// 
    /// </summary>
    private void ColisionZonesForm_Load(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.GridVisible = false;
        this._Sprite.ShowImages = Settings.Instance.ShowImages;
        this._rbBB.Checked = true;
        this.Refresh();
        this.EnableControls();
        this.ActiveControl = this._txtX;
        if (this._LstZones.Items.Count > 0)
          this._LstZones.SelectedIndex = 0;

		this._Sprite.SelectedAnimationFrame = this._AnimationFrame;
        this._accepted = false;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void ColisionZonesForm_Resize(object sender, EventArgs e)
    {
      try
      {
        this.OnFormResize();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_FrameClicked(object sender, AnimationFrame frame)
    {
      try
      {

      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _rbBB_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _rbBs_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.EnableControls();

      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtX_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtY_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtWidth_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtHeight_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtBSX_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtBSY_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtBSRadius_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Refresh();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_SpritePaint(object sender, Graphics graphics)
    {
      try
      {
        if (this.ActiveColisionArea.IsZero())
          return;

        if (this._Sprite.SelectedAnimationFrames.Count == 0)
          return;

        foreach (ColisionAreaBase area in this.ColisionZones)
        {
          area.Draw(new Point(this._Sprite.SelectedAnimationFrames[0].Rectangle.Left,
                                                 this._Sprite.SelectedAnimationFrames[0].Rectangle.Top), graphics,
                                                 Color.DarkRed);
        }

        this.ActiveColisionArea.Draw(new Point(this._Sprite.SelectedAnimationFrames[0].Rectangle.Left,
                                               this._Sprite.SelectedAnimationFrames[0].Rectangle.Top), graphics,
                                               Color.Red);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnAdd_Click(object sender, EventArgs e)
    {
      try
      {
        ColisionAreaBase area = this.ActiveColisionArea;
        this.ColisionZones.Add(area);
        this._LstZones.Items.Add(area);
        this._LstZones.SelectedItem = area;
        this.EnableControls();
        this._changed = true;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
 
    /// <summary>
    /// 
    /// </summary>
    private void _BtnUpdate_Click(object sender, EventArgs e)
    {
      try
      {
        if (this._LstZones.SelectedIndex == -1)
          return;

        this._LstZones.Items[this._LstZones.SelectedIndex] = this.ActiveColisionArea;
        this._LstZones.Refresh();
        this.EnableControls();
        this._changed = true;

      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _LstZones_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (this._LstZones.SelectedItem == null)
          return;

        this.ActiveColisionArea = (ColisionAreaBase)this._LstZones.SelectedItem;
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnDelete_Click(object sender, EventArgs e)
    {
      try
      {
        if (this._LstZones.SelectedItem == null)
          return;
        this._LstZones.Items.Remove(this._LstZones.SelectedItem);
        this._txtBSX.Text = "";
        this._txtBSY.Text = "";
        this._txtBSRadius.Text = "";

        this._txtX.Text = "";
        this._txtY.Text = "";
        this._txtHeight.Text = "";
        this._txtWidth.Text = "";
        this._txtDescription.Text = "";

        this.EnableControls();
        this._changed = true;

      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
 
    }


    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    private void OnFormResize()
    {
    //  this._BtnOk.Left = (this._GrpBottom.Width / 2) - (this._BtnOk.Width) - 5;
    //  this._BtnCancel.Left = (this._GrpBottom.Width / 2) + 5;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableControls()
    {
      this._grpBB.Enabled = this._rbBB.Checked;
      this._grpBS.Enabled = this._rbBs.Checked;

      this._BtnAdd.Enabled = !this.ActiveColisionArea.IsZero();
      this._BtnDelete.Enabled = (this._LstZones.SelectedIndex != -1);
      this._BtnUpdate.Enabled = (this._LstZones.SelectedIndex != -1) && (!this.ActiveColisionArea.IsZero());

      if (this._LstZones.SelectedItem == null)
      {
        this.AcceptButton = this._BtnAdd;
      }
      else
      {
        this.AcceptButton = this._BtnUpdate;
      }
    }


    #endregion

    private void _BtnOk_Click(object sender, EventArgs e)
    {
      try
      {
        this.Animation.Changed = true;
        this._accepted = true;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    private void _BtnCancel_Click(object sender, EventArgs e)
    {

    }

    private void ColisionZonesForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            if (this._changed && !this._accepted)
            {
                if (MessageBox.Show(this, "Data has been changed. Discard changes?", 
                            Settings.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) ==
                    DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
        catch (System.Exception ex)
        {
            Diag.ShowException(this, ex);
        }
    }

  }
}
