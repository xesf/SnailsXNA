using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SpriteAnimationEditor.Forms
{
  public partial class AddNewFramesForm : Form, IViewState
  {
    #region Variables
    Animation _Animation;
    Controls.ZoomMenu _mnuZoom;
    bool _FrameCaptureStarted;
    Point _CaptureCorner1;
    Point _CaptureCorner2;
    bool _FrameCaptured;
    Rectangle _FrameRect;
    #endregion

    #region Properties
    private Animation Animation
    {
      get { return this._Animation; }
      set { this._Animation = value; }
    }

    public Grid Grid
    {
      get { return this._Sprite.Grid; }
      set { this._Sprite.Grid = value; }
    }

    private Point CaptureCorner1
    {
      get { return this._CaptureCorner1; }
      set 
      {
        int x = value.X;
        if (x < 0) x = 0;
        if (x >= this._Sprite.Sprite.Width - 1) x = this._Sprite.Sprite.Width - 1;

        int y = value.Y;
        if (y < 0) x = 0;
        if (y >= this._Sprite.Sprite.Height - 1) y = this._Sprite.Sprite.Height - 1;

        this._CaptureCorner1 = new Point(x, y);
      }
    }

    private Point CaptureCorner2
    {
      get { return this._CaptureCorner2; }
      set 
      {
        int x = value.X;
        if (x < 0) x = 0;
        if (x >= this._Sprite.Sprite.Width - 1) x = this._Sprite.Sprite.Width - 1;

        int y = value.Y;
        if (y < 0) y = 0;
        if (y >= this._Sprite.Sprite.Height - 1) y = this._Sprite.Sprite.Height - 1;

        this._CaptureCorner2 = new Point(x, y);
      }
    }

    private bool FrameCaptureStarted
    {
      get { return this._FrameCaptureStarted; }
      set { this._FrameCaptureStarted = value; }
    }

    private Point UpperLeftFrameCorner
    {
      get
      {
        int x1 = (this.CaptureCorner1.X < this.CaptureCorner2.X ? this.CaptureCorner1.X : this.CaptureCorner2.X);
        int y1 = (this.CaptureCorner1.Y < this.CaptureCorner2.Y ? this.CaptureCorner1.Y : this.CaptureCorner2.Y);
        return new Point(x1, y1);
      }
      set
      {
        this.CaptureCorner1 = value;
      }
    }

    private Point LowerRightFrameCorner
    {
      get
      {
        int x1 = (this.CaptureCorner1.X > this.CaptureCorner2.X ? this.CaptureCorner1.X : this.CaptureCorner2.X);
        int y1 = (this.CaptureCorner1.Y > this.CaptureCorner2.Y ? this.CaptureCorner1.Y : this.CaptureCorner2.Y);
        return new Point(x1, y1);
      }
      set
      {
        this.CaptureCorner2 = value;
      }
    }
    private Size FrameSize
    {
      get
      {
        Point ur = this.UpperLeftFrameCorner;
        Point lr = this.LowerRightFrameCorner;

        return new Size(lr.X - ur.X, lr.Y - ur.Y);
      }
    }

    public Rectangle FrameRectangle
    {
      get
      {
        if (this._FrameRect == null)
          this._FrameRect = new Rectangle();
        return this._FrameRect;
       // return new Rectangle(this.FrameLeft, this.FrameTop, this.FrameWidth, this.FrameHeight);
      }

      set
      {
/*        this.FrameLeft = value.Left;
        this.FrameTop = value.Top;
        this.FrameWidth = value.Width;
        this.FrameHeight = value.Height;*/
        this._FrameRect = value;
        this._txtX.Text = this.FrameRectangle.Left.ToString();
        this._txtY.Text = this.FrameRectangle.Top.ToString();
        this._txtWidth.Text = this.FrameRectangle.Width.ToString();
        this._txtHeight.Text = this.FrameRectangle.Height.ToString();
        this.RefreshFrame();

      }
    }

    private bool GridVisible
    {
      set
      {
        if (this._Sprite.GridVisible != value)
        {
          this._Sprite.GridVisible = value;
          this.EnableControls();
        }
      }
      get { return this._Sprite.GridVisible; }
    }

    public bool FrameCaptured
    {
      get { return this._FrameCaptured; }
      set { this._FrameCaptured = value; }
    }

    int FrameLeft
    {
      get 
      {
        int i;
        int.TryParse(this._txtX.Text, out i);
        return i;
      }
      set 
      { 
        this._txtX.Text = value.ToString();
      }
    }
    int FrameTop
    {
      get 
      {
        int i;
        int.TryParse(this._txtY.Text, out i);
        return i;
      }
      set 
      {
        this._txtY.Text = value.ToString();
      }
    }
    int FrameWidth
    {
      get 
      {
        int i;
        int.TryParse(this._txtWidth.Text, out i);
        return i;
      }
      set 
      { 
        this._txtWidth.Text = value.ToString();
      }
    }
    int FrameHeight
    {
      get 
      {
        int i;
        int.TryParse(this._txtHeight.Text, out i);
        return i;
      }
      set 
      { 
        this._txtHeight.Text = value.ToString();
      }
    }
    #endregion

    #region Constructs, overrides and control events
    /// <summary>
    /// 
    /// </summary>
    public AddNewFramesForm()
    {
      InitializeComponent();
      this._mnuZoom = new SpriteAnimationEditor.Controls.ZoomMenu();
      this._mnuZoom.ZoomSelected += new SpriteAnimationEditor.Controls.ZoomMenu.ZoomSelectedHandler(_mnuZoom_ZoomSelected);
      this._menuView.DropDownItems.Add(this._mnuZoom);
    }

    /// <summary>
    /// 
    /// </summary>
    internal DialogResult ShowAddDialog(IWin32Window owner, Animation animation)
    {
      this._BtnOk.Visible = false;
      this._btnCancel.Visible = false;
      this._BtnAdd.Visible = true;
      this._Sprite.Project = animation.ParentProject;
      this.Animation = animation;
      this.FrameRectangle = new Rectangle(0, 0, 0, 0);
      this.Text = "Add New Frames";
      return this.ShowDialog(owner);
    }

    /// <summary>
    /// 
    /// </summary>
    internal DialogResult ShowEditDialog(IWin32Window owner, AnimationFrame frame)
    {
      this._BtnOk.Visible = true;
      this._btnCancel.Visible = true;
      this._BtnAdd.Visible = false;
      this._Sprite.Project = frame.ParentAnimation.ParentProject;
      this.Animation = frame.ParentAnimation;
      this.FrameRectangle = frame.Rectangle;
      this.Text = "Edit Frame";
      return this.ShowDialog(owner);
    }

    /// <summary>
    /// 
    /// </summary>
    private void AddNewFramesForm_Load(object sender, EventArgs e)
    {
      try
      {
        if (this.Animation == null)
        {
          this.Close();
          return;
        }

        this.SetToProjectGrid();
        this._Sprite.ShowImages = Settings.Instance.ShowImages;
        this._Sprite.Refresh();
        if (this.Animation != null)
        {
          this._pnlFrame.BackColor = this.Animation.ParentProject.SpriteBackColor;
        }
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
    private void _mnuZoom_ZoomSelected(object sender, ZoomFactor zoom)
    {
      try
      {
        this._Sprite.Zoom = zoom;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optViewGrid_Click(object sender, EventArgs e)
    {
      try
      {
        this.ToggleGridVisible();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void zoomCombo1_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.Zoom = this._cmbZoom.Zoom;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_GridCellDoubleClicked(object sender, Rectangle gridRect)
    {
      try
      {
        if (this.FrameCaptureStarted == true)
          return;

        if ((Control.ModifierKeys & Keys.Control) != 0)
        {
          this.FrameRectangle = gridRect;
          this.AddFrame();
          this.EnableControls();
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_GridCellClicked(object sender, Rectangle gridRect)
    {
      try
      {
        if (this.FrameCaptureStarted == true)
          return;

        if ((Control.ModifierKeys & Keys.Control) != 0)
        {
          this.FrameRectangle = gridRect;
          this.EnableControls();
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _btnToggleGrid_Click(object sender, EventArgs e)
    {
      try
      {
        this.ToggleGridVisible();
        this.Refresh();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetToProjectGrid()
    {
      this.Grid = (Grid)this.Animation.Grid.Clone();
      this.Refresh();
      this.EnableControls();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnSetToPrjGrid_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetToProjectGrid();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_SpriteMouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        if ((Control.ModifierKeys & Keys.Control) != 0)
          return;

        this.CaptureCorner1 = new Point(e.Location.X / (int)this._Sprite.Zoom,
                                        e.Location.Y / (int)this._Sprite.Zoom);
        this.CaptureCorner2 = this.CaptureCorner1;


        this.FrameCaptureStarted = true;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_SpriteMouseMove(object sender, MouseEventArgs e)
    {
      try
      {
        int x = (e.Location.X / (int)this._Sprite.Zoom) + 1,
            y = (e.Location.Y / (int)this._Sprite.Zoom) + 1;
        this._lblPosition.Text = string.Format("{0}x{1}", x, y);
        if (this.FrameCaptureStarted == false)
          return;

        this.CaptureCorner2 = new Point(x, y);

        this.FrameLeft = this.UpperLeftFrameCorner.X;
        this.FrameTop = this.UpperLeftFrameCorner.Y;
        this.FrameWidth = (this.LowerRightFrameCorner.X - this.UpperLeftFrameCorner.X);
        this.FrameHeight = (this.LowerRightFrameCorner.Y - this.UpperLeftFrameCorner.Y);
        this._Sprite.Refresh();

      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_MouseLeave(object sender, EventArgs e)
    {
      try
      {
        this._lblPosition.Text = "";
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_SpriteMouseUp(object sender, MouseEventArgs e)
    {
      try
      {
        this.FrameCaptureStarted = false;
        this.FrameCaptured = true;
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
 /*       Pen pen = new Pen(Brushes.White);
        pen.Width = (int)this._Sprite.Zoom;
        Rectangle rect = this.FrameRectangle;
        rect = new Rectangle(((rect.X + 1) * (int)this._Sprite.Zoom) - ((int)this._Sprite.Zoom / 2),
                              ((rect.Y + 1) * (int)this._Sprite.Zoom) - ((int)this._Sprite.Zoom / 2),
                              (rect.Width - 1) * (int)this._Sprite.Zoom,
                              (rect.Height - 1) * (int)this._Sprite.Zoom);

        graphics.DrawRectangle(pen, rect);*/
      }
      catch (System.Exception)
      {
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_BeforeGridPaint(object sender, Graphics graphics)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void _Sprite_ZoomChanged(object sender, EventArgs e)
    {
      try
      {
        this._mnuZoom.SelectZoom(this._Sprite.Zoom);
        this.RefreshFrame();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _pnlFrame_Paint(object sender, PaintEventArgs e)
    {
      try
      {
        if (Settings.Instance.ShowImages == false)
          return;

        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        Rectangle destRect = new Rectangle(0, 0,
                this.FrameSize.Width * (int)this._Sprite.Zoom,
                this.FrameSize.Height * (int)this._Sprite.Zoom);

        e.Graphics.DrawImage(this._Sprite.Sprite, destRect, this.FrameRectangle, GraphicsUnit.Pixel);
      }
      catch (System.Exception)
      {
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void AddFrame()
    {
      MainForm.SolutionManager.AddFrameToAnimation(this.Animation, this.FrameRectangle);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _BtnAdd_Click(object sender, EventArgs e)
    {
      try
      {
        this.AddFrame();
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
        int left = 0;
        int.TryParse(this._txtX.Text, out left);
        this._FrameRect = new Rectangle(left, this.FrameRectangle.Top, this.FrameRectangle.Width, this.FrameRectangle.Height);
        this.RefreshFrame();
        this.RefreshSprite();
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
        int top = 0;
        int.TryParse(this._txtY.Text, out top);
        this._FrameRect = new Rectangle(this.FrameRectangle.Left, top, this.FrameRectangle.Width, this.FrameRectangle.Height);
        this.RefreshFrame();
        this.RefreshSprite();
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
        int width = 0;
        int.TryParse(this._txtWidth.Text, out width);
        this._FrameRect = new Rectangle(this.FrameRectangle.Left, this.FrameRectangle.Top, width, this.FrameRectangle.Height);
        this.RefreshFrame();
        this.RefreshSprite();
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
        int height = 0;
        int.TryParse(this._txtHeight.Text, out height);
        this._FrameRect = new Rectangle(this.FrameRectangle.Left, this.FrameRectangle.Top, this.FrameRectangle.Width, height);
        this.RefreshFrame();
        this.RefreshSprite();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }

    }
    #endregion

    #region Other
    /// <summary>
    /// 
    /// </summary>
    private void ToggleGridVisible()
    {
      if (this.GridVisible)
        this.GridVisible = false;
      else
        this.GridVisible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableControls()
    {
      this._optViewGrid.Checked = this._Sprite.Grid.Visible;
      this._BtnAdd.Enabled = this.FrameCaptured;
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshCornerValues()
    {
      int x, y, width, height;

      int.TryParse(this._txtX.Text, out x);
      int.TryParse(this._txtY.Text, out y);
      int.TryParse(this._txtWidth.Text, out width);
      int.TryParse(this._txtHeight.Text, out height);

      this.UpperLeftFrameCorner = new Point(x, y);
      this.LowerRightFrameCorner = new Point(x + width, y + height);
      this.RefreshFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshFrame()
    {
      this._pnlFrame.Size = new Size(this.FrameRectangle.Width * (int)this._Sprite.Zoom,
                                     this.FrameRectangle.Height * (int)this._Sprite.Zoom);
      this._pnlFrame.Refresh();
      this._Sprite.Frame = new AnimationFrame(this.FrameRectangle, null);
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshSprite()
    {
      this._Sprite.Refresh();
    }
    #endregion

    #region IViewState Members

    /// <summary>
    /// 
    /// </summary>
    public void InitFromXml(System.Xml.XmlElement elemParent)
    {
      XmlElement elem = (XmlElement)elemParent.SelectSingleNode(this.Name);
      if (elem == null)
        return; 

      this.Left = XmlHelper.GetAttribute(elem, "Left", 100);
      this.Top = XmlHelper.GetAttribute(elem, "Top", 100);
      this.Width = XmlHelper.GetAttribute(elem, "Width", 300);
      this.Height = XmlHelper.GetAttribute(elem, "Height", 300);

      if (this.Width < 300)
        this.Width = 300;
      if (this.Height < 300)
        this.Height = 300;

      this._Sprite.Zoom = XmlHelper.GetAttribute(elem, "Zoom", ZoomFactor.Normal);
      this._Sprite.Grid.Visible = XmlHelper.GetAttribute(elem, "GridVisible", false);
    }

    /// <summary>
    /// 
    /// </summary>
    public System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      XmlElement xmlElement = xmlDoc.CreateElement(this.Name);
      xmlElement.SetAttribute("Left", this.Left.ToString());
      xmlElement.SetAttribute("Top", this.Top.ToString());
      xmlElement.SetAttribute("Width", this.Width.ToString());
      xmlElement.SetAttribute("Height", this.Height.ToString());
      xmlElement.SetAttribute("Zoom", this._Sprite.Zoom.ToString());
      xmlElement.SetAttribute("GridVisible", this._Sprite.Grid.Visible.ToString());
      return xmlElement;
    }

    #endregion

    #region Main menu
    /// <summary>
    /// 
    /// </summary>
    private void _optGridSize_Click(object sender, EventArgs e)
    {
      try
      {
        this._Sprite.EditGridSize();
        this.EnableControls();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }


    #endregion

 

  }
}
