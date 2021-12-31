namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    partial class EditableObjectList
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
            this._objList = new System.Windows.Forms.ListBox();
            this._btnAdd = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnClear = new System.Windows.Forms.Button();
            this._pnlButtons = new System.Windows.Forms.Panel();
            this._gpContainer = new System.Windows.Forms.GroupBox();
            this._pnlButtons.SuspendLayout();
            this._gpContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _objList
            // 
            this._objList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._objList.FormattingEnabled = true;
            this._objList.Location = new System.Drawing.Point(3, 16);
            this._objList.Name = "_objList";
            this._objList.Size = new System.Drawing.Size(150, 150);
            this._objList.Sorted = true;
            this._objList.TabIndex = 0;
            this._objList.SelectedIndexChanged += new System.EventHandler(this._objList_SelectedIndexChanged);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(6, 3);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(61, 23);
            this._btnAdd.TabIndex = 1;
            this._btnAdd.Text = "Add";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(6, 32);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(61, 23);
            this._btnRemove.TabIndex = 2;
            this._btnRemove.Text = "Remove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this._btnRemove_Click);
            // 
            // _btnClear
            // 
            this._btnClear.Location = new System.Drawing.Point(6, 61);
            this._btnClear.Name = "_btnClear";
            this._btnClear.Size = new System.Drawing.Size(61, 23);
            this._btnClear.TabIndex = 3;
            this._btnClear.Text = "Clear";
            this._btnClear.UseVisualStyleBackColor = true;
            this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
            // 
            // _pnlButtons
            // 
            this._pnlButtons.Controls.Add(this._btnAdd);
            this._pnlButtons.Controls.Add(this._btnClear);
            this._pnlButtons.Controls.Add(this._btnRemove);
            this._pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this._pnlButtons.Location = new System.Drawing.Point(153, 16);
            this._pnlButtons.Name = "_pnlButtons";
            this._pnlButtons.Size = new System.Drawing.Size(70, 150);
            this._pnlButtons.TabIndex = 4;
            // 
            // _gpContainer
            // 
            this._gpContainer.Controls.Add(this._objList);
            this._gpContainer.Controls.Add(this._pnlButtons);
            this._gpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gpContainer.Location = new System.Drawing.Point(0, 0);
            this._gpContainer.Name = "_gpContainer";
            this._gpContainer.Size = new System.Drawing.Size(226, 169);
            this._gpContainer.TabIndex = 5;
            this._gpContainer.TabStop = false;
            // 
            // EditableObjectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gpContainer);
            this.Name = "EditableObjectList";
            this.Size = new System.Drawing.Size(226, 169);
            this._pnlButtons.ResumeLayout(false);
            this._gpContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _objList;
        private System.Windows.Forms.Button _btnAdd;
        private System.Windows.Forms.Button _btnRemove;
        private System.Windows.Forms.Button _btnClear;
        private System.Windows.Forms.Panel _pnlButtons;
        private System.Windows.Forms.GroupBox _gpContainer;
    }
}
