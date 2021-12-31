namespace LevelEditor.Controls
{
  partial class PropertiesCtl
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
      this.group1 = new TwoBrains.Common.Controls.Group();
      this._ObjProps = new System.Windows.Forms.PropertyGrid();
      this.group1.BodyPanel.SuspendLayout();
      this.group1.SuspendLayout();
      this.SuspendLayout();
      // 
      // group1
      // 
      // 
      // group1._PanelBody
      // 
      this.group1.BodyPanel.BackColor = System.Drawing.SystemColors.Control;
      this.group1.BodyPanel.Controls.Add(this._ObjProps);
      this.group1.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this.group1.BodyPanel.Name = "_PanelBody";
      this.group1.BodyPanel.Size = new System.Drawing.Size(150, 132);
      this.group1.BodyPanel.TabIndex = 2;
      this.group1.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
      this.group1.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.group1.CaptionVisible = true;
      this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.group1.Location = new System.Drawing.Point(0, 0);
      this.group1.Name = "group1";
      this.group1.Size = new System.Drawing.Size(150, 150);
      this.group1.TabIndex = 0;
      this.group1.Text = "Properties";
      // 
      // _ObjProps
      // 
      this._ObjProps.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ObjProps.HelpVisible = false;
      this._ObjProps.Location = new System.Drawing.Point(0, 0);
      this._ObjProps.Name = "_ObjProps";
      this._ObjProps.Size = new System.Drawing.Size(150, 132);
      this._ObjProps.TabIndex = 1;
      this._ObjProps.ToolbarVisible = false;
      this._ObjProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this._ObjProps_PropertyValueChanged);
      // 
      // PropertiesCtl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.group1);
      this.Name = "PropertiesCtl";
      this.group1.BodyPanel.ResumeLayout(false);
      this.group1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private TwoBrains.Common.Controls.Group group1;
    private System.Windows.Forms.PropertyGrid _ObjProps;
  }
}
