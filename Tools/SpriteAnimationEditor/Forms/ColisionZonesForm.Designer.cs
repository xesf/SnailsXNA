namespace SpriteAnimationEditor.Forms
{
  partial class ColisionZonesForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      SpriteAnimationEditor.Grid grid2 = new SpriteAnimationEditor.Grid();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._grpBS = new System.Windows.Forms.GroupBox();
      this._txtBSX = new System.Windows.Forms.TextBox();
      this._rbBs = new System.Windows.Forms.RadioButton();
      this.label6 = new System.Windows.Forms.Label();
      this._txtBSRadius = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this._txtBSY = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this._rbBB = new System.Windows.Forms.RadioButton();
      this._grpBB = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this._txtDescription = new System.Windows.Forms.TextBox();
      this._txtX = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._txtWidth = new System.Windows.Forms.TextBox();
      this._txtHeight = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this._txtY = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this._BtnDelete = new System.Windows.Forms.Button();
      this._BtnAdd = new System.Windows.Forms.Button();
      this._BtnUpdate = new System.Windows.Forms.Button();
      this._LstZones = new System.Windows.Forms.ListBox();
      this._GrpBottom = new System.Windows.Forms.GroupBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this._BtnOk = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this._BtnCancel = new System.Windows.Forms.Button();
      this._Sprite = new SpriteAnimationEditor.Controls.OutputSprite();
      this.label9 = new System.Windows.Forms.Label();
      this._txtBSDescription = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this._grpBS.SuspendLayout();
      this._grpBB.SuspendLayout();
      this._GrpBottom.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._rbBs);
      this.groupBox1.Controls.Add(this._grpBS);
      this.groupBox1.Controls.Add(this._rbBB);
      this.groupBox1.Controls.Add(this._grpBB);
      this.groupBox1.Controls.Add(this._BtnDelete);
      this.groupBox1.Controls.Add(this._BtnAdd);
      this.groupBox1.Controls.Add(this._BtnUpdate);
      this.groupBox1.Controls.Add(this._LstZones);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
      this.groupBox1.Location = new System.Drawing.Point(343, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(376, 407);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      // 
      // _grpBS
      // 
      this._grpBS.Controls.Add(this.label9);
      this._grpBS.Controls.Add(this._txtBSX);
      this._grpBS.Controls.Add(this._txtBSDescription);
      this._grpBS.Controls.Add(this.label6);
      this._grpBS.Controls.Add(this._txtBSRadius);
      this._grpBS.Controls.Add(this.label7);
      this._grpBS.Controls.Add(this._txtBSY);
      this._grpBS.Controls.Add(this.label8);
      this._grpBS.Location = new System.Drawing.Point(211, 178);
      this._grpBS.Name = "_grpBS";
      this._grpBS.Size = new System.Drawing.Size(153, 127);
      this._grpBS.TabIndex = 23;
      this._grpBS.TabStop = false;
      // 
      // _txtBSX
      // 
      this._txtBSX.Location = new System.Drawing.Point(61, 19);
      this._txtBSX.Name = "_txtBSX";
      this._txtBSX.Size = new System.Drawing.Size(43, 20);
      this._txtBSX.TabIndex = 0;
      this._txtBSX.TextChanged += new System.EventHandler(this._txtBSX_TextChanged);
      // 
      // _rbBs
      // 
      this._rbBs.AutoSize = true;
      this._rbBs.Location = new System.Drawing.Point(220, 175);
      this._rbBs.Name = "_rbBs";
      this._rbBs.Size = new System.Drawing.Size(107, 17);
      this._rbBs.TabIndex = 22;
      this._rbBs.TabStop = true;
      this._rbBs.Text = "Bounding Sphere";
      this._rbBs.UseVisualStyleBackColor = true;
      this._rbBs.CheckedChanged += new System.EventHandler(this._rbBs_CheckedChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(16, 44);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(14, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Y";
      // 
      // _txtBSRadius
      // 
      this._txtBSRadius.Location = new System.Drawing.Point(61, 64);
      this._txtBSRadius.Name = "_txtBSRadius";
      this._txtBSRadius.Size = new System.Drawing.Size(43, 20);
      this._txtBSRadius.TabIndex = 2;
      this._txtBSRadius.TextChanged += new System.EventHandler(this._txtBSRadius_TextChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(15, 67);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(40, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Radius";
      // 
      // _txtBSY
      // 
      this._txtBSY.Location = new System.Drawing.Point(61, 41);
      this._txtBSY.Name = "_txtBSY";
      this._txtBSY.Size = new System.Drawing.Size(43, 20);
      this._txtBSY.TabIndex = 1;
      this._txtBSY.TextChanged += new System.EventHandler(this._txtBSY_TextChanged);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(16, 22);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(14, 13);
      this.label8.TabIndex = 8;
      this.label8.Text = "X";
      // 
      // _rbBB
      // 
      this._rbBB.AutoSize = true;
      this._rbBB.Location = new System.Drawing.Point(217, 8);
      this._rbBB.Name = "_rbBB";
      this._rbBB.Size = new System.Drawing.Size(91, 17);
      this._rbBB.TabIndex = 0;
      this._rbBB.TabStop = true;
      this._rbBB.Text = "Bounding Box";
      this._rbBB.UseVisualStyleBackColor = true;
      this._rbBB.CheckedChanged += new System.EventHandler(this._rbBB_CheckedChanged);
      // 
      // _grpBB
      // 
      this._grpBB.Controls.Add(this.label5);
      this._grpBB.Controls.Add(this._txtDescription);
      this._grpBB.Controls.Add(this._txtX);
      this._grpBB.Controls.Add(this.label4);
      this._grpBB.Controls.Add(this.label2);
      this._grpBB.Controls.Add(this._txtWidth);
      this._grpBB.Controls.Add(this._txtHeight);
      this._grpBB.Controls.Add(this.label3);
      this._grpBB.Controls.Add(this._txtY);
      this._grpBB.Controls.Add(this.label1);
      this._grpBB.Location = new System.Drawing.Point(211, 12);
      this._grpBB.Name = "_grpBB";
      this._grpBB.Size = new System.Drawing.Size(153, 157);
      this._grpBB.TabIndex = 0;
      this._grpBB.TabStop = false;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(15, 108);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(60, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Description";
      // 
      // _txtDescription
      // 
      this._txtDescription.Location = new System.Drawing.Point(18, 124);
      this._txtDescription.Name = "_txtDescription";
      this._txtDescription.Size = new System.Drawing.Size(129, 20);
      this._txtDescription.TabIndex = 15;
      // 
      // _txtX
      // 
      this._txtX.Location = new System.Drawing.Point(61, 19);
      this._txtX.Name = "_txtX";
      this._txtX.Size = new System.Drawing.Size(43, 20);
      this._txtX.TabIndex = 0;
      this._txtX.TextChanged += new System.EventHandler(this._txtX_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(15, 88);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(38, 13);
      this.label4.TabIndex = 14;
      this.label4.Text = "Height";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Y";
      // 
      // _txtWidth
      // 
      this._txtWidth.Location = new System.Drawing.Point(61, 63);
      this._txtWidth.Name = "_txtWidth";
      this._txtWidth.Size = new System.Drawing.Size(43, 20);
      this._txtWidth.TabIndex = 2;
      this._txtWidth.TextChanged += new System.EventHandler(this._txtWidth_TextChanged);
      // 
      // _txtHeight
      // 
      this._txtHeight.Location = new System.Drawing.Point(61, 85);
      this._txtHeight.Name = "_txtHeight";
      this._txtHeight.Size = new System.Drawing.Size(43, 20);
      this._txtHeight.TabIndex = 3;
      this._txtHeight.TextChanged += new System.EventHandler(this._txtHeight_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(15, 66);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(35, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Width";
      // 
      // _txtY
      // 
      this._txtY.Location = new System.Drawing.Point(61, 41);
      this._txtY.Name = "_txtY";
      this._txtY.Size = new System.Drawing.Size(43, 20);
      this._txtY.TabIndex = 1;
      this._txtY.TextChanged += new System.EventHandler(this._txtY_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "X";
      // 
      // _BtnDelete
      // 
      this._BtnDelete.Location = new System.Drawing.Point(211, 369);
      this._BtnDelete.Name = "_BtnDelete";
      this._BtnDelete.Size = new System.Drawing.Size(153, 23);
      this._BtnDelete.TabIndex = 4;
      this._BtnDelete.Text = "&Delete";
      this._BtnDelete.UseVisualStyleBackColor = true;
      this._BtnDelete.Click += new System.EventHandler(this._BtnDelete_Click);
      // 
      // _BtnAdd
      // 
      this._BtnAdd.Location = new System.Drawing.Point(211, 311);
      this._BtnAdd.Name = "_BtnAdd";
      this._BtnAdd.Size = new System.Drawing.Size(153, 23);
      this._BtnAdd.TabIndex = 2;
      this._BtnAdd.Text = "&Add";
      this._BtnAdd.UseVisualStyleBackColor = true;
      this._BtnAdd.Click += new System.EventHandler(this._BtnAdd_Click);
      // 
      // _BtnUpdate
      // 
      this._BtnUpdate.Location = new System.Drawing.Point(211, 340);
      this._BtnUpdate.Name = "_BtnUpdate";
      this._BtnUpdate.Size = new System.Drawing.Size(153, 23);
      this._BtnUpdate.TabIndex = 3;
      this._BtnUpdate.Text = "&Update";
      this._BtnUpdate.UseVisualStyleBackColor = true;
      this._BtnUpdate.Click += new System.EventHandler(this._BtnUpdate_Click);
      // 
      // _LstZones
      // 
      this._LstZones.FormattingEnabled = true;
      this._LstZones.Location = new System.Drawing.Point(6, 12);
      this._LstZones.Name = "_LstZones";
      this._LstZones.Size = new System.Drawing.Size(199, 381);
      this._LstZones.TabIndex = 0;
      this._LstZones.SelectedIndexChanged += new System.EventHandler(this._LstZones_SelectedIndexChanged);
      // 
      // _GrpBottom
      // 
      this._GrpBottom.Controls.Add(this.panel2);
      this._GrpBottom.Controls.Add(this.panel1);
      this._GrpBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._GrpBottom.Location = new System.Drawing.Point(0, 407);
      this._GrpBottom.Name = "_GrpBottom";
      this._GrpBottom.Size = new System.Drawing.Size(719, 47);
      this._GrpBottom.TabIndex = 2;
      this._GrpBottom.TabStop = false;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this._BtnOk);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel2.Location = new System.Drawing.Point(544, 16);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(87, 28);
      this.panel2.TabIndex = 3;
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(3, 2);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(75, 23);
      this._BtnOk.TabIndex = 0;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      this._BtnOk.Click += new System.EventHandler(this._BtnOk_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._BtnCancel);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel1.Location = new System.Drawing.Point(631, 16);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(85, 28);
      this.panel1.TabIndex = 2;
      // 
      // _BtnCancel
      // 
      this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._BtnCancel.Location = new System.Drawing.Point(3, 2);
      this._BtnCancel.Name = "_BtnCancel";
      this._BtnCancel.Size = new System.Drawing.Size(75, 23);
      this._BtnCancel.TabIndex = 1;
      this._BtnCancel.Text = "&Cancel";
      this._BtnCancel.UseVisualStyleBackColor = true;
      this._BtnCancel.Click += new System.EventHandler(this._BtnCancel_Click);
      // 
      // _Sprite
      // 
      this._Sprite.AllowFrameClick = true;
      this._Sprite.Animation = null;
      this._Sprite.CaptionVisible = false;
      this._Sprite.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Sprite.Frame = null;
      this._Sprite.FrameColor = System.Drawing.Color.White;
      grid2.ForeColor = System.Drawing.Color.Black;
      grid2.Height = 32;
      grid2.OffsetX = 0;
      grid2.OffsetY = 0;
      grid2.Visible = true;
      grid2.Width = 32;
      this._Sprite.Grid = grid2;
      this._Sprite.GridColor = System.Drawing.Color.Black;
      this._Sprite.GridVisible = true;
      this._Sprite.Location = new System.Drawing.Point(0, 0);
      this._Sprite.Name = "_Sprite";
      this._Sprite.Project = null;
      this._Sprite.ShowImages = false;
      this._Sprite.Size = new System.Drawing.Size(343, 407);
      this._Sprite.SpriteBackColor = System.Drawing.SystemColors.Control;
      this._Sprite.TabIndex = 0;
      this._Sprite.Zoom = SpriteAnimationEditor.ZoomFactor.x1;
      this._Sprite.SpritePaint += new SpriteAnimationEditor.Controls.OutputSprite.GraphicsHandler(this._Sprite_SpritePaint);
      this._Sprite.FrameClicked += new SpriteAnimationEditor.Controls.OutputSprite.FrameClickedHandler(this._Sprite_FrameClicked);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(15, 85);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(60, 13);
      this.label9.TabIndex = 18;
      this.label9.Text = "Description";
      // 
      // _txtBSDescription
      // 
      this._txtBSDescription.Location = new System.Drawing.Point(18, 101);
      this._txtBSDescription.Name = "_txtBSDescription";
      this._txtBSDescription.Size = new System.Drawing.Size(129, 20);
      this._txtBSDescription.TabIndex = 17;
      // 
      // ColisionZonesForm
      // 
      this.AcceptButton = this._BtnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._BtnCancel;
      this.ClientSize = new System.Drawing.Size(719, 454);
      this.Controls.Add(this._Sprite);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this._GrpBottom);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ColisionZonesForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "Animation Colision Zones";
      this.Load += new System.EventHandler(this.ColisionZonesForm_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColisionZonesForm_FormClosing);
      this.Resize += new System.EventHandler(this.ColisionZonesForm_Resize);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this._grpBS.ResumeLayout(false);
      this._grpBS.PerformLayout();
      this._grpBB.ResumeLayout(false);
      this._grpBB.PerformLayout();
      this._GrpBottom.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private SpriteAnimationEditor.Controls.OutputSprite _Sprite;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox _GrpBottom;
    private System.Windows.Forms.Button _BtnCancel;
    private System.Windows.Forms.Button _BtnOk;
    private System.Windows.Forms.ListBox _LstZones;
    private System.Windows.Forms.Button _BtnDelete;
    private System.Windows.Forms.Button _BtnAdd;
    private System.Windows.Forms.Button _BtnUpdate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _txtHeight;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _txtY;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox _txtWidth;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _txtX;
    private System.Windows.Forms.RadioButton _rbBB;
    private System.Windows.Forms.GroupBox _grpBB;
    private System.Windows.Forms.GroupBox _grpBS;
    private System.Windows.Forms.TextBox _txtBSX;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox _txtBSRadius;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox _txtBSY;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.RadioButton _rbBs;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox _txtDescription;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox _txtBSDescription;
  }
}