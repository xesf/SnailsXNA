using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SpriteAnimationEditor.Controls
{
  partial class AnimationPlayback : UserControl
  {
    #region Events
    public event EventHandler CurrentFrameChanged;
    public event EventHandler FramesPerSecondChanged;
    #endregion

    #region Variables
    int _CurrentFrameNr;
    Animation _Animation;
    AnimationFrame _CurrentFrame;
    bool _ShowImages;
    bool _Playing;
    #endregion

    #region Properties

    public bool ShowImages
    {
      get { return this._ShowImages; }
      set { this._ShowImages = value; }
    }

    public Color FrameBackColor
    {
      get { return this._PanelFrame.BackColor; }
      set { this._PanelFrame.BackColor = value; }
    }

    public Animation Animation
    {
      get { return this._Animation; }
      set
      {
        if (this._Animation != value)
        {
          this.Stop();
          this._Speed.Value = 12;

          if (value != null)
          {
            this._Animation = value;
            this.FramesPerSecond = this._Animation.FramesPerSecond;
          }
          this.MoveToFirst();
          this.EnableButtons();
        }
      }
    }

    public Animation BackgroundAnimation
    {
        get 
        {
          if (this.Animation == null)
          {
            return null;
          }
          return this.Animation.ParentAnimation; 
        }
    }

    public bool NoCurrentFrame
    {
      get
      {
        return (this.CurrentFrameNr == 0);
      }

    }

    public bool IsPlaying
    {
      get
      {
        return (this._AnimTimer.Enabled);
      }
    }

    public AnimationFrame CurrentFrame
    {
      get 
      {
        if (this.Animation == null)
          return null;

        return this.Animation.GetFrame(this.CurrentFrameNr);
      }
      set 
      {
        if (this.Animation == null)
          return;

        if (this._CurrentFrame != value)
        {
          this._CurrentFrame = value;
          this.CurrentFrameNr = this.Animation.GetFrameNr(this._CurrentFrame);
        }
      }
    }

    public AnimationFrame CurrentBackgroundFrame
    {
      get
      {
        if (this.BackgroundAnimation == null)
          return null;

        return this.BackgroundAnimation.GetFrame(this.CurrentFrameNr);
      }
    
    }

    public int CurrentFrameNr
    {
      get
      {
        return this._CurrentFrameNr;
      }
      set
      {
        if (this._CurrentFrameNr != value)
        {
          this._CurrentFrameNr = value;
         
          this.OnCurrentFrameChanged();
        }
      }
    }

    public int FramesPerSecond
    {
      get
      {
        return this._Speed.Value;
      }
      set
      {
        if (value < this._Speed.Minimum)
        {
          this._Speed.Value = this._Speed.Minimum;
        }
        else
        if (value > this._Speed.Maximum)
        {
          this._Speed.Value = this._Speed.Maximum;
        }
        else
          this._Speed.Value = value;
      }
    }

    #endregion

    #region Constructs and overrides
    /// <summary>
    /// 
    /// </summary>
    public AnimationPlayback()
    {
      InitializeComponent();
      this.EnableButtons();
      this._CurrentFrameNr = 0;
      this._LblMessage.Dock = DockStyle.Fill;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      this.ResizeFrame();
      base.Refresh();
      this.RefreshFrame();
    }
    #endregion

    #region Other methods
    /// <summary>
    /// 
    /// </summary>
    private void MoveToNext()
    {
      if (this._Animation.HasFrames == false)
        return;

      if (this.CurrentFrameNr >= this._Animation.FrameCount)
      {
        this.CurrentFrameNr = 1;
      }
      else
      {
        this.CurrentFrameNr++;
      }

      this.RefreshFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveToPrevious()
    {
      if (this._Animation.HasFrames == false)
        return;

      if (this.CurrentFrameNr == 1)
      {
        this.CurrentFrameNr = this._Animation.FrameCount;
      }
      else
      {
        this.CurrentFrameNr--;
      }

      this.RefreshFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshFrame()
    {
      if (this.CurrentFrameNr > 0 && this.CurrentFrameNr <= this.Animation.FrameCount)
      {
        this._PanelFrame.Refresh();
        this._PanelFrame.BackColor = this.Animation.ParentProject.SpriteBackColor;
        this._LblMessage.Visible = false;
        this._PanelFrame.Visible = true;
      }
      else
      {
        this._PanelFrame.Visible = false;
        this._LblMessage.Visible = true;
      }
      this.ResizeFrame();
      this._PanelFrame.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveToFirst()
    {
      if (this._Animation != null && this._Animation.FrameCount > 0)
      {
        this.CurrentFrameNr = 1;
        this.RefreshFrame();
      }
      else
      {
        this.CurrentFrameNr = 0;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Stop()
    {
      try
      {
        this._AnimTimer.Enabled = false;
        this._Play.Text = ">>";
        this._Playing = false;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EnableButtons()
    {
      if (this._Animation != null)
      {
        this._PrevFrame.Enabled = true;
        this._NextFrame.Enabled = true;
        this._Play.Enabled = true;
        this._Speed.Enabled = true;
      }
      else
      {
        this._PrevFrame.Enabled = false;
        this._NextFrame.Enabled = false;
        this._Play.Enabled = false;
        this._Speed.Enabled = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResizeFrame()
    {
      if (this.CurrentFrameNr <= 0)
        return;

      if (this.BackgroundAnimation == null)
      {
        this._PanelFrame.Width = this.CurrentFrame.Rectangle.Width + 2;
        this._PanelFrame.Height = this.CurrentFrame.Rectangle.Height + 2;
      }
      else
      {
        this._PanelFrame.Width = Math.Max(this.CurrentFrame.Rectangle.Width, this.CurrentBackgroundFrame.Rectangle.Width) + 2;
        this._PanelFrame.Height = Math.Max(this.CurrentFrame.Rectangle.Height, this.CurrentBackgroundFrame.Rectangle.Height) + 2;
      }
      this._PanelFrame.Left = (this._FramePanel.Width / 2) - (this._PanelFrame.Width / 2);
      this._PanelFrame.Top = (this._FramePanel.Height / 2) - (this._PanelFrame.Height / 2);
    }
    #endregion

    #region Control events
    /// <summary>
    /// 
    /// </summary>
    private void _Play_Click(object sender, EventArgs e)
    {
      try
      {
        if (this._Playing)
        {
          this.Stop();
          return;
        }
        
        this._Playing = true;
        this._Play.Text = "[]";

        do
        {
          if (this.CurrentFrame.PlayTime == 0)
          {
            Thread.Sleep(1000 / this.Animation.FramesPerSecond);
          }
          else
          {
            Thread.Sleep(this.CurrentFrame.PlayTime);
          }

          this.MoveToNext();
          Application.DoEvents();
        }
        while (this._Playing);
     /*   if (this.IsPlaying)
        {
          this.Stop();
        }
        else
        {
          this._AnimTimer.Enabled = true;
          this._Play.Text = "[]";
        }*/
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

  
    /// <summary>
    /// 
    /// </summary>
    private void _AnimTimer_Tick(object sender, EventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        this._AnimTimer.Enabled = false;
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _NextFrame_Click(object sender, EventArgs e)
    {
      try
      {
        this.Stop();
        this.MoveToNext();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _PrevFrame_Click(object sender, EventArgs e)
    {
      try
      {
        this.Stop();
        this.MoveToPrevious();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _Speed_ValueChanged(object sender, EventArgs e)
    {
      try
      {
    /*    if (this._Speed.Value > 0)
          this._AnimTimer.Interval = (1000 / this._Speed.Value);
        else
          this._AnimTimer.Interval = 10;
        */
        this._LblFps.Text = this._Speed.Value.ToString();
        this.OnFpsChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void AnimationPlayback_Resize(object sender, EventArgs e)
    {
      try
      {
        this._PanelControls.Left = (this.Width / 2) - (this._PanelControls.Width / 2);
        this.ResizeFrame();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _PanelFrame_Paint(object sender, PaintEventArgs e)
    {
      try
      {
        if (this.Animation == null)
          return;

        if (this.Animation.ParentProject == null)
            return;

        if (this.ShowImages == false)
          return;

        if (this.NoCurrentFrame)
          return;

        if (this.Animation.ParentProject.WithSprite == false)
          return;

        if (this.CurrentFrame != null)
        {
          if (this.Animation.ParentAnimation != null)
          {
            e.Graphics.DrawImage(this.Animation.ParentAnimation.ParentProject.OutputSprite, new Rectangle(1, 1, this.CurrentBackgroundFrame.Rectangle.Width, this.CurrentBackgroundFrame.Rectangle.Height),
                   this.CurrentBackgroundFrame.Rectangle, GraphicsUnit.Pixel);
          }
          if (this.CurrentFrame.Rotation == 0)
          {
            e.Graphics.DrawImage(this.Animation.ParentProject.OutputSprite, new Rectangle(this.CurrentFrame.Offset.X - this.CurrentFrame.Pivot.X, this.CurrentFrame.Offset.Y - this.CurrentFrame.Pivot.Y, this.CurrentFrame.Rectangle.Width, this.CurrentFrame.Rectangle.Height), this.CurrentFrame.Rectangle, GraphicsUnit.Pixel);
          }
          else
          {
            float angle = Microsoft.Xna.Framework.MathHelper.ToRadians(this.CurrentFrame.Rotation);
            Microsoft.Xna.Framework.Matrix rotMatrix = Microsoft.Xna.Framework.Matrix.CreateRotationZ(angle);
            Microsoft.Xna.Framework.Vector2 v1 = new Microsoft.Xna.Framework.Vector2(-this.CurrentFrame.Pivot.X, -this.CurrentFrame.Pivot.Y);
            Microsoft.Xna.Framework.Vector2 v2 = new Microsoft.Xna.Framework.Vector2(-this.CurrentFrame.Pivot.X + this.CurrentFrame.Rectangle.Width, - this.CurrentFrame.Pivot.Y);
            Microsoft.Xna.Framework.Vector2 v3 = new Microsoft.Xna.Framework.Vector2(-this.CurrentFrame.Pivot.X, this.CurrentFrame.Rectangle.Height - this.CurrentFrame.Pivot.Y);
         
            /*     //v1.Normalize();
//            v2.Normalize();
  //          v3.Normalize();
            //v1 = new Microsoft.Xna.Framework.Vector2((float)(v1.X * Math.Sin(angle)), (float)(v1.Y * Math.Cos(angle)));
            v2 = new Microsoft.Xna.Framework.Vector2((float)(v2.X * Math.Sin(angle)), (float)(v2.Y * Math.Cos(angle)));
            v3 = new Microsoft.Xna.Framework.Vector2((float)(v3.X * Math.Sin(angle)), (float)(v3.Y * Math.Cos(angle)));
            */

            v1 = Microsoft.Xna.Framework.Vector2.Transform(v1, rotMatrix);
            v2 = Microsoft.Xna.Framework.Vector2.Transform(v2, rotMatrix);
            v3 = Microsoft.Xna.Framework.Vector2.Transform(v3, rotMatrix);

            Point[] points = new Point[3];
            points[0] = new Point((int)v1.X + this.CurrentFrame.Offset.X, (int)v1.Y + this.CurrentFrame.Offset.Y);
            points[1] = new Point((int)v2.X + this.CurrentFrame.Offset.X, (int)v2.Y + this.CurrentFrame.Offset.Y);
            points[2] = new Point((int)v3.X + this.CurrentFrame.Offset.X, (int)v3.Y + this.CurrentFrame.Offset.Y);
            e.Graphics.DrawImage(this.Animation.ParentProject.OutputSprite, points, this.CurrentFrame.Rectangle, GraphicsUnit.Pixel);
          }
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion

    #region Animation events
    /// <summary>
    /// 
    /// </summary>
    void Animation_FrameAdded(object sender, AnimationFrame frame)
    {
      try
      {
        if (this.Animation.FrameCount == 1)
        {
          MoveToFirst();
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
    public void FrameRemoved(AnimationFrame frame)
    {
      if (this.CurrentFrame == frame)
      {
        this.MoveToPrevious();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Animation_FramesPerSecondChanged(object sender, EventArgs e)
    {
      try
      {
        this._Speed.Value = this._Animation.FramesPerSecond;
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
    private void OnCurrentFrameChanged()
    {
      this._LblCurrentFrameNr.Text = this.CurrentFrameNr.ToString();
      this.RefreshFrame();
      if (this.CurrentFrameChanged != null)
      {
        this.CurrentFrameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnFpsChanged()
    {
      if (this.FramesPerSecondChanged != null)
      {
        this.FramesPerSecondChanged(this, new EventArgs());
      }
    }   
    #endregion
  }
}
