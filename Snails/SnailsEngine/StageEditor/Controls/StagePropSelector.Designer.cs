namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    partial class StagePropSelector
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
            this._propsList = new TwoBrainsGames.Snails.StageEditor.Controls.ImageListx();
            this.group1.BodyPanel.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // group1
            // 
            this.group1.AllowCollapse = true;
            // 
            // group1._PanelBody
            // 
            this.group1.BodyPanel.BackColor = System.Drawing.SystemColors.Control;
            this.group1.BodyPanel.Controls.Add(this._propsList);
            this.group1.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.BodyPanel.Location = new System.Drawing.Point(0, 18);
            this.group1.BodyPanel.Name = "_PanelBody";
            this.group1.BodyPanel.Size = new System.Drawing.Size(150, 132);
            this.group1.BodyPanel.TabIndex = 2;
            this.group1.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.group1.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.group1.CaptionVisible = true;
            this.group1.Collapsed = false;
            this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.Location = new System.Drawing.Point(0, 0);
            this.group1.Name = "group1";
            this.group1.Size = new System.Drawing.Size(150, 150);
            this.group1.TabIndex = 0;
            this.group1.Text = "Props";
            // 
            // _propsList
            // 
            this._propsList.AutoScroll = true;
            this._propsList.BackColor = System.Drawing.Color.White;
            this._propsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._propsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propsList.Location = new System.Drawing.Point(0, 0);
            this._propsList.Name = "_propsList";
            this._propsList.Padding = new System.Windows.Forms.Padding(5);
            this._propsList.SelectedItem = null;
            this._propsList.Size = new System.Drawing.Size(150, 132);
            this._propsList.TabIndex = 0;
            this._propsList.SelectedItemChanged += new System.EventHandler(this._propsList_SelectedItemChanged);
            // 
            // StagePropSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group1);
            this.Name = "StagePropSelector";
            this.group1.BodyPanel.ResumeLayout(false);
            this.group1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TwoBrains.Common.Controls.Group group1;
        private ImageListx _propsList;
    }
}
