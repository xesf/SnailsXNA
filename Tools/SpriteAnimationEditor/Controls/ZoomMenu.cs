using System;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Controls
{
  class ZoomMenu : ToolStripMenuItem
  {
    public delegate void ZoomSelectedHandler(object sender, ZoomFactor zoom);

    public event ZoomSelectedHandler ZoomSelected;

    #region variables
    ToolStripMenuItem _optZoom1x;
    ToolStripMenuItem _optZoom2x;
    ToolStripMenuItem _optZoom4x;
    ToolStripMenuItem _optZoom8x;
    ToolStripMenuItem _optZoom16x;
    #endregion

    #region Constructors and overrides
    /// <summary>
    /// 
    /// </summary>
    public ZoomMenu()
    {
      this.Text = "Zoom";

      ToolStripMenuItem item = new ToolStripMenuItem();
      item.Text = "Zoom In";
      item.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
      item.Click += new System.EventHandler(this.ZoomIn_Click);
      this.DropDownItems.Add(item);

      item = new ToolStripMenuItem();
      item.Text = "Zoom Out";
      item.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Z)));
      item.Click += new System.EventHandler(this.ZoomOut_Click);
      this.DropDownItems.Add(item);

      this.DropDownItems.Add(new ToolStripSeparator());

      this._optZoom1x = new ToolStripMenuItem();
      this._optZoom1x.Text = ZoomFactor.Normal.ToString();
      this._optZoom1x.Click += new System.EventHandler(this.Zoom1x_Click);
      this._optZoom1x.CheckOnClick = true;
      this.DropDownItems.Add(this._optZoom1x);

      this._optZoom2x = new ToolStripMenuItem();
      this._optZoom2x.Text = ZoomFactor.x2.ToString();
      this._optZoom2x.Click += new System.EventHandler(this.Zoom2x_Click);
      this._optZoom2x.CheckOnClick = true;
      this.DropDownItems.Add(this._optZoom2x);

      this._optZoom4x = new ToolStripMenuItem();
      this._optZoom4x.Text = ZoomFactor.x4.ToString();
      this._optZoom4x.Click += new System.EventHandler(this.Zoom4x_Click);
      this._optZoom4x.CheckOnClick = true;
      this.DropDownItems.Add(this._optZoom4x);

      this._optZoom8x = new ToolStripMenuItem();
      this._optZoom8x.Text = ZoomFactor.x8.ToString();
      this._optZoom8x.Click += new System.EventHandler(this.Zoom8x_Click);
      this._optZoom8x.CheckOnClick = true;
      this.DropDownItems.Add(this._optZoom8x);

      this._optZoom16x = new ToolStripMenuItem();
      this._optZoom16x.Text = ZoomFactor.x16.ToString();
      this._optZoom16x.Click += new System.EventHandler(this.Zoom16x_Click);
      this._optZoom16x.CheckOnClick = true;
      this.DropDownItems.Add(this._optZoom16x);
    }
    #endregion

    #region Other
    /// <summary>
    /// 
    /// </summary>
    private ZoomFactor GetSelectedZoom()
    {
      if (this._optZoom1x.Checked)
        return ZoomFactor.Normal;
      if (this._optZoom2x.Checked)
        return ZoomFactor.x2;
      if (this._optZoom4x.Checked)
        return ZoomFactor.x4;
      if (this._optZoom8x.Checked)
        return ZoomFactor.x8;
      if (this._optZoom16x.Checked)
        return ZoomFactor.x16;

      this.SelectZoom(ZoomFactor.Normal);
      return ZoomFactor.Normal;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SelectZoom(ZoomFactor factor)
    {
      this._optZoom1x.Checked = (factor == ZoomFactor.x1);
      this._optZoom2x.Checked = (factor == ZoomFactor.x2);
      this._optZoom4x.Checked = (factor == ZoomFactor.x4);
      this._optZoom8x.Checked = (factor == ZoomFactor.x8);
      this._optZoom16x.Checked = (factor == ZoomFactor.x16);

      this.OnZoomSelected(factor);
    }
    #endregion

    #region Events
    /// <summary>
    /// 
    /// </summary>
    private void OnZoomSelected(ZoomFactor factor)
    {
      if (this.ZoomSelected != null)
      {
        this.ZoomSelected(this, factor);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ZoomIn_Click(object sender, EventArgs e)
    {
      ZoomFactor factor = this.GetSelectedZoom();
      if (factor == ZoomFactor.Max)
        return;

      int z = (int)factor;
      z = z << 1;
      this.SelectZoom((ZoomFactor)z);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ZoomOut_Click(object sender, EventArgs e)
    {
      ZoomFactor factor = this.GetSelectedZoom();
      if (factor == ZoomFactor.Normal)
        return;

      int z = (int)factor;
      z = z >> 1;
      this.SelectZoom((ZoomFactor)z);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom1x_Click(object sender, EventArgs e)
    {
      this.SelectZoom(ZoomFactor.Normal);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom2x_Click(object sender, EventArgs e)
    {
      this.SelectZoom(ZoomFactor.x2);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom4x_Click(object sender, EventArgs e)
    {
      this.SelectZoom(ZoomFactor.x4);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom8x_Click(object sender, EventArgs e)
    {
      this.SelectZoom(ZoomFactor.x8);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom16x_Click(object sender, EventArgs e)
    {
      this.SelectZoom(ZoomFactor.x16);
    }
    #endregion
  }
}
