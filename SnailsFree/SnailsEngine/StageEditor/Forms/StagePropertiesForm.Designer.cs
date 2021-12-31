namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class StagePropertiesForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._lblBuildNr = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._btnDelete = new System.Windows.Forms.Button();
            this._btnEdit = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._btnDown = new System.Windows.Forms.Button();
            this._btnUp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._lstLayers = new System.Windows.Forms.ListBox();
            this._chkWithShadows = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this._txtId = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._lblGoldMedals = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._lblSilverMedals = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._lblBronzeMedals = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(186, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._lblBronzeMedals);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this._lblSilverMedals);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this._lblGoldMedals);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._lblBuildNr);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this._chkWithShadows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._txtId);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 253);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // _lblBuildNr
            // 
            this._lblBuildNr.AutoSize = true;
            this._lblBuildNr.Location = new System.Drawing.Point(415, 30);
            this._lblBuildNr.Name = "_lblBuildNr";
            this._lblBuildNr.Size = new System.Drawing.Size(19, 13);
            this._lblBuildNr.TabIndex = 10;
            this._lblBuildNr.Text = "[0]";
            this._lblBuildNr.Click += new System.EventHandler(this._lblBuildNr_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(315, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Build Nr:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._btnDelete);
            this.groupBox2.Controls.Add(this._btnEdit);
            this.groupBox2.Controls.Add(this._btnAdd);
            this.groupBox2.Controls.Add(this._btnDown);
            this.groupBox2.Controls.Add(this._btnUp);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this._lstLayers);
            this.groupBox2.Location = new System.Drawing.Point(21, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 160);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layers";
            // 
            // _btnDelete
            // 
            this._btnDelete.Location = new System.Drawing.Point(140, 131);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(56, 23);
            this._btnDelete.TabIndex = 12;
            this._btnDelete.Text = "&Delete";
            this._btnDelete.UseVisualStyleBackColor = true;
            this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
            // 
            // _btnEdit
            // 
            this._btnEdit.Location = new System.Drawing.Point(80, 131);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.Size = new System.Drawing.Size(54, 23);
            this._btnEdit.TabIndex = 11;
            this._btnEdit.Text = "&Edit";
            this._btnEdit.UseVisualStyleBackColor = true;
            this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(20, 130);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(54, 23);
            this._btnAdd.TabIndex = 10;
            this._btnAdd.Text = "&Add";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _btnDown
            // 
            this._btnDown.Location = new System.Drawing.Point(203, 58);
            this._btnDown.Name = "_btnDown";
            this._btnDown.Size = new System.Drawing.Size(51, 23);
            this._btnDown.TabIndex = 9;
            this._btnDown.Text = "Down";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += new System.EventHandler(this._btnDown_Click);
            // 
            // _btnUp
            // 
            this._btnUp.Location = new System.Drawing.Point(203, 28);
            this._btnUp.Name = "_btnUp";
            this._btnUp.Size = new System.Drawing.Size(51, 23);
            this._btnUp.TabIndex = 8;
            this._btnUp.Text = "Up";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += new System.EventHandler(this._btnUp_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 1;
            // 
            // _lstLayers
            // 
            this._lstLayers.FormattingEnabled = true;
            this._lstLayers.Location = new System.Drawing.Point(20, 28);
            this._lstLayers.Name = "_lstLayers";
            this._lstLayers.Size = new System.Drawing.Size(176, 95);
            this._lstLayers.TabIndex = 7;
            this._lstLayers.SelectedIndexChanged += new System.EventHandler(this._lstLayers_SelectedIndexChanged);
            // 
            // _chkWithShadows
            // 
            this._chkWithShadows.AutoSize = true;
            this._chkWithShadows.Location = new System.Drawing.Point(21, 54);
            this._chkWithShadows.Name = "_chkWithShadows";
            this._chkWithShadows.Size = new System.Drawing.Size(95, 17);
            this._chkWithShadows.TabIndex = 6;
            this._chkWithShadows.Text = "With Shadows";
            this._chkWithShadows.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Id";
            // 
            // _txtId
            // 
            this._txtId.Location = new System.Drawing.Point(52, 23);
            this._txtId.Name = "_txtId";
            this._txtId.Size = new System.Drawing.Size(121, 20);
            this._txtId.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 29);
            this.panel1.TabIndex = 2;
            // 
            // _lblGoldMedals
            // 
            this._lblGoldMedals.AutoSize = true;
            this._lblGoldMedals.Location = new System.Drawing.Point(415, 55);
            this._lblGoldMedals.Name = "_lblGoldMedals";
            this._lblGoldMedals.Size = new System.Drawing.Size(19, 13);
            this._lblGoldMedals.TabIndex = 12;
            this._lblGoldMedals.Text = "[0]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(315, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Gold Medals";
            // 
            // _lblSilverMedals
            // 
            this._lblSilverMedals.AutoSize = true;
            this._lblSilverMedals.Location = new System.Drawing.Point(415, 80);
            this._lblSilverMedals.Name = "_lblSilverMedals";
            this._lblSilverMedals.Size = new System.Drawing.Size(19, 13);
            this._lblSilverMedals.TabIndex = 14;
            this._lblSilverMedals.Text = "[0]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(315, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Silver Medals:";
            // 
            // _lblBronzeMedals
            // 
            this._lblBronzeMedals.AutoSize = true;
            this._lblBronzeMedals.Location = new System.Drawing.Point(415, 105);
            this._lblBronzeMedals.Name = "_lblBronzeMedals";
            this._lblBronzeMedals.Size = new System.Drawing.Size(19, 13);
            this._lblBronzeMedals.TabIndex = 16;
            this._lblBronzeMedals.Text = "[0]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(315, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Bronze Medals:";
            // 
            // StagePropertiesForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 288);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "StagePropertiesForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Stage Properties";
            this.Load += new System.EventHandler(this.StagePropertiesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _txtId;
        private System.Windows.Forms.CheckBox _chkWithShadows;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _btnDelete;
        private System.Windows.Forms.Button _btnEdit;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.Button _btnDown;
        private System.Windows.Forms.Button _btnUp;
        private System.Windows.Forms.ListBox _lstLayers;
		private System.Windows.Forms.Label _lblBuildNr;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _lblBronzeMedals;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label _lblSilverMedals;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label _lblGoldMedals;
        private System.Windows.Forms.Label label5;
    }
}