namespace TwoBrainsGames.Snails.Winforms
{
    partial class ScreenModeForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenModeForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this._picSnails = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this._lblInfo = new System.Windows.Forms.Label();
            this._btnFullscreen = new System.Windows.Forms.Button();
            this._btnWindow = new System.Windows.Forms.Button();
            this._lblMessage = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picSnails)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this._picSnails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(369, 101);
            this.panel1.TabIndex = 0;
            // 
            // _picSnails
            // 
            this._picSnails.ImageLocation = "";
            this._picSnails.Location = new System.Drawing.Point(0, 0);
            this._picSnails.Name = "_picSnails";
            this._picSnails.Size = new System.Drawing.Size(186, 50);
            this._picSnails.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._picSnails.TabIndex = 0;
            this._picSnails.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._lblInfo);
            this.panel2.Controls.Add(this._btnFullscreen);
            this.panel2.Controls.Add(this._btnWindow);
            this.panel2.Controls.Add(this._lblMessage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(369, 115);
            this.panel2.TabIndex = 1;
            // 
            // _lblInfo
            // 
            this._lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lblInfo.Location = new System.Drawing.Point(0, 93);
            this._lblInfo.Name = "_lblInfo";
            this._lblInfo.Size = new System.Drawing.Size(369, 22);
            this._lblInfo.TabIndex = 3;
            this._lblInfo.Text = "You can change the screen mode later from the Options menu";
            this._lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _btnFullscreen
            // 
            this._btnFullscreen.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnFullscreen.Location = new System.Drawing.Point(195, 50);
            this._btnFullscreen.Name = "_btnFullscreen";
            this._btnFullscreen.Size = new System.Drawing.Size(97, 23);
            this._btnFullscreen.TabIndex = 2;
            this._btnFullscreen.Text = "Fullscreen";
            this._btnFullscreen.UseVisualStyleBackColor = true;
            this._btnFullscreen.Click += new System.EventHandler(this._btnFullscreen_Click);
            // 
            // _btnWindow
            // 
            this._btnWindow.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnWindow.Location = new System.Drawing.Point(77, 50);
            this._btnWindow.Name = "_btnWindow";
            this._btnWindow.Size = new System.Drawing.Size(97, 23);
            this._btnWindow.TabIndex = 1;
            this._btnWindow.Text = "Windowed";
            this._btnWindow.UseVisualStyleBackColor = true;
            this._btnWindow.Click += new System.EventHandler(this._btnWindow_Click);
            // 
            // _lblMessage
            // 
            this._lblMessage.AutoSize = true;
            this._lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblMessage.Location = new System.Drawing.Point(94, 19);
            this._lblMessage.Name = "_lblMessage";
            this._lblMessage.Size = new System.Drawing.Size(181, 13);
            this._lblMessage.TabIndex = 0;
            this._lblMessage.Text = "Please select the screen mode";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "brains-logo.png");
            this.imageList1.Images.SetKeyName(1, "snails-logo.png");
            // 
            // ScreenModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 216);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snails";
            this.Load += new System.EventHandler(this.ScreenModeForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picSnails)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label _lblInfo;
        private System.Windows.Forms.Button _btnFullscreen;
        private System.Windows.Forms.Button _btnWindow;
        private System.Windows.Forms.Label _lblMessage;
        private System.Windows.Forms.PictureBox _picSnails;
        private System.Windows.Forms.ImageList imageList1;
    }
}