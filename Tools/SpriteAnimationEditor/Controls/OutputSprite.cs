using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SpriteAnimationEditor.Controls
{
  internal partial class OutputSprite : UserControl
  {

    #region Consts
    const string CATEGORY_GRID = "Grid";
    #endregion

    #region Events
    public delegate void FrameClickedHandler(object sender, AnimationFrame frame);
    public delegate void GridCellClickedHandler(object sender, Rectangle gridRect);
    public delegate void GraphicsHandler(object sender, Graphics graphics);

    public event FrameClickedHandler FrameClicked;
    public event GridCellClickedHandler GridCellClicked;
    public event GridCellClickedHandler GridCellDoubleClicked;
    public event MouseEventHandler SpriteClicked;
    public event MouseEventHandler SpriteMouseMove;
    public event MouseEventHandler SpriteMouseDown;
    public event MouseEventHandler SpriteMouseUp;
    public event GraphicsHandler SpritePaint;
    public event GraphicsHandler BeforeGridPaint;
    public event EventHandler ZoomChanged;
    public event EventHandler GridChanged;
    public event EventHandler SelectedFrameChanged;
    #endregion

    #region Variables
    Project _Project;
    Animation _Animation;
    AnimationFrame _Frame;
    Color _FrameColor;
    List<AnimationFrame> _SelectedAnimationFrames;
    bool _ShowImages;
    ZoomFactor _Zoom;
    // Grid
    Grid _Grid;
    Color _GridColor;
    #endregion

    #region Properties

	public bool AllowFrameClick { get; set; }

    private Point ScrollBar
    {
      get
      {
        return new Point(this._PanelContainer.HorizontalScroll.Value,
                      this._PanelContainer.VerticalScroll.Value);
      }

    }
    
    public Project Project
    {
      get { return this._Project; }
      set
      {
        if (this._Project != value)
        {
          if (value != null)
          {
            this._Project = value;
          }
          this.Animation = null;
          this.SelectedAnimationFrames.Clear();
          this.MyRefresh();
        }
      }
    }
   
    public Animation Animation
    {
      get { return this._Animation; }
      set
      {
        if (this.Animation != value)
        {
          if (value != null)
          {
            this._Animation = value;
          }
          this.MyRefresh();
        }
      }
    }
    public Color SpriteBackColor
    {
      get { return this._PanelImage.BackColor; }
      set { this._PanelImage.BackColor = value; }
    }


    public bool ShowImages
    {
      get { return this._ShowImages; }
      set 
      {
        if (this._ShowImages != value)
        {
          this._ShowImages = value;
          this.MyRefresh();
        }
      }
    }

    public  List<AnimationFrame> SelectedAnimationFrames
    {
      get 
      {
        if (this._SelectedAnimationFrames == null)
          this._SelectedAnimationFrames = new List<AnimationFrame>();
        return this._SelectedAnimationFrames; 
      }
      private set
      {
        this._SelectedAnimationFrames = value;
        this.MyRefresh();
      }
    }

	public AnimationFrame SelectedAnimationFrame
	{
		set
		{
			this.SelectedAnimationFrames.Clear();
            if (value != null)
            {
                this.SelectedAnimationFrames.Add(value);
            }
			this.MyRefresh();
		}
	}

    public Image Sprite
    {
      get
      {
        if (this.Project == null)
          return null;

        return this.Project.OutputSprite;
      }
    }

    public Grid Grid
    {
      get { return this._Grid; }
      set
      {
        if (this._Grid != value)
        {
          this._Grid = value;
          this.MyRefresh();
        }
      }
    }

    public bool GridVisible
    {
      get { return this.Grid.Visible; }
      set
      {
        this.Grid.Visible = value;
        this.MyRefresh();
      }
    }

    #endregion

    #region Browsable attributes
    [Browsable(true)]
    [DefaultValue(true)]
    public bool CaptionVisible
    {
      get { return this._Title.CaptionVisible; }
      set { this._Title.CaptionVisible = value; }
    }

    [Browsable(true)]
    public ZoomFactor Zoom
    {
      get { return this._Zoom; }
      set
      { 
        if (this._Zoom != value)
        {
          this._Zoom = value;
          this.MyRefresh();
          this.OnZoomChanged();
        }
      }
    }
  

    [BrowsableAttribute(true)]
    [CategoryAttribute(CATEGORY_GRID)]
    public Color GridColor
    {
      get { return this._GridColor; }
      set
      {
        if (this._GridColor != value)
        {
          this._GridColor = value;
          this.MyRefresh();
        }
      }
    }
    #endregion

    [Browsable(false)]
    public AnimationFrame Frame
    {
      get { return this._Frame; }
      set 
      { 
        this._Frame = value;
        this.Invalidate();
      }
    }

    [Browsable(false)]
    public Color FrameColor
    {
      get { return this._FrameColor; }
      set
      {
        this._FrameColor = value;
        this.Invalidate();
      }
    }
    #region Class constructs, control events and overloadings
    /// <summary>
    /// 
    /// </summary>
    public OutputSprite()
    {
      InitializeComponent();
      this._lblNoSpriteMsg.Dock = DockStyle.Fill;
      this.GridColor = Color.Black;
      this.Zoom = ZoomFactor.Normal;
      this.Grid = new Grid();
      this.FrameColor = Color.White;
	  this.AllowFrameClick = true;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void MyRefresh()
    {
      if (this.Sprite != null)
      {
        this._PanelImage.Visible = true;
        this._PanelImage.Width = this.Sprite.Width * (int)this.Zoom;
        this._PanelImage.Height = this.Sprite.Height * (int)this.Zoom;
        this._PanelImage.Left = 5 - this.ScrollBar.X;
        this._PanelImage.Top = 5 - this.ScrollBar.Y;
        this._lblNoSpriteMsg.Visible = false;
        this._PanelImage.Refresh();
      }
      else
      {
        this._PanelImage.Visible = false;
        this._lblNoSpriteMsg.Visible = true;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_Paint(object sender, PaintEventArgs e)
    {
      try
      {

        if (DesignMode) { base.OnPaint(e); return; }

        e.Graphics.Clear(this.Project.SpriteBackColor);
        if (this.Sprite == null)
        {
          return;
        }

        // Image
        if (this.ShowImages)
        {
          Rectangle source = new Rectangle(0, 0, this.Sprite.Width, this.Sprite.Height);
          Rectangle dest = new Rectangle((int)this.Zoom / 2, (int)this.Zoom / 2, this.Sprite.Width * (int)this.Zoom, this.Sprite.Height * (int)this.Zoom);
          e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
          e.Graphics.DrawImage(this.Sprite, dest, source, GraphicsUnit.Pixel);
        }

        // Frame numbers
        if (Animation != null)
        {
          foreach (AnimationFrame animationFrame in this.Animation.Frames)
          {
            Brush brush = new SolidBrush(this.Animation.ParentProject.FrameNumbersColor);
            e.Graphics.DrawString(animationFrame.FrameNr.ToString(), new Font("Tahoma", 10), brush,
                      new PointF(animationFrame.Rectangle.Left * (int)this.Zoom, animationFrame.Rectangle.Top* (int)this.Zoom));
          }
        }

        this.OnBeforeGridPaint(e.Graphics);
        // Grid
        if (this.Grid.Visible)
        {
          // Pixel grid
          Pen pen = new Pen(this.GridColor);
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
          if (this.Zoom >= ZoomFactor.x8)
          {
            for (int i = 0; i < this.Sprite.Width * (int)this.Zoom; i++)
            {
              e.Graphics.DrawLine(pen, i * (int)this.Zoom, 0, i * (int)this.Zoom, this.Sprite.Height * (int)this.Zoom);
            }

            for (int i = 0; i < this.Sprite.Height * (int)this.Zoom; i++)
            {
              e.Graphics.DrawLine(pen, 0, i * (int)this.Zoom, this.Sprite.Width * (int)this.Zoom, i * (int)this.Zoom);
            }
          }

          // cells grids
          pen = new Pen(this.GridColor);
          pen.DashStyle = DashStyle.Dash;
          if (this.Zoom >= ZoomFactor.x8)
          {
            pen.DashStyle = DashStyle.Solid;
          }
          // Vertical lines
          for (int i = 0; i < this.Sprite.Width * (int)this.Zoom; i += this.Grid.Width * (int)this.Zoom)
          {
            e.Graphics.DrawLine(pen, i + this.Grid.OffsetX ,
                                    0,
                                    i + this.Grid.OffsetX,
                                    this.Sprite.Height * (int)this.Zoom);
          }

          for (int i = 0; i < this.Sprite.Height * (int)this.Zoom; i += this.Grid.Height * (int)this.Zoom)
          {
            e.Graphics.DrawLine(pen, 0, i + this.Grid.OffsetY, this.Sprite.Width * (int)this.Zoom, i + this.Grid.OffsetY);
          }
        }

        // Selected frame
        if (this.SelectedAnimationFrames != null && this.Animation != null && this.Animation.ParentProject != null)
        {
          Pen pen = new Pen(this.Animation.ParentProject.FrameRectangleColor);
          foreach (AnimationFrame animationFrame in this.SelectedAnimationFrames)
          {
            this.DrawFrame(animationFrame, pen, e.Graphics);
          }
        }

        if (this.Frame != null)
        {
          Pen pen = new Pen(this.FrameColor);
          this.DrawFrame(this.Frame, pen, e.Graphics);
        }
        this.OnSpritePaint(e.Graphics);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void DrawFrame(AnimationFrame frame, Pen pen, Graphics graphics)
    {
      if (frame == null)
      {
        return;
      }
      Rectangle zoomedFrameRect = new Rectangle
       (frame.Rectangle.Left * (int)this.Zoom, frame.Rectangle.Top * (int)this.Zoom,
        frame.Rectangle.Width * (int)this.Zoom, frame.Rectangle.Height * (int)this.Zoom);

      graphics.DrawRectangle(pen, zoomedFrameRect);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_MouseDown(object sender, MouseEventArgs e)
    {
      this._PanelImage.Capture = true;
      this.OnSpriteMouseDown(e);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_MouseMove(object sender, MouseEventArgs e)
    {
      this.OnSpriteMouseMove(e);
      this._PanelImage.Invalidate();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_MouseUp(object sender, MouseEventArgs e)
    {
      this._PanelImage.Capture = false;
      this.OnSpriteMouseUp(e);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSpriteClick(MouseEventArgs e, bool doubleClick)
    {

      if (this.Grid.Visible)
      {
        int x = (e.Location.X - this.Grid.OffsetX) / (this.Grid.Width * (int)this.Zoom);
        int y = (e.Location.Y - this.Grid.OffsetY) / (this.Grid.Height * (int)this.Zoom);

        if (doubleClick)
        {
          this.OnGridCellDoubleClicked(
           new Rectangle(
                (x * this.Grid.Width) + this.Grid.OffsetX,
                (y * this.Grid.Height) + this.Grid.OffsetY,
               this.Grid.Width, this.Grid.Height));
        }
        else
        {
          this.OnGridCellClicked(
                  new Rectangle(
                       (x * this.Grid.Width) + this.Grid.OffsetX,
                       (y * this.Grid.Height) + this.Grid.OffsetY,
                      this.Grid.Width, this.Grid.Height));
        }
      }
      int mx = e.Location.X / (int)this.Zoom;
      int my = e.Location.Y / (int)this.Zoom;
      MouseEventArgs mouseArgs = new MouseEventArgs(e.Button, e.Clicks, mx, my, e.Delta);
      this.OnSpriteClicked(mouseArgs);

	  if (this.Animation != null && this.AllowFrameClick)
      {
        foreach (AnimationFrame frame in this.Animation.Frames)
        {
          Rectangle zoomedFrameRect = new Rectangle(
          frame.Rectangle.Left * (int)this.Zoom,
          frame.Rectangle.Top * (int)this.Zoom,
          frame.Rectangle.Width * (int)this.Zoom,
          frame.Rectangle.Height * (int)this.Zoom);

          if (zoomedFrameRect.Contains(e.Location))
          {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
              this.SelectedAnimationFrames.Add(frame);
            }
            else
            {
              this.SelectedAnimationFrames.Clear();
              this.SelectedAnimationFrames.Add(frame);
              this.Refresh();
            }
            this.OnSelectedFrameChanged();
//            this.OnFrameClicked(frame);
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        this.OnSpriteClick(e, false);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelImage_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      try
      {
        this.OnSpriteClick(e, true);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }

    }

    #endregion

    #region Event lauching

    /// <summary>
    /// 
    /// </summary>
    void OnFrameClicked(AnimationFrame frame)
    {
      if (this.FrameClicked != null)
      {
        this.FrameClicked(this, frame);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGridCellClicked(Rectangle gridRect)
    {
      if (this.GridCellClicked != null)
      {
        this.GridCellClicked(this, gridRect);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGridCellDoubleClicked(Rectangle gridRect)
    {
      if (this.GridCellDoubleClicked != null)
      {
        this.GridCellDoubleClicked(this, gridRect);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSpriteClicked(MouseEventArgs e)
    {
      if (this.SpriteClicked != null)
      {
        this.SpriteClicked(this, e);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSpriteMouseDown(MouseEventArgs e)
    {
      if (this.SpriteMouseDown != null)
      {
        this.SpriteMouseDown(this, e);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSpriteMouseUp(MouseEventArgs e)
    {
      if (this.SpriteMouseUp != null)
      {
        this.SpriteMouseUp(this, e);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSpriteMouseMove(MouseEventArgs e)
    {
      if (this.SpriteMouseMove != null)
      {
        this.SpriteMouseMove(this, e);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSpritePaint(Graphics graphics)
    {
      if (this.SpritePaint != null)
      {
        this.SpritePaint(this, graphics);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnZoomChanged()
    {
      if (this.ZoomChanged != null)
      {
        this.ZoomChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGridChanged()
    {
      if (this.GridChanged != null)
      {
        this.GridChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSelectedFrameChanged()
    {
      if (this.SelectedFrameChanged != null)
      {
        this.SelectedFrameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnBeforeGridPaint(Graphics graphics)
    {
      if (this.BeforeGridPaint != null)
      {
        this.BeforeGridPaint(this, graphics);
      }
    }
    #endregion 

    #region Other
    /// <summary>
    /// 
    /// </summary>
    public void SetSelectedAnimationFrames(List<AnimationFrame> frameList)
    {
      this.SelectedAnimationFrames = frameList;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetSelectedAnimationFrames(AnimationFrame frame)
    {
      List<AnimationFrame> frameList = new List<AnimationFrame>();
      frameList.Add(frame);
      this.SelectedAnimationFrames = frameList;
    }
    /// <summary>
    /// 
    /// </summary>
    public void SelectFirstFrame()
    {
      if (this.Animation.Frames.Count == 0)
        return;

      this.SelectedAnimationFrames.Clear();
      this.SelectedAnimationFrames.Add(this.Animation.Frames[0]);
    }
    /// <summary>
    /// 
    /// </summary>
    public void ZoomIn()
    {
      if (this.Zoom == ZoomFactor.Max)
        return;
      int z = (int)this.Zoom;
      z = z << 1;
      this.Zoom = (ZoomFactor)z;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ZoomOut()
    {
      if (this.Zoom == ZoomFactor.Normal)
        return;

      int z = (int)this.Zoom;
      z = z >> 1;
      this.Zoom = (ZoomFactor)z;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void EditGridSize()
    {
      Forms.GridForm form = new Forms.GridForm();
      form.Grid = this.Grid;
      if (form.ShowDialog(this.ParentForm) == DialogResult.OK)
      {
        this.Grid = form.Grid;
        this.OnGridChanged();
      }
    }
  }
}
