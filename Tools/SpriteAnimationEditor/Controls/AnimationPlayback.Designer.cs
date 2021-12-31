namespace SpriteAnimationEditor.Controls
{
  partial class AnimationPlayback
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this._AnimTimer = new System.Windows.Forms.Timer(this.components);
      this.group1 = new SpriteAnimationEditor.Controls.Group();
      this._FramePanel = new System.Windows.Forms.Panel();
      this._PanelFrame = new System.Windows.Forms.Panel();
      this._LblMessage = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._PanelControls = new System.Windows.Forms.Panel();
      this._PrevFrame = new System.Windows.Forms.Button();
      this._LblFps = new System.Windows.Forms.Label();
      this._NextFrame = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this._LblCurrentFrameNr = new System.Windows.Forms.Label();
      this._Speed = new System.Windows.Forms.TrackBar();
      this._Play = new System.Windows.Forms.Button();
      this.group1.BodyPanel.SuspendLayout();
      this.group1.SuspendLayout();
      this._FramePanel.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this._PanelControls.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this._Speed)).BeginInit();
      this.SuspendLayout();
      // 
      // _AnimTimer
      // 
      this._AnimTimer.Interval = 50;
      this._AnimTimer.Tick += new System.EventHandler(this._AnimTimer_Tick);
      // 
      // group1
      // 
      // 
      // group1._PanelBody
      // 
      this.group1.BodyPanel.BackColor = System.Drawing.Color.Transparent;
      this.group1.BodyPanel.Controls.Add(this._FramePanel);
      this.group1.BodyPanel.Controls.Add(this.groupBox1);
      this.group1.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this.group1.BodyPanel.Name = "_PanelBody";
      this.group1.BodyPanel.Size = new System.Drawing.Size(260, 218);
      this.group1.BodyPanel.TabIndex = 2;
      this.group1.CaptionBackColor = System.Drawing.Color.DarkGray;
      this.group1.CaptionForeColor = System.Drawing.Color.Black;
      this.group1.CaptionVisible = true;
      this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.Location = new System.Drawing.Point(0, 0);
      this.group1.Name = "group1";
      this.group1.Size = new System.Drawing.Size(260, 236);
      this.group1.TabIndex = 5;
      this.group1.Text = "Animation Playback";
      // 
      // _FramePanel
      // 
      this._FramePanel.AutoScroll = true;
      this._FramePanel.Controls.Add(this._PanelFrame);
      this._FramePanel.Controls.Add(this._LblMessage);
      this._FramePanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._FramePanel.Location = new System.Drawing.Point(0, 0);
      this._FramePanel.Name = "_FramePanel";
      this._FramePanel.Size = new System.Drawing.Size(260, 142);
      this._FramePanel.TabIndex = 6;
      // 
      // _PanelFrame
      // 
      this._PanelFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._PanelFrame.Location = new System.Drawing.Point(24, 11);
      this._PanelFrame.Name = "_PanelFrame";
      this._PanelFrame.Size = new System.Drawing.Size(70, 73);
      this._PanelFrame.TabIndex = 5;
      this._PanelFrame.Paint += new System.Windows.Forms.PaintEventHandler(this._PanelFrame_Paint);
      // 
      // _LblMessage
      // 
      this._LblMessage.Location = new System.Drawing.Point(100, 29);
      this._LblMessage.Name = "_LblMessage";
      this._LblMessage.Size = new System.Drawing.Size(124, 82);
      this._LblMessage.TabIndex = 4;
      this._LblMessage.Text = "No frames in animation";
      this._LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._PanelControls);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox1.Location = new System.Drawing.Point(0, 142);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(260, 76);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      // 
      // _PanelControls
      // 
      this._PanelControls.Controls.Add(this._PrevFrame);
      this._PanelControls.Controls.Add(this._LblFps);
      this._PanelControls.Controls.Add(this._NextFrame);
      this._PanelControls.Controls.Add(this.label1);
      this._PanelControls.Controls.Add(this._LblCurrentFrameNr);
      this._PanelControls.Controls.Add(this._Speed);
      this._PanelControls.Controls.Add(this._Play);
      this._PanelControls.Location = new System.Drawing.Point(7, 10);
      this._PanelControls.Name = "_PanelControls";
      this._PanelControls.Size = new System.Drawing.Size(196, 61);
      this._PanelControls.TabIndex = 7;
      // 
      // _PrevFrame
      // 
      this._PrevFrame.Location = new System.Drawing.Point(0, 3);
      this._PrevFrame.Name = "_PrevFrame";
      this._PrevFrame.Size = new System.Drawing.Size(31, 23);
      this._PrevFrame.TabIndex = 0;
      this._PrevFrame.Text = "<";
      this._PrevFrame.UseVisualStyleBackColor = true;
      this._PrevFrame.Click += new System.EventHandler(this._PrevFrame_Click);
      // 
      // _LblFps
      // 
      this._LblFps.AutoSize = true;
      this._LblFps.Location = new System.Drawing.Point(174, 39);
      this._LblFps.Name = "_LblFps";
      this._LblFps.Size = new System.Drawing.Size(19, 13);
      this._LblFps.TabIndex = 6;
      this._LblFps.Text = "24";
      // 
      // _NextFrame
      // 
      this._NextFrame.Location = new System.Drawing.Point(37, 3);
      this._NextFrame.Name = "_NextFrame";
      this._NextFrame.Size = new System.Drawing.Size(31, 23);
      this._NextFrame.TabIndex = 1;
      this._NextFrame.Text = ">";
      this._NextFrame.UseVisualStyleBackColor = true;
      this._NextFrame.Click += new System.EventHandler(this._NextFrame_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(-3, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Speed (fps)";
      // 
      // _LblCurrentFrameNr
      // 
      this._LblCurrentFrameNr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._LblCurrentFrameNr.Location = new System.Drawing.Point(74, 3);
      this._LblCurrentFrameNr.Name = "_LblCurrentFrameNr";
      this._LblCurrentFrameNr.Size = new System.Drawing.Size(80, 23);
      this._LblCurrentFrameNr.TabIndex = 2;
      this._LblCurrentFrameNr.Text = "0";
      this._LblCurrentFrameNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // _Speed
      // 
      this._Speed.AutoSize = false;
      this._Speed.Location = new System.Drawing.Point(64, 35);
      this._Speed.Maximum = 60;
      this._Speed.Minimum = 1;
      this._Speed.Name = "_Speed";
      this._Speed.Size = new System.Drawing.Size(104, 29);
      this._Speed.TabIndex = 4;
      this._Speed.TickFrequency = 10;
      this._Speed.TickStyle = System.Windows.Forms.TickStyle.None;
      this._Speed.Value = 1;
      this._Speed.ValueChanged += new System.EventHandler(this._Speed_ValueChanged);
      // 
      // _Play
      // 
      this._Play.Location = new System.Drawing.Point(162, 3);
      this._Play.Name = "_Play";
      this._Play.Size = new System.Drawing.Size(31, 23);
      this._Play.TabIndex = 3;
      this._Play.Text = ">>";
      this._Play.UseVisualStyleBackColor = true;
      this._Play.Click += new System.EventHandler(this._Play_Click);
      // 
      // AnimationPlayback
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.group1);
      this.Name = "AnimationPlayback";
      this.Size = new System.Drawing.Size(260, 236);
      this.Resize += new System.EventHandler(this.AnimationPlayback_Resize);
      this.group1.BodyPanel.ResumeLayout(false);
      this.group1.ResumeLayout(false);
      this._FramePanel.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this._PanelControls.ResumeLayout(false);
      this._PanelControls.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this._Speed)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _Play;
    private System.Windows.Forms.Label _LblCurrentFrameNr;
    private System.Windows.Forms.Button _NextFrame;
    private System.Windows.Forms.Button _PrevFrame;
    private System.Windows.Forms.Timer _AnimTimer;
    private System.Windows.Forms.Label _LblFps;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TrackBar _Speed;
    private SpriteAnimationEditor.Controls.Group group1;
    private System.Windows.Forms.Label _LblMessage;
    private System.Windows.Forms.Panel _PanelControls;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Panel _FramePanel;
    private System.Windows.Forms.Panel _PanelFrame;
  }
}
