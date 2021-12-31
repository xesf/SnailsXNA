namespace TwoBrainsGames.Snails.Debuging
{
    partial class SettingsInfo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._txtShadowDepth = new System.Windows.Forms.NumericUpDown();
            this._cbShowOOBB = new System.Windows.Forms.CheckBox();
            this._cbShowShadows = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._cbShowPaths = new System.Windows.Forms.CheckBox();
            this._cbShowCoordinates = new System.Windows.Forms.CheckBox();
            this._cbShowFrames = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._txtGravity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._chkShowIds = new System.Windows.Forms.CheckBox();
            this._chkEntranceActive = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtShadowDepth)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtGravity)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._chkEntranceActive);
            this.groupBox1.Controls.Add(this._chkShowIds);
            this.groupBox1.Controls.Add(this._txtShadowDepth);
            this.groupBox1.Controls.Add(this._cbShowOOBB);
            this.groupBox1.Controls.Add(this._cbShowShadows);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 115);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Objects";
            // 
            // _txtShadowDepth
            // 
            this._txtShadowDepth.Location = new System.Drawing.Point(112, 16);
            this._txtShadowDepth.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._txtShadowDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtShadowDepth.Name = "_txtShadowDepth";
            this._txtShadowDepth.Size = new System.Drawing.Size(44, 20);
            this._txtShadowDepth.TabIndex = 10;
            this._txtShadowDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _cbShowOOBB
            // 
            this._cbShowOOBB.AutoSize = true;
            this._cbShowOOBB.Location = new System.Drawing.Point(6, 40);
            this._cbShowOOBB.Name = "_cbShowOOBB";
            this._cbShowOOBB.Size = new System.Drawing.Size(133, 17);
            this._cbShowOOBB.TabIndex = 9;
            this._cbShowOOBB.Text = "Show Bounding Boxes";
            this._cbShowOOBB.UseVisualStyleBackColor = true;
            this._cbShowOOBB.CheckedChanged += new System.EventHandler(this._cbShowOOBB_CheckedChanged);
            // 
            // _cbShowShadows
            // 
            this._cbShowShadows.AutoSize = true;
            this._cbShowShadows.Location = new System.Drawing.Point(6, 19);
            this._cbShowShadows.Name = "_cbShowShadows";
            this._cbShowShadows.Size = new System.Drawing.Size(100, 17);
            this._cbShowShadows.TabIndex = 1;
            this._cbShowShadows.Text = "Show Shadows";
            this._cbShowShadows.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._cbShowPaths);
            this.groupBox2.Controls.Add(this._cbShowCoordinates);
            this.groupBox2.Controls.Add(this._cbShowFrames);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(165, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(131, 115);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Board";
            // 
            // _cbShowPaths
            // 
            this._cbShowPaths.AutoSize = true;
            this._cbShowPaths.Location = new System.Drawing.Point(6, 63);
            this._cbShowPaths.Name = "_cbShowPaths";
            this._cbShowPaths.Size = new System.Drawing.Size(83, 17);
            this._cbShowPaths.TabIndex = 5;
            this._cbShowPaths.Text = "Show Paths";
            this._cbShowPaths.UseVisualStyleBackColor = true;
            this._cbShowPaths.CheckedChanged += new System.EventHandler(this._cbShowPaths_CheckedChanged);
            // 
            // _cbShowCoordinates
            // 
            this._cbShowCoordinates.AutoSize = true;
            this._cbShowCoordinates.Location = new System.Drawing.Point(6, 40);
            this._cbShowCoordinates.Name = "_cbShowCoordinates";
            this._cbShowCoordinates.Size = new System.Drawing.Size(112, 17);
            this._cbShowCoordinates.TabIndex = 4;
            this._cbShowCoordinates.Text = "Show Coordinates";
            this._cbShowCoordinates.UseVisualStyleBackColor = true;
            this._cbShowCoordinates.CheckedChanged += new System.EventHandler(this._cbShowCoordinates_CheckedChanged);
            // 
            // _cbShowFrames
            // 
            this._cbShowFrames.AutoSize = true;
            this._cbShowFrames.Location = new System.Drawing.Point(6, 19);
            this._cbShowFrames.Name = "_cbShowFrames";
            this._cbShowFrames.Size = new System.Drawing.Size(90, 17);
            this._cbShowFrames.TabIndex = 3;
            this._cbShowFrames.Text = "Show Frames";
            this._cbShowFrames.UseVisualStyleBackColor = true;
            this._cbShowFrames.CheckedChanged += new System.EventHandler(this._cbShowFrames_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._txtGravity);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(296, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(136, 115);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Game";
            // 
            // _txtGravity
            // 
            this._txtGravity.Location = new System.Drawing.Point(53, 16);
            this._txtGravity.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this._txtGravity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtGravity.Name = "_txtGravity";
            this._txtGravity.Size = new System.Drawing.Size(53, 20);
            this._txtGravity.TabIndex = 1;
            this._txtGravity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._txtGravity.ValueChanged += new System.EventHandler(this._txtGravity_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Gravity";
            // 
            // _chkShowIds
            // 
            this._chkShowIds.AutoSize = true;
            this._chkShowIds.Location = new System.Drawing.Point(6, 63);
            this._chkShowIds.Name = "_chkShowIds";
            this._chkShowIds.Size = new System.Drawing.Size(104, 17);
            this._chkShowIds.TabIndex = 11;
            this._chkShowIds.Text = "Show Object Ids";
            this._chkShowIds.UseVisualStyleBackColor = true;
            this._chkShowIds.CheckedChanged += new System.EventHandler(this._chkShowIds_CheckedChanged);
            // 
            // _chkEntranceActive
            // 
            this._chkEntranceActive.AutoSize = true;
            this._chkEntranceActive.Location = new System.Drawing.Point(6, 86);
            this._chkEntranceActive.Name = "_chkEntranceActive";
            this._chkEntranceActive.Size = new System.Drawing.Size(107, 17);
            this._chkEntranceActive.TabIndex = 12;
            this._chkEntranceActive.Text = "Entrances Active";
            this._chkEntranceActive.UseVisualStyleBackColor = true;
            // 
            // SettingsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsInfo";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(435, 121);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtShadowDepth)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._txtGravity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _cbShowOOBB;
        private System.Windows.Forms.CheckBox _cbShowShadows;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox _cbShowCoordinates;
        private System.Windows.Forms.CheckBox _cbShowFrames;
        private System.Windows.Forms.NumericUpDown _txtShadowDepth;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown _txtGravity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _cbShowPaths;
        private System.Windows.Forms.CheckBox _chkShowIds;
        private System.Windows.Forms.CheckBox _chkEntranceActive;
    }
}
