using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpriteAnimationEditor.Forms
{
  partial class MoveFramesForm : Form
  {
    private List<AnimationFrame> _selectedFrames;
    private Animation _animation;
    List<AnimationFrame> _framesMoved;

    private int _movedX;
    private int _movedY;

    public int OffsetX
    {
      get
      {
        int x;
        int.TryParse(this._txtOffsetX.Text, out x);

        return x;
      }
    }

    public int OffsetY
    {
      get
      {
        int y;
        int.TryParse(this._txtOffsetY.Text, out y);

        return y;
      }
    }

    public MoveFramesForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    public DialogResult ShowDialog(Animation animation, List<AnimationFrame> selectedFrames)
    {
      this._animation = animation;
      this._selectedFrames = selectedFrames;

      return this.ShowDialog();
    }

    private void MoveFramesForm_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtOffsetX_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.MoveFrames(this.OffsetX, this.OffsetY);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _txtOffsetY_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.MoveFrames(this.OffsetX, this.OffsetY);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveFrames(int x, int y)
    {
      List<AnimationFrame> frames;
      if (this._chkSelectedOnly.Checked)
      {
        frames = this._selectedFrames;
      }
      else
      {
        frames = this._animation.Frames;
      }

      foreach (AnimationFrame frame in frames)
      {
        frame.Rectangle = new Rectangle(frame.Rectangle.X + x - this._movedX, 
                                        frame.Rectangle.Y + y - this._movedY, 
                                        frame.Rectangle.Width, frame.Rectangle.Height);
      }

      Settings.FormMainForm.RefreshFrames();
      this._movedX = x;
      this._movedY = y;
      this._framesMoved = frames;
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {

    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveFrames(0, 0);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this, ex);
      }
    }
  }
}
