namespace TwoBrainsGames.Snails.StageEditor.Forms
{
	partial class StageDataEditorForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._pnlTile = new System.Windows.Forms.Panel();
            this._cmbBottomPath = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._cmbRightPath = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._cmbLeftPath = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._cmbTopPath = new TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox();
            this._pnlImage = new System.Windows.Forms.Panel();
            this._btnUndoTileChanges = new System.Windows.Forms.Button();
            this._txtTileId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this._udFrameNr = new System.Windows.Forms.NumericUpDown();
            this._udStyleGroupId = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._lstTiles = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._pnlObjects = new System.Windows.Forms.Panel();
            this._btnUndoObjChanges = new System.Windows.Forms.Button();
            this._cmbType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._chkCanDieWithCrates = new System.Windows.Forms.CheckBox();
            this._chkCanDieWithAnyTypeOfExplosion = new System.Windows.Forms.CheckBox();
            this._chkCanDie = new System.Windows.Forms.CheckBox();
            this._chkCanDieWithExplosions = new System.Windows.Forms.CheckBox();
            this._chkCanWalkOnWalls = new System.Windows.Forms.CheckBox();
            this._chkCanWalk = new System.Windows.Forms.CheckBox();
            this._chkCanHoover = new System.Windows.Forms.CheckBox();
            this._chkCanCollide = new System.Windows.Forms.CheckBox();
            this._chkCanFall = new System.Windows.Forms.CheckBox();
            this._txtSprite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._txtResource = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._txtId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._lstObjects = new System.Windows.Forms.ListBox();
            this._btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this._pnlTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._udFrameNr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._udStyleGroupId)).BeginInit();
            this.groupBox2.SuspendLayout();
            this._pnlObjects.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._pnlTile);
            this.groupBox1.Controls.Add(this._lstTiles);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 328);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tiles";
            // 
            // _pnlTile
            // 
            this._pnlTile.Controls.Add(this._cmbBottomPath);
            this._pnlTile.Controls.Add(this._cmbRightPath);
            this._pnlTile.Controls.Add(this._cmbLeftPath);
            this._pnlTile.Controls.Add(this._cmbTopPath);
            this._pnlTile.Controls.Add(this._pnlImage);
            this._pnlTile.Controls.Add(this._btnUndoTileChanges);
            this._pnlTile.Controls.Add(this._txtTileId);
            this._pnlTile.Controls.Add(this.label7);
            this._pnlTile.Controls.Add(this._udFrameNr);
            this._pnlTile.Controls.Add(this._udStyleGroupId);
            this._pnlTile.Controls.Add(this.label2);
            this._pnlTile.Controls.Add(this.label1);
            this._pnlTile.Location = new System.Drawing.Point(142, 19);
            this._pnlTile.Name = "_pnlTile";
            this._pnlTile.Size = new System.Drawing.Size(245, 303);
            this._pnlTile.TabIndex = 3;
            // 
            // _cmbBottomPath
            // 
            this._cmbBottomPath.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PathBehaviour;
            this._cmbBottomPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbBottomPath.FormattingEnabled = true;
            this._cmbBottomPath.Location = new System.Drawing.Point(83, 210);
            this._cmbBottomPath.Name = "_cmbBottomPath";
            this._cmbBottomPath.Size = new System.Drawing.Size(75, 21);
            this._cmbBottomPath.TabIndex = 21;
            // 
            // _cmbRightPath
            // 
            this._cmbRightPath.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PathBehaviour;
            this._cmbRightPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRightPath.FormattingEnabled = true;
            this._cmbRightPath.Location = new System.Drawing.Point(156, 156);
            this._cmbRightPath.Name = "_cmbRightPath";
            this._cmbRightPath.Size = new System.Drawing.Size(75, 21);
            this._cmbRightPath.TabIndex = 20;
            // 
            // _cmbLeftPath
            // 
            this._cmbLeftPath.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PathBehaviour;
            this._cmbLeftPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbLeftPath.FormattingEnabled = true;
            this._cmbLeftPath.Location = new System.Drawing.Point(6, 156);
            this._cmbLeftPath.Name = "_cmbLeftPath";
            this._cmbLeftPath.Size = new System.Drawing.Size(75, 21);
            this._cmbLeftPath.TabIndex = 19;
            // 
            // _cmbTopPath
            // 
            this._cmbTopPath.ComboType = TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox.ComboBoxType.PathBehaviour;
            this._cmbTopPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTopPath.FormattingEnabled = true;
            this._cmbTopPath.Location = new System.Drawing.Point(83, 114);
            this._cmbTopPath.Name = "_cmbTopPath";
            this._cmbTopPath.Size = new System.Drawing.Size(75, 21);
            this._cmbTopPath.TabIndex = 18;
            // 
            // _pnlImage
            // 
            this._pnlImage.Location = new System.Drawing.Point(87, 141);
            this._pnlImage.Name = "_pnlImage";
            this._pnlImage.Size = new System.Drawing.Size(63, 63);
            this._pnlImage.TabIndex = 17;
            this._pnlImage.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlImage_Paint);
            // 
            // _btnUndoTileChanges
            // 
            this._btnUndoTileChanges.Location = new System.Drawing.Point(139, 254);
            this._btnUndoTileChanges.Name = "_btnUndoTileChanges";
            this._btnUndoTileChanges.Size = new System.Drawing.Size(106, 23);
            this._btnUndoTileChanges.TabIndex = 16;
            this._btnUndoTileChanges.Text = "Undo Changes";
            this._btnUndoTileChanges.UseVisualStyleBackColor = true;
            this._btnUndoTileChanges.Click += new System.EventHandler(this._btnUndoTileChanges_Click);
            // 
            // _txtTileId
            // 
            this._txtTileId.Location = new System.Drawing.Point(83, 15);
            this._txtTileId.Name = "_txtTileId";
            this._txtTileId.Size = new System.Drawing.Size(87, 20);
            this._txtTileId.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Id";
            // 
            // _udFrameNr
            // 
            this._udFrameNr.Location = new System.Drawing.Point(83, 41);
            this._udFrameNr.Name = "_udFrameNr";
            this._udFrameNr.Size = new System.Drawing.Size(57, 20);
            this._udFrameNr.TabIndex = 3;
            // 
            // _udStyleGroupId
            // 
            this._udStyleGroupId.Location = new System.Drawing.Point(83, 67);
            this._udStyleGroupId.Name = "_udStyleGroupId";
            this._udStyleGroupId.Size = new System.Drawing.Size(57, 20);
            this._udStyleGroupId.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Style Group Id";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Frame Nr";
            // 
            // _lstTiles
            // 
            this._lstTiles.FormattingEnabled = true;
            this._lstTiles.Location = new System.Drawing.Point(16, 19);
            this._lstTiles.Name = "_lstTiles";
            this._lstTiles.Size = new System.Drawing.Size(120, 303);
            this._lstTiles.TabIndex = 0;
            this._lstTiles.SelectedIndexChanged += new System.EventHandler(this._lstTiles_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._pnlObjects);
            this.groupBox2.Controls.Add(this._lstObjects);
            this.groupBox2.Location = new System.Drawing.Point(411, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 327);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Objects";
            // 
            // _pnlObjects
            // 
            this._pnlObjects.Controls.Add(this._btnUndoObjChanges);
            this._pnlObjects.Controls.Add(this._cmbType);
            this._pnlObjects.Controls.Add(this.label3);
            this._pnlObjects.Controls.Add(this.groupBox3);
            this._pnlObjects.Controls.Add(this._txtSprite);
            this._pnlObjects.Controls.Add(this.label4);
            this._pnlObjects.Controls.Add(this._txtResource);
            this._pnlObjects.Controls.Add(this.label5);
            this._pnlObjects.Controls.Add(this._txtId);
            this._pnlObjects.Controls.Add(this.label6);
            this._pnlObjects.Location = new System.Drawing.Point(142, 19);
            this._pnlObjects.Name = "_pnlObjects";
            this._pnlObjects.Size = new System.Drawing.Size(355, 302);
            this._pnlObjects.TabIndex = 3;
            // 
            // _btnUndoObjChanges
            // 
            this._btnUndoObjChanges.Location = new System.Drawing.Point(158, 276);
            this._btnUndoObjChanges.Name = "_btnUndoObjChanges";
            this._btnUndoObjChanges.Size = new System.Drawing.Size(107, 23);
            this._btnUndoObjChanges.TabIndex = 4;
            this._btnUndoObjChanges.Text = "Undo Changes";
            this._btnUndoObjChanges.UseVisualStyleBackColor = true;
            this._btnUndoObjChanges.Click += new System.EventHandler(this._btnUndoObjChanges_Click);
            // 
            // _cmbType
            // 
            this._cmbType.FormattingEnabled = true;
            this._cmbType.Location = new System.Drawing.Point(77, 9);
            this._cmbType.Name = "_cmbType";
            this._cmbType.Size = new System.Drawing.Size(149, 21);
            this._cmbType.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Type";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._chkCanDieWithCrates);
            this.groupBox3.Controls.Add(this._chkCanDieWithAnyTypeOfExplosion);
            this.groupBox3.Controls.Add(this._chkCanDie);
            this.groupBox3.Controls.Add(this._chkCanDieWithExplosions);
            this.groupBox3.Controls.Add(this._chkCanWalkOnWalls);
            this.groupBox3.Controls.Add(this._chkCanWalk);
            this.groupBox3.Controls.Add(this._chkCanHoover);
            this.groupBox3.Controls.Add(this._chkCanCollide);
            this.groupBox3.Controls.Add(this._chkCanFall);
            this.groupBox3.Location = new System.Drawing.Point(10, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(342, 152);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Static Flags";
            // 
            // _chkCanDieWithCrates
            // 
            this._chkCanDieWithCrates.AutoSize = true;
            this._chkCanDieWithCrates.Location = new System.Drawing.Point(19, 129);
            this._chkCanDieWithCrates.Name = "_chkCanDieWithCrates";
            this._chkCanDieWithCrates.Size = new System.Drawing.Size(113, 17);
            this._chkCanDieWithCrates.TabIndex = 8;
            this._chkCanDieWithCrates.Text = "CanDieWithCrates";
            this._chkCanDieWithCrates.UseVisualStyleBackColor = true;
            // 
            // _chkCanDieWithAnyTypeOfExplosion
            // 
            this._chkCanDieWithAnyTypeOfExplosion.AutoSize = true;
            this._chkCanDieWithAnyTypeOfExplosion.Location = new System.Drawing.Point(19, 107);
            this._chkCanDieWithAnyTypeOfExplosion.Name = "_chkCanDieWithAnyTypeOfExplosion";
            this._chkCanDieWithAnyTypeOfExplosion.Size = new System.Drawing.Size(181, 17);
            this._chkCanDieWithAnyTypeOfExplosion.TabIndex = 7;
            this._chkCanDieWithAnyTypeOfExplosion.Text = "CanDieWithAnyTypeOfExplosion";
            this._chkCanDieWithAnyTypeOfExplosion.UseVisualStyleBackColor = true;
            // 
            // _chkCanDie
            // 
            this._chkCanDie.AutoSize = true;
            this._chkCanDie.Location = new System.Drawing.Point(208, 63);
            this._chkCanDie.Name = "_chkCanDie";
            this._chkCanDie.Size = new System.Drawing.Size(61, 17);
            this._chkCanDie.TabIndex = 6;
            this._chkCanDie.Text = "CanDie";
            this._chkCanDie.UseVisualStyleBackColor = true;
            // 
            // _chkCanDieWithExplosions
            // 
            this._chkCanDieWithExplosions.AutoSize = true;
            this._chkCanDieWithExplosions.Location = new System.Drawing.Point(208, 42);
            this._chkCanDieWithExplosions.Name = "_chkCanDieWithExplosions";
            this._chkCanDieWithExplosions.Size = new System.Drawing.Size(133, 17);
            this._chkCanDieWithExplosions.TabIndex = 5;
            this._chkCanDieWithExplosions.Text = "CanDieWithExplosions";
            this._chkCanDieWithExplosions.UseVisualStyleBackColor = true;
            this._chkCanDieWithExplosions.CheckedChanged += new System.EventHandler(this._chkCanDieWithExplosions_CheckedChanged);
            // 
            // _chkCanWalkOnWalls
            // 
            this._chkCanWalkOnWalls.AutoSize = true;
            this._chkCanWalkOnWalls.Location = new System.Drawing.Point(208, 19);
            this._chkCanWalkOnWalls.Name = "_chkCanWalkOnWalls";
            this._chkCanWalkOnWalls.Size = new System.Drawing.Size(110, 17);
            this._chkCanWalkOnWalls.TabIndex = 4;
            this._chkCanWalkOnWalls.Text = "CanWalkOnWalls";
            this._chkCanWalkOnWalls.UseVisualStyleBackColor = true;
            // 
            // _chkCanWalk
            // 
            this._chkCanWalk.AutoSize = true;
            this._chkCanWalk.Location = new System.Drawing.Point(19, 85);
            this._chkCanWalk.Name = "_chkCanWalk";
            this._chkCanWalk.Size = new System.Drawing.Size(70, 17);
            this._chkCanWalk.TabIndex = 3;
            this._chkCanWalk.Text = "CanWalk";
            this._chkCanWalk.UseVisualStyleBackColor = true;
            // 
            // _chkCanHoover
            // 
            this._chkCanHoover.AutoSize = true;
            this._chkCanHoover.Location = new System.Drawing.Point(19, 63);
            this._chkCanHoover.Name = "_chkCanHoover";
            this._chkCanHoover.Size = new System.Drawing.Size(80, 17);
            this._chkCanHoover.TabIndex = 2;
            this._chkCanHoover.Text = "CanHoover";
            this._chkCanHoover.UseVisualStyleBackColor = true;
            // 
            // _chkCanCollide
            // 
            this._chkCanCollide.AutoSize = true;
            this._chkCanCollide.Location = new System.Drawing.Point(19, 41);
            this._chkCanCollide.Name = "_chkCanCollide";
            this._chkCanCollide.Size = new System.Drawing.Size(76, 17);
            this._chkCanCollide.TabIndex = 1;
            this._chkCanCollide.Text = "CanCollide";
            this._chkCanCollide.UseVisualStyleBackColor = true;
            // 
            // _chkCanFall
            // 
            this._chkCanFall.AutoSize = true;
            this._chkCanFall.Location = new System.Drawing.Point(19, 19);
            this._chkCanFall.Name = "_chkCanFall";
            this._chkCanFall.Size = new System.Drawing.Size(61, 17);
            this._chkCanFall.TabIndex = 0;
            this._chkCanFall.Text = "CanFall";
            this._chkCanFall.UseVisualStyleBackColor = true;
            // 
            // _txtSprite
            // 
            this._txtSprite.Location = new System.Drawing.Point(77, 88);
            this._txtSprite.Name = "_txtSprite";
            this._txtSprite.Size = new System.Drawing.Size(149, 20);
            this._txtSprite.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Id";
            // 
            // _txtResource
            // 
            this._txtResource.Location = new System.Drawing.Point(77, 62);
            this._txtResource.Name = "_txtResource";
            this._txtResource.Size = new System.Drawing.Size(149, 20);
            this._txtResource.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Resource";
            // 
            // _txtId
            // 
            this._txtId.Location = new System.Drawing.Point(77, 36);
            this._txtId.Name = "_txtId";
            this._txtId.Size = new System.Drawing.Size(149, 20);
            this._txtId.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Sprite";
            // 
            // _lstObjects
            // 
            this._lstObjects.FormattingEnabled = true;
            this._lstObjects.Location = new System.Drawing.Point(16, 18);
            this._lstObjects.Name = "_lstObjects";
            this._lstObjects.Size = new System.Drawing.Size(120, 303);
            this._lstObjects.Sorted = true;
            this._lstObjects.TabIndex = 0;
            this._lstObjects.SelectedIndexChanged += new System.EventHandler(this._lstObjects_SelectedIndexChanged);
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(378, 346);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 2;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // StageDataEditorForm
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 381);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StageDataEditorForm";
            this.Text = "StageData Editor";
            this.Load += new System.EventHandler(this.StageDataEditorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this._pnlTile.ResumeLayout(false);
            this._pnlTile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._udFrameNr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._udStyleGroupId)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this._pnlObjects.ResumeLayout(false);
            this._pnlObjects.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox _lstTiles;
		private System.Windows.Forms.NumericUpDown _udFrameNr;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown _udStyleGroupId;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox _lstObjects;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox _txtSprite;
		private System.Windows.Forms.TextBox _txtResource;
		private System.Windows.Forms.TextBox _txtId;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox _cmbType;
		private System.Windows.Forms.CheckBox _chkCanHoover;
		private System.Windows.Forms.CheckBox _chkCanCollide;
		private System.Windows.Forms.CheckBox _chkCanFall;
		private System.Windows.Forms.CheckBox _chkCanDie;
		private System.Windows.Forms.CheckBox _chkCanDieWithExplosions;
		private System.Windows.Forms.CheckBox _chkCanWalkOnWalls;
		private System.Windows.Forms.CheckBox _chkCanWalk;
		private System.Windows.Forms.Button _btnOk;
		private System.Windows.Forms.Panel _pnlTile;
        private System.Windows.Forms.Panel _pnlObjects;
		private System.Windows.Forms.TextBox _txtTileId;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button _btnUndoTileChanges;
		private System.Windows.Forms.Button _btnUndoObjChanges;
        private System.Windows.Forms.Panel _pnlImage;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cmbBottomPath;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cmbRightPath;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cmbLeftPath;
        private TwoBrainsGames.Snails.StageEditor.Controls.BaseComboBox _cmbTopPath;
        private System.Windows.Forms.CheckBox _chkCanDieWithAnyTypeOfExplosion;
        private System.Windows.Forms.CheckBox _chkCanDieWithCrates;
	}
}