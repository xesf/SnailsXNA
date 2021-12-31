namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    partial class StageObjectsSelectorForm
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
            this._btnSelect = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._objList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _btnSelect
            // 
            this._btnSelect.Location = new System.Drawing.Point(117, 230);
            this._btnSelect.Name = "_btnSelect";
            this._btnSelect.Size = new System.Drawing.Size(75, 23);
            this._btnSelect.TabIndex = 0;
            this._btnSelect.Text = "&Select";
            this._btnSelect.UseVisualStyleBackColor = true;
            this._btnSelect.Click += new System.EventHandler(this._btnSelect_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(36, 230);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _objList
            // 
            this._objList.FormattingEnabled = true;
            this._objList.Location = new System.Drawing.Point(14, 12);
            this._objList.Name = "_objList";
            this._objList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._objList.Size = new System.Drawing.Size(201, 212);
            this._objList.Sorted = true;
            this._objList.TabIndex = 2;
            this._objList.SelectedIndexChanged += new System.EventHandler(this._objList_SelectedIndexChanged);
            // 
            // StageObjectsSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 264);
            this.Controls.Add(this._objList);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnSelect);
            this.Name = "StageObjectsSelectorForm";
            this.Text = "StageObjectsSelectorForm";
            this.Load += new System.EventHandler(this.StageObjectsSelectorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnSelect;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ListBox _objList;
    }
}